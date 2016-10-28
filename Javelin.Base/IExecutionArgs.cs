using System.Collections.Generic;

namespace Javelin.Base
{
	public interface IExecutionArgs
	{
		string this[int index] { get; }

		List<string> ToList();
	}
}