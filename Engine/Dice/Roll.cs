using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Dice {
	public static class Roll
	{
		private static Random _generator = new Random();//the numbers generator.
		public static int RollD(int faces)
		{
			return _generator.Next(1, faces + 1);
		}
		public static int Interval(int min, int max)
		{
			return _generator.Next(min, max + 1);
		}
		public static int D20()
		{
			return RollD(20);
		}
		/// <summary>
		/// Lance les dés et en fait la somme.
		/// </summary>
		/// <param name="dice">Pool de dés à lancer.</param>
		/// <returns>La somme du lancer.</returns>
		public static int RollD( int nbr, int face ) {
			//Contract.Requires<ArgumentNullException>(dice != null);
			int res = 0;
			for (int i = 0; i < nbr; i++)
			{
				res += RollD(face);
			}
			return res;
		}
	}
}
