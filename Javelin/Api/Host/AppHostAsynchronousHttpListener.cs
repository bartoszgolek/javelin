using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Forcom.CDN.Wrapper.ServiceStack.Host;
using log4net;
using ServiceStack.Common.Web;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace Javelin.Api.Host
{
	/// <summary>
	/// Listener written using original ServiceStack's <see cref="AppHostHttpListenerLongRunningBase"/>
	/// source code with fixed ThreadPool usage.
	/// </summary>
	public abstract class AppHostAsynchronousHttpListener : AppHostHttpListenerBase
	{
		protected AppHostAsynchronousHttpListener(string serviceName, int workingThreadsMaxCount, int totalThreadsMaxCount,
												params Assembly[] assembliesWithServices)
			: base(serviceName, assembliesWithServices)
		{
			threadPoolManager = new ServiceStackThreadPoolManager(workingThreadsMaxCount, totalThreadsMaxCount);
		}

		private bool disposed;

		protected override void Dispose(bool disposing)
		{
			if (disposed) return;

			lock (this)
			{
				if (disposed) return;

				if (disposing)
				{
					threadPoolManager.Dispose();
				}

				// new shared cleanup logic
				disposed = true;

				base.Dispose(disposing);
			}
		}

		public override void Start(string urlBase)
		{
			Start(urlBase, Listen);
		}

		private bool IsListening
		{
			get { return IsStarted && Listener != null && Listener.IsListening; }
		}

		// Loop here to begin processing of new requests.
		private void Listen(object state)
		{
			while (IsListening)
			{
				if (Listener == null) return;

				try
				{
					Listener.BeginGetContext(ListenerCallback, Listener);
					listenForNextRequest.WaitOne();
				}
				catch (Exception ex)
				{
					logger.Error("Listen()", ex);
					return;
				}
				if (Listener == null) return;
			}
		}

		// Handle the processing of a request in here.
		private void ListenerCallback(IAsyncResult asyncResult)
		{
			var listener = asyncResult.AsyncState as HttpListener;
			HttpListenerContext context;

			if (listener == null) return;
			var isListening = listener.IsListening;

			try
			{
				if (!isListening)
				{
					logger.Debug("Ignoring ListenerCallback() as HttpListener is no longer listening");
					return;
				}
				// The EndGetContext() method, as with all Begin/End asynchronous methods in the .NET Framework,
				// blocks until there is a request to be processed or some type of data is available.
				context = listener.EndGetContext(asyncResult);
			}
			catch (Exception ex)
			{
				// You will get an exception when httpListener.Stop() is called
				// because there will be a thread stopped waiting on the .EndGetContext()
				// method, and again, that is just the way most Begin/End asynchronous
				// methods of the .NET Framework work.
				string errMsg = ex + ": " + isListening;
				logger.Warn(errMsg);
				return;
			}
			finally
			{
				// Once we know we have a request (or exception), we signal the other thread
				// so that it calls the BeginGetContext() (or possibly exits if we're not
				// listening any more) method to start handling the next incoming request
				// while we continue to process this request on a different thread.
				listenForNextRequest.Set();
			}

			logger.InfoFormat("{0} Request : {1}", context.Request.UserHostAddress, context.Request.RawUrl);

			RaiseReceiveWebRequest(context);

			var wasQueued = threadPoolManager.TryQueueWorkItem(obj =>
				{
					try
					{
						ProcessRequest(context);
					}
					catch (Exception ex)
					{
						logger.Error(string.Format("Error this.ProcessRequest(context): [{0}]: {1}", ex.GetType().Name, ex.Message), ex);

						var sb = new StringBuilder();
						sb.AppendLine("{");
						sb.AppendLine("\"ResponseStatus\":{");
						sb.AppendFormat(" \"ErrorCode\":{0},\n", ex.GetType().Name.EncodeJson());
						sb.AppendFormat(" \"Message\":{0},\n", ex.Message.EncodeJson());
						sb.AppendFormat(" \"StackTrace\":{0}\n", ex.StackTrace.EncodeJson());
						sb.AppendLine("}");
						sb.AppendLine("}");

						SendFaultResponse(context, 500, sb.ToString());
					}
				}, context);

			if (!wasQueued)
			{
				// TotalThreadsMaxCount exceeded, force client to quit.
				SendFaultResponse(context, 503);
			}
		}

		private void SendFaultResponse(HttpListenerContext context, int faultCode, string responseBody = null)
		{
			try
			{
				context.Response.StatusCode = faultCode;
				context.Response.ContentType = ContentType.Json;
				if (!string.IsNullOrEmpty(responseBody))
				{
					byte[] sbBytes = responseBody.ToUtf8Bytes();
					context.Response.OutputStream.Write(sbBytes, 0, sbBytes.Length);
				}
				context.Response.Close();
			}
			catch (Exception errorEx)
			{
				logger.Error(
					string.Format("Error this.ProcessRequest(context)(Exception while writing error to the response): [{0}]: {1}", errorEx.GetType().Name, errorEx.Message),
					errorEx);
			}
		}

		private readonly AutoResetEvent listenForNextRequest = new AutoResetEvent(false);
		private readonly ServiceStackThreadPoolManager threadPoolManager;
		private readonly ILog logger = LogManager.GetLogger(typeof(AppHostAsynchronousHttpListener));
	}
}