using System.Collections.Generic;
using System.Linq;
using Javelin.Base;

namespace Javelin
{
	internal class ExecutionArgs : IExecutionArgs
	{
		private readonly string[] args;

		public ExecutionArgs(IEnumerable<string> args)
		{
			this.args = args.ToArray();
		}

		public string this[int index]
		{
			get { return args[index]; }
		}

		public List<string> ToList()
		{
			return args.ToList();
		}
	}
}