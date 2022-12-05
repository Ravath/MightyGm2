using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Process
{
	public interface IProcessStep
	{
		/// <summary>
		/// Checks the current step meets every need in order to progress to next one.
		/// </summary>
		/// <param name="ErrorMessage">The reference tag to the error message.</param>
		/// <returns>True if can proced.</returns>
		bool CanProgress(out string ErrorMessageTag);
		/// <summary>
		/// Initialize the step process.
		/// </summary>
		/// <param name="process">The process itself.</param>
		void Init(IProcess process);
		/// <summary>
		/// Remove the step process modifications before going back to former step.
		/// Shall be Init^(-1).
		/// </summary>
		void Reset();
		/// <summary>
		/// Do some last modifications before preceding to next step.
		/// </summary>
		void PreprossNext();
		/// <summary>
		/// Remove the last modifications done before comming back from next step.
		/// Shall be PreprossNext^(-1).
		/// </summary>
		void PreprossReset();
		/// <summary>
		/// Get the message to display for presenting the step.
		/// </summary>
		/// <returns>A message to display.</returns>
		string GetStepMessage();
	}
}
