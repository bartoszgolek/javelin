using System;
using Javelin.Api.Rest;

namespace Javelin.Tasks.Perspectiv
{
	internal class PerspectivManagementClient : RestClient
	{
		public PerspectivManagementClient(
			IRestReader reader, string host)
			: base(reader, host, ContentTypes.Json)
		{
		}

		public void StopJobs()
		{
			GetResponse<string>("IManagementService/Management/StopJobs");
		}

		public String[] ListActiveJobs()
		{
			return GetResponse<string[]>("IManagementService/Management/ListActiveJobs");
		}
	}
}