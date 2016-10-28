using System;

namespace Javelin.Api
{
	[Flags]
	public enum MethodVerbs
	{
		Get = 1,
		Post = 2,
		Put = 4,
		Delete = 8
	}
}