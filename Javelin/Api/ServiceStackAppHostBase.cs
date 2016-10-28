using System;
using System.Collections.Generic;
using System.Reflection;
using Funq;
using Javelin.Api.Host;
using log4net;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;

namespace Javelin.Api
{
	public abstract class ServiceStackAppHostBase : AppHostAsynchronousHttpListener
	{
		private readonly IContainerAdapter containerAdapter;

		protected ServiceStackAppHostBase(
			IContainerAdapter containerAdapter,
			string serviceName, params Assembly[] assembliesWithServices)
			: base(serviceName, 50, 5000, assembliesWithServices)
		{
			this.containerAdapter = containerAdapter;
		}

		public override void Configure(Container container)
		{
			ServiceExceptionHandler = UnhandledExceptionsHandler;
			container.Adapter = containerAdapter;

			foreach (var register in GetRegisters())
			{
				Routes.Add(register.Parameter, register.Path, GetVerbsString(register.Verbs));
			}
		}

		private static object UnhandledExceptionsHandler(IHttpRequest httpReq, object request, Exception exception)
		{
			var message = string.Format("Request processing failed: {0}", httpReq.PathInfo);
			Logger.Error(message, exception);
			throw exception;
		}

		protected abstract ServiceStackMethodRegistrationInfo[] GetRegisters();

		protected string GetVerbsString(MethodVerbs verbs)
		{
			var verbsList = new List<string>();
			if (verbs.HasFlag(MethodVerbs.Get))
				verbsList.Add("GET");
			if (verbs.HasFlag(MethodVerbs.Post))
				verbsList.Add("POST");
			if (verbs.HasFlag(MethodVerbs.Put))
				verbsList.Add("PUT");
			if (verbs.HasFlag(MethodVerbs.Delete))
				verbsList.Add("DELETE");

			return string.Join(",", verbsList);
		}

		private static readonly ILog Logger = LogManager.GetLogger(typeof(ServiceStackAppHostBase));
	}
}