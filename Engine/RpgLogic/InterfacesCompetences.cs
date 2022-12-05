using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {
	/// <summary>
	/// Un test de compétence, avec une barre de dificulté.
	/// </summary>
	public interface ITest {
		/// <summary>
		/// Le seuil de difficulté, tel qu'utilisé dans la plupart des jdrs.
		/// </summary>
		int Difficulty { get; set; }
	}
	public interface ITestResult {
		/// <summary>
		/// Le test est-il réussit?
		/// </summary>
		bool IsSucces { get; }
	}

	public interface ICompetence {
		/// <summary>
		/// Effectue le test. Doit Assigner un objet à TestResult
		/// </summary>
		/// <param name="conditions">Les conditions, la difficulté du test.</param>
		/// <returns>true if test succeded.</returns>
		ITestResult DoTest( ITest conditions );
	}
}
