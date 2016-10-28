using Autofac;
using Javelin.Api;
using Javelin.Api.Helpers;
using Javelin.Api.Host;
using Javelin.Api.Rest;
using Javelin.Api.Rest.Serializers;
using ServiceStack.Configuration;

namespace Javelin
{
	public class RestModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ServiceStackAppHost>();
			builder.RegisterType<RestReader>().As<IRestReader>();
			builder.RegisterGeneric(typeof(ServiceStackTemplate<>)).As(typeof(IServiceStackTemplate<>)).SingleInstance();
			builder.RegisterType<AutofacIocAdapter>().As<IContainerAdapter>().SingleInstance();

			builder.RegisterType<WebClientSerializerFactory>().As<IWebClientSerializerFactory>();
			builder.RegisterType<XmlWebClientSerializer>()
				.As<IWebClientSerializer>()
				.Keyed<IWebClientSerializer>(ContentTypes.Xml);
			builder.RegisterType<JsonWebClientSerializer>()
				.As<IWebClientSerializer>()
				.Keyed<IWebClientSerializer>(ContentTypes.Json);
		}
	}
}