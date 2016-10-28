using System;
using System.Net;
using log4net;

namespace Javelin.Api.Rest
{
	public abstract class RestClient
	{
		protected RestClient(IRestReader reader, string host, string contentType)
		{
			this.reader = reader;
			this.host = host;
			this.contentType = contentType;
		}

		public virtual TResponse GetResponse<TResponse>(string methodCall)
		{
			return ReadResponse<TResponse>(methodCall);
		}

		public virtual TResponse PostRequest<TResponse, TRequest>(string methodCall, TRequest requestData)
		{
			return DoRequest(methodCall, uri => reader.PostRequest<TResponse, TRequest>(uri, contentType, requestData));
		}

		private TResponse ReadResponse<TResponse>(string methodCall)
		{
			try
			{
				var uri = GetUri(methodCall);
				LogManager.GetLogger(GetType()).InfoFormat("Outgoing GET request: {0}", uri);
				return reader.GetResponse<TResponse>(uri, contentType).Content;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("UnknownErrorMessage", ex);
			}
		}

		private T DoRequest<T>(string methodCall, Func<string, RestReaderResponse<T>> postStrategy)
		{
			try
			{
				var uri = GetUri(methodCall);
				LogManager.GetLogger(GetType()).InfoFormat("Outgoing POST request: {0}", uri);
				return postStrategy(uri).Content;
			}
			catch (RestReaderException)
			{
				throw;
			}
			catch (UriFormatException)
			{
				throw;
			}
			catch (WebException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("UnknownErrorMessage", ex);
			}
		}

		private string GetUri(string method)
		{
			if (host.EndsWith("/"))
				return host + method;

			return String.Format("{0}/{1}", host, method);
		}

		private readonly string host;
		private readonly string contentType;
		private readonly IRestReader reader;
	}
}