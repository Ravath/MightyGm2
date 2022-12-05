using Engine.RpgLogic;

namespace L5R.Model.Agent
{
	public class Money
	{
		public static int buToZeni = 10;
		public static int kokuToBu = 5;

		public Value Koku { get; }
		public Value Bu { get; }
		public Value Zeni { get; }

		public Money()
		{
			Koku = new Value();
			Bu = new Value();
			Zeni = new Value();
		}

		public void SetMoney(int koku, int bu, int zeni)
		{
			Koku.BaseValue = koku;
			Bu.BaseValue = bu;
			Zeni.BaseValue = zeni;
		}

		public void AddMoney(int koku, int bu, int zeni)
		{
			Koku.BaseValue += koku;
			Bu.BaseValue += bu;
			Zeni.BaseValue += zeni;
		}

		/// <summary>
		/// Optimize money repartition.
		/// </summary>
		public void Mend()
		{
			int savings = Zeni.BaseValue
				+ Bu.BaseValue * buToZeni
				+ Koku.BaseValue * buToZeni * kokuToBu;

			Koku.BaseValue = savings / (kokuToBu * buToZeni);
			savings = savings % (kokuToBu * buToZeni);
			Bu.BaseValue = savings / buToZeni;
			Zeni.BaseValue = savings % buToZeni;
		}

		public bool CanSpend(int koku, int bu, int zeni)
		{
			int spendings = zeni
				+ bu * buToZeni
				+ koku * buToZeni * kokuToBu;

			int savings = Zeni.BaseValue
				+ Bu.BaseValue * buToZeni
				+ Koku.BaseValue * buToZeni * kokuToBu;

			return savings > spendings;
		}

		public bool Spend(int koku, int bu, int zeni)
		{
			if(!CanSpend(koku, bu, zeni))
				return false;

			// Spend Koku
			if (koku <= Koku.BaseValue)
			{
				Koku.BaseValue -= koku;
				koku = 0;
			}
			else
			{
				koku -= Koku.BaseValue;
				Koku.BaseValue = 0;
			}

			// Spend Bu
			bu += koku * kokuToBu;
			koku = 0;
			if (bu <= Bu.BaseValue)
			{
				Bu.BaseValue -= bu;
				bu = 0;
			}
			else
			{
				bu -= Bu.BaseValue;
				Bu.BaseValue = 0;
			}

			// Spend Zeni
			zeni += bu * buToZeni;
			bu = 0;
			if (zeni <= Zeni.BaseValue)
			{
				Zeni.BaseValue -= zeni;
				zeni = 0;
			}
			else
			{
				zeni -= Zeni.BaseValue;
				Zeni.BaseValue = 0;
			}


			if(zeni != 0)
			{
				RoughSpend(zeni);
			}

			return true;
		}

		private void RoughSpend(int zeniToSpend)
		{
			int savings = Zeni.BaseValue
				+ Bu.BaseValue * buToZeni
				+ Koku.BaseValue * buToZeni * kokuToBu;

			savings -= zeniToSpend;

			//Mend
			Koku.BaseValue = savings / (kokuToBu * buToZeni);
			savings = savings % (kokuToBu * buToZeni);
			Bu.BaseValue = savings / buToZeni;
			Zeni.BaseValue = savings % buToZeni;
		}

	}
}
