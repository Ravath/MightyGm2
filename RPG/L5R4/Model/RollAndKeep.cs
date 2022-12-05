using Engine.Dice;
using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R4.JdrCore {

	/// <summary>
	/// Manager of the rolls done in 'Legend of the Five Rings'
	/// </summary>
	public class RollAndKeep : IRoll
	{
		public Pool _pool;
		private OptionalOp _specialisation;
		private OptionalOp _explosive;
		private KeepHighestDice _keep;

		#region IRoll Implementation
		public int NbrFace { get { return 10; } }
		public int NetResult{ get { return _keep.NetResult; } }
		public IEnumerable<DiceResult> Result { get { return _keep.Result; } }
		public List<DiceResult> Roll(){ return _keep.Roll(); }
		#endregion

		public IValue RollValue { get { return _pool.NbrDice; } }
		public virtual IValue KeepValue { get { return _keep.Number; } }
		public bool Reroll1 { get { return _specialisation.DoOp; } set { _specialisation.DoOp = value; } }
		public bool Explode { get { return _explosive.DoOp; } set { _explosive.DoOp = value; } }

		/// <summary>
		/// Dosn't reroll1 by default, but explode 10s.
		/// </summary>
		/// <param name="roll">Number of Dice to Roll.</param>
		/// <param name="keep">Number of Dice to Keep.</param>
		public RollAndKeep(IValue roll, IValue keep)
		{
			_pool = new Pool(roll, 10);
			_specialisation = new OptionalOp(_pool, new RerollLowerThan(_pool, 1));
			_explosive = new OptionalOp(_specialisation, new ExplodeHigherThan(_specialisation, 10));
			_keep = new KeepHighestDice(_explosive, keep);
			Reroll1 = false;
			Explode = true;
        }

        public RollAndKeep(L5R_Dice dice) : this(dice.Roll, dice.Keep) { }

        public RollAndKeep(int roll, int keep) : this(new Value(roll), new Value(keep)) { }


		//
		// Résumé :
		//     Retourne une chaîne qui représente l'objet actuel.
		//
		// Retourne :
		//     Chaîne qui représente l'objet actuel.
		public override string ToString()
		{
			return RollValue.TotalValue + "g" + KeepValue.TotalValue;
		}

		public string ToMacro()
		{
			return _pool.ToMacro();
		}
	}

	//public abstract class AbsRKPool : IPool {

	//	#region members
	//	private List<int> _result = new List<int>();
	//	#endregion

	//	public abstract IValue KeepIValue { get; }
	//	public abstract IValue RollIValue { get; }

	//	public virtual int Keep {
	//		get { return KeepIValue.TotalValue; }
	//		set { }
	//	}
	//	/// <summary>
	//	/// the natural numbre of dice to roll.
	//	/// Set the base value, but get the total value!!
	//	/// </summary>
	//	public virtual int Number {
	//		get { return RollIValue.TotalValue; }
	//		set { }
	//	}
	//	/// <summary>
	//	/// Always 10.
	//	/// </summary>
	//	public int Face {
	//		get { return 10; }
	//		set {/* nothing */}
	//	}
	//	/// <summary>
	//	/// All the results from the last roll..
	//	/// </summary>
	//	public IEnumerable<int> Results {
	//		get { return _result; }
	//	}

	//	/// <summary>
	//	/// Throw 'number' dice'.
	//	/// Sum the 'Keep' highest.
	//	/// </summary>
	//	/// <returns>The sum of the highest scores.</returns>
	//	public int GetResult() {
	//		Roll();
	//		int s = 0;
	//		int keep = Math.Min(Keep, _result.Count);
	//		for(int i = 0; i<keep; i++) {
	//			s += _result[i];
	//		}
	//		return s;
	//	}

	//	public void Roll() {
	//		_result.Clear();
	//		for(int i =0; i<Number; i++) {
	//			_result.Add(Roll.RollD(Face));
	//		}
	//		_result.Sort();
	//		_result.Reverse();
	//	}

	//	public override string ToString() {
	//		return String.Format("{0}g{1}",Number, Keep);
	//	}
	//}


	//public class CompositeRKPool : AbsRKPool
	//{

	//	private IValue _keep;
	//	private IValue _number;

	//	public override IValue KeepIValue
	//	{
	//		get { return _keep; }
	//	}

	//	public override IValue RollIValue
	//	{
	//		get { return _number; }
	//	}

	//	public CompositeRKPool(IValue n, IValue k)
	//	{
	//		_keep = k;
	//		_number = n;
	//	}
	//	public CompositeRKPool(AbsRKPool pool) : this(pool.RollIValue, pool.KeepIValue) { }
	//}


	//public class RKPool : AbsRKPool
	//{

	//	public Value KeepValue { get; protected set; }
	//	public Value RollValue { get; protected set; }

	//	public override IValue KeepIValue{ get { return KeepValue; } }
	//	public override IValue RollIValue{ get { return RollValue; } }
	//	public override int Keep {
	//		get { return base.Keep; }
	//		set { KeepValue.BaseValue = value; }
	//	}
	//	/// <summary>
	//	/// the natural numbre of dice to roll.
	//	/// Set the base value, but get the total value!!
	//	/// </summary>
	//	public override int Number
	//	{
	//		get { return base.Number; }
	//		set { RollValue.BaseValue = value; }
	//	}

	//	public RKPool() : this(0, 0) { }
	//	public RKPool(int roll, int keep)
	//	{
	//		KeepValue = new Value(roll);
	//		RollValue = new Value(keep);
	//	}
	//}
}
