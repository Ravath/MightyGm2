using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Process
{

	public enum FinishedState
	{
		Done, Canceled
	}

	public class IProcessEndArguments
	{
		public FinishedState FinishedState { get; set; }
	}
}
