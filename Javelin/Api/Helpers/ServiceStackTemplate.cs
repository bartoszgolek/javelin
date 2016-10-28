using System;
using System.ServiceModel.Web;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace Javelin.Api.Helpers
{
	public class ServiceStackTemplate<T> : IServiceStackTemplate<T>
	{
		public IHttpResult DoAction<TResult>(Func<TResult> action)
		{
			try
			{
				return new HttpResult(action());
			}
			catch (WebFaultException<T> e)
			{
				return new HttpResult(e.Detail, e.StatusCode);
			}
		}

		public IHttpResult DoAction(Action action)
		{
			try
			{
				action();
				return new HttpResult();
			}
			catch (WebFaultException<T> e)
			{
				return new HttpResult(e.Detail, e.StatusCode);
			}
		}
	}
}