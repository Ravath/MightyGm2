using MightyGm2.Engine.RpgDatabase;
using System;
using System.Linq;

namespace L5R4.JdrCore
{
	public class FiveRingsComplementParser : ComplementParser
	{
		public FiveRingsComplementParser(string complement) : base(complement) {}

		public RollAndKeep GetPool(int index)
		{
			string val = GetAt(index);
			foreach (char sep in new char[] { 'k', 'g' })
			{
				if (val.Contains(sep))
				{
					string[] dice = val.Split(sep);
					int.TryParse(dice[0], out int roll);
					int.TryParse(dice[1], out int keep);
					return new RollAndKeep(roll, keep);
				}
			}
			return new RollAndKeep(1, 1);
		}
	}
}
