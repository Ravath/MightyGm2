using MightyGm2.RPG.L5R4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R4
{
	public static class Misc
	{
		public static ElementSort Convert(this Anneau anneau)
		{
			switch (anneau)
			{
				case Anneau.Feu:
					return ElementSort.Feu;
				case Anneau.Air:
					return ElementSort.Air;
				case Anneau.Terre:
					return ElementSort.Terre;
				case Anneau.Eau:
					return ElementSort.Eau;
				case Anneau.Vide:
					return ElementSort.Vide;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
