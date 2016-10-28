using System;
using Autofac.Features.Indexed;

namespace Javelin.Api.Rest
{
	internal class WebClientSerializerFactory : IWebClientSerializerFactory
	{
		private readonly IIndex<string, Func<IWebClientSerializer>> serializers;

		public WebClientSerializerFactory(IIndex<string, Func<IWebClientSerializer>> serializers)
		{
			this.serializers = serializers;
		}

		public IWebClientSerializer GetSerializer(string contentType)
		{
			Func<IWebClientSerializer> serializer;
			if (!serializers.TryGetValue(contentType, out serializer))
				throw new ArgumentException(String.Format("Unsuported content-type: '{0}'", contentType), "contentType");

			return serializer();
		}
	}
}