using System;

namespace Javelin.Api
{
	public class ServiceStackMethodRegistrationInfo
	{
		public ServiceStackMethodRegistrationInfo(MethodVerbs verbs, string path, Type parameter)
		{
			Parameter = parameter;
			Path = path;
			Verbs = verbs;
		}

		public MethodVerbs Verbs { get; private set; }
		public string Path { get; private set; }
		public Type Parameter { get; private set; }
	}
}