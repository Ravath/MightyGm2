using Engine.RpgLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Dice
{
	/// <summary>
	/// The interpretation of a rolled die.
	/// </summary>
	public class DiceResult
	{
		/// <summary>
		/// The result of the die.
		/// </summary>
		public int result;
		/// <summary>
		/// True if this die has to be used in the final result.
		/// </summary>
		public bool kept;
	}

	/// <summary>
	/// A pool and result of dice roll. used to implement a Decorator pattern for composing a roll formula.
	/// </summary>
	public interface IRoll
	{
		/// <summary>
		/// The number of faces the dice of the pool have.
		/// </summary>
		int NbrFace { get; }
		/// <summary>
		/// The final interpretation of the roll.
		/// </summary>
		int NetResult { get; }
		/// <summary>
		/// The raw result of the roll.
		/// </summary>
		IEnumerable<DiceResult> Result { get; }
		/// <summary>
		/// Throw the pool, and operate result.
		/// </summary>
		/// <returns></returns>
		List<DiceResult> Roll();
		/// <summary>
		/// Get the macro formula associated with this operation.
		/// </summary>
		/// <returns></returns>
		string ToMacro();
	}

	/// <summary>
	/// A Pool of dice to roll. The result is the sum of every kept die.
	/// </summary>
	public class Pool : IRoll
	{
		private List<DiceResult> _result = new List<DiceResult>();

		public IValue NbrDice { get; private set; }
		public int NbrFace { get; set; }
		public int NetResult { get { return _result.Sum(a => a.kept ? a.result : 0); } }
		public IEnumerable<DiceResult> Result { get { return _result; } }

		public Pool(IValue nbrDice, int nbrFace) { NbrDice = nbrDice; NbrFace = nbrFace; }
		public Pool(int nbrDice, int nbrFace) : this(new Value(nbrDice), nbrFace) { }

		public List<DiceResult> Roll()
		{
			_result.Clear();
			for (int i = 0; i < NbrDice.TotalValue; i++)
			{
				_result.Add(new DiceResult()
				{
					result = Dice.Roll.RollD(NbrFace),
					kept = true
				});
			}
			return _result;
		}

		public string ToMacro()
		{
			return NbrDice.TotalValue + "d" + NbrFace;
		}
	}
	
	/// <summary>
	/// A Pool of dice to roll. The result is the sum of every kept die.
	/// </summary>
	public class Intervalle : IRoll
	{
		private List<DiceResult> _result = new List<DiceResult>();

		private Value _nbrDice = new Value(1);
		public IValue NbrDice { get { return _nbrDice; } }
		public int NbrFace { get { return Max.TotalValue - Min.TotalValue; } }
		public int NetResult { get { return _result.Sum(a => a.kept ? a.result : 0); } }
		public IEnumerable<DiceResult> Result { get { return _result; } }

		public IValue Min { get; set; }
		public IValue Max { get; set; }

		public Intervalle(IValue min, IValue max) {
			Min = min;
			Max = max;
		}
		public Intervalle(int min, int max) : this(new Value(min), new Value(max)) { }

		public List<DiceResult> Roll()
		{
			_result.Clear();
			// Order min & max values
			int min = Min.TotalValue;
			int max = Max.TotalValue;
			if (max < min)
			{
				int t = max;
				max = min;
				min = t;
			}
			// Do the random
			_result.Add(new DiceResult()
			{
				result = Dice.Roll.Interval(min, max),
				kept = true
			});
			// Return
			return _result;
		}

		public string ToMacro()
		{
			int min = Min.TotalValue, max = Max.TotalValue;
			if(min == max)
			{
				return min+"";
			}
			else if (min<max)
			{
				return min + ":" + max;
			}
			else
			{
				return max + ":" + min;
			}
		}
	}

	/// <summary>
	/// Decorator base class.
	/// </summary>
	public abstract class RollResult : IRoll
	{
		protected static char UpperChar = 'h';
		protected static char LowerChar = 'l';

		public IRoll Prev { get; set; }
		public int NbrFace { get { return Prev.NbrFace; } }
		public virtual int NetResult { get { return Prev.NetResult; } }
		public IEnumerable<DiceResult> Result { get { return Prev.Result; } }

		public RollResult(IRoll prev)
		{
			Prev = prev;
		}

		public virtual List<DiceResult> Roll() { return Prev.Roll(); }

		public abstract string ToMacro();
	}

	public class CountOp : RollResult
	{
		public CountOp(IRoll prev) : base(prev) {}
		public override int NetResult { get { return Result.Count(r=>r.kept); } }

		public override string ToMacro()
		{
			return Prev.ToMacro()+"c";
		}
	}

	public class AddOp : RollResult
	{
		public Value Bonus { get; set; }
		public AddOp(IRoll prev, Value add) : base(prev) { Bonus = add; }
		public AddOp(IRoll prev, int add) : this(prev, new Value(add)) {}
		public override int NetResult { get { return Prev.NetResult+Bonus.TotalValue; } }

		public override string ToMacro()
		{
			int b = Bonus.TotalValue;
			if (b > 0) { return Prev.ToMacro() + "+" + b; }
			else if (b<0) { return Prev.ToMacro() + "-" + (-b); }
			else /* b==0 */ { return Prev.ToMacro(); }
		}
	}

	public abstract class IndividualFilterOp : RollResult
	{
		public IndividualFilterOp(IRoll prev) : base(prev){}

		public override List<DiceResult> Roll()
		{
			var res = Prev.Roll();
			foreach (var item in res)
			{
				if (item.kept)
				{
					item.kept = Keep(item);
				}
			}
			return res;
		}
		public abstract bool Keep(DiceResult result);
	}

	public abstract class KeepThreshold : IndividualFilterOp
	{
		public int Threshold { get; set; }
		public KeepThreshold(IRoll prev, int threshold) : base(prev){ Threshold = threshold; }
	}

	public class KeepHigherThan : KeepThreshold
	{
		public KeepHigherThan(IRoll prev, int threshold) : base(prev, threshold) { }
		public override bool Keep(DiceResult result)
		{
			return result.result >= Threshold;
		}
		public override string ToMacro()
		{
			return Prev.ToMacro() + ">=" + Threshold; 
		}
	}

	public class KeepLowerThan : KeepThreshold
	{
		public KeepLowerThan(IRoll prev, int threshold) : base(prev, threshold) { }
		public override bool Keep(DiceResult result)
		{
			return result.result <= Threshold;
		}
		public override string ToMacro()
		{
			return Prev.ToMacro() + "<=" + Threshold;
		}
	}

	public abstract class GeneralFilterOp : RollResult
	{
		public GeneralFilterOp(IRoll prev) : base(prev) { }

		public override List<DiceResult> Roll()
		{
			var res = Prev.Roll();
			Filter(res.Where(r => r.kept == true).ToList());
			return res;
		}

		public abstract void Filter(IEnumerable<DiceResult> enumerable);
	}

	public abstract class KeepDiceNumber : GeneralFilterOp
	{
		/// <summary>
		/// The number of dice to keep.
		/// </summary>
		public IValue Number { get; set; }
		public KeepDiceNumber(IRoll prev, IValue number) : base(prev) { Number = number; }
	}

	public class KeepHighestDice : KeepDiceNumber
	{
		public KeepHighestDice(IRoll prev, IValue number) : base(prev, number) { }
		public KeepHighestDice(IRoll prev, int number) : base(prev, new Value(number)) { }

		public override void Filter(IEnumerable<DiceResult> enumerable)
		{
			var sorted = enumerable.OrderByDescending(r => r.result);
			for (int i = Number.TotalValue; i < sorted.Count(); i++)
			{
				sorted.ElementAt(i).kept = false;
			}
		}

		public override string ToMacro()
		{
			return Prev.ToMacro() + "h" + Number.TotalValue;
		}
	}

	public class KeepLowestDice : KeepDiceNumber
	{
		public KeepLowestDice(IRoll prev, IValue number) : base(prev, number) { }
		public KeepLowestDice(IRoll prev, int number) : base(prev, new Value(number)) { }

		public override void Filter(IEnumerable<DiceResult> enumerable)
		{
			var sorted = enumerable.OrderBy(r => r.result);
			for (int i = Number.TotalValue; i < sorted.Count(); i++)
			{
				sorted.ElementAt(i).kept = false;
			}
		}

		public override string ToMacro()
		{
			return Prev.ToMacro() + "l" + Number.TotalValue;
		}
	}

	public abstract class ReRollOp : RollResult
	{
		public bool RerollOnce { get; set; }
		public bool DiscardPrevious { get; set; }

		public ReRollOp(IRoll prev) : base(prev) { RerollOnce = false; DiscardPrevious = true; }

		public override List<DiceResult> Roll()
		{
			List<DiceResult> res = Prev.Roll();
			int nbrdice = res.Count;
			for (int i = 0; i < (RerollOnce? nbrdice : res.Count); i++)
			{
				if (res[i].kept)
				{
					if (DoReroll(res[i]))
					{
						if(DiscardPrevious)
							res[i].kept = false;
						res.Add(new DiceResult()
						{
							kept = true,
							result = Dice.Roll.RollD(NbrFace)
						});
					}
				}
			}
			return res;
		}

		public abstract bool DoReroll(DiceResult res);
	}

	public abstract class RerollThreshold : ReRollOp
	{
		public int Threshold { get; set; }
		public RerollThreshold(IRoll prev, int threshold) : base(prev) { Threshold = threshold; }

		public override string ToMacro()
		{
			char op = 'a';
			if (DiscardPrevious) op = 'r';
			if (!RerollOnce) op = Char.ToUpper(op);
			return Prev.ToMacro() + op + GetComplChar + Threshold;
		}
		protected abstract char GetComplChar { get; }
	}

	public class RerollHigherThan : RerollThreshold
	{
		public RerollHigherThan(IRoll prev, int threshold) : base(prev, threshold) {}
		public override bool DoReroll(DiceResult res){ return res.result >= Threshold; }
		protected override char GetComplChar { get { return RollResult.UpperChar; } }
	}

	public class RerollLowerThan : RerollThreshold
	{
		public RerollLowerThan(IRoll prev, int threshold) : base(prev, threshold) { }
		public override bool DoReroll(DiceResult res) { return res.result <= Threshold; }
		protected override char GetComplChar { get { return RollResult.LowerChar; } }
	}

	public abstract class ExplodeOp : RollResult
	{
		public bool RerollOnce { get; set; }

		public ExplodeOp(IRoll prev) : base(prev) { RerollOnce = false; }

		public override List<DiceResult> Roll()
		{
			List<DiceResult> res = Prev.Roll();
			int nbrdice = res.Count;
			for (int i = 0; i < nbrdice; i++)
			{
				if (res[i].kept)
				{
					int r = 0;
					int relance = res[i].result;

					while (DoReroll(relance, r) && (!RerollOnce||r==0))
					{
						relance = Dice.Roll.RollD(NbrFace);
						res[i].result += relance;
						r++;
					}
				}
			}
			return res;
		}

		public abstract bool DoReroll(int diceResult, int nbrofReroll);
	}

	public abstract class ExplodeThreshold : ExplodeOp
	{
		public int Threshold { get; set; }
		public ExplodeThreshold(IRoll prev, int threshold) : base(prev) { Threshold = threshold; }

		public override string ToMacro()
		{
			char op = 'e';
			if (!RerollOnce) op = Char.ToUpper(op);
			return Prev.ToMacro() + op + GetComplChar + Threshold;
		}
		protected abstract char GetComplChar { get; }
	}

	public class ExplodeHigherThan : ExplodeThreshold
	{
		public ExplodeHigherThan(IRoll prev, int threshold) : base(prev, threshold) {}
		public override bool DoReroll(int diceResult, int nbrofReroll){ return diceResult >= Threshold; }
		protected override char GetComplChar { get { return RollResult.UpperChar; } }
	}

	public class ExplodeLowerThan : ExplodeThreshold
	{
		public ExplodeLowerThan(IRoll prev, int threshold) : base(prev, threshold) { }
		public override bool DoReroll(int diceResult, int nbrofReroll) { return diceResult <= Threshold; }
		protected override char GetComplChar { get { return RollResult.LowerChar; } }
	}

	/// <summary>
	/// Used in a succession of roll operations in order to skip some of them.
	/// </summary>
	public class OptionalOp : RollResult
	{
		public bool DoOp { get; set; }
		public IRoll Option { get; set; }

		public OptionalOp(IRoll prev, IRoll optionalOp) : base(prev) { Option = optionalOp; }

		public override List<DiceResult> Roll()
		{
			if (DoOp)
			{
				return Option.Roll();
			}
			else
			{
				return Prev.Roll();
			}
		}

		public override string ToMacro()
		{
			if (DoOp)
			{
				return Prev.ToMacro() + Option.ToMacro();
			}
			else
			{
				return Prev.ToMacro();
			}
		}
	}
}