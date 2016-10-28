using Autofac;

namespace Javelin.Api.Rest
{
	public static class ContainerExtender
	{
		public static void RegisterRestOperation<TParameter>(this ContainerBuilder builder, MethodVerbs verbs, string path)
		{
			builder.Register(c => new ServiceStackMethodRegistrationInfo(verbs, path, typeof(TParameter)));
		}
	}
}