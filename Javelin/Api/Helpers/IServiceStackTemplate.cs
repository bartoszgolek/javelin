using System;
using ServiceStack.ServiceHost;

namespace Javelin.Api.Helpers
{
	public interface IServiceStackTemplate<TFaultDetails>
	{
		IHttpResult DoAction<TResult>(Func<TResult> action);

		IHttpResult DoAction(Action action);
	}
}