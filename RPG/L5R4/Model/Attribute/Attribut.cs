using Engine.RpgLogic;
using L5R4.JdrCore;
using System;

namespace L5R.Model.Attribute {
	public class Attribut : Value {

		/// <summary>
		/// used for Attribute tests.
		/// </summary>
		public RollAndKeep Pool { get; private set; }

		public Attribut() : base(2){
			Pool = new RollAndKeep(this, this);
		}
	}

	/// <summary>
	/// Un anneau élémentaire de L5R, sauf le vide.
	/// </summary>
	public class Anneau : DerivedValue {
		private Attribut _a1;
		private Attribut _a2;

		/// <summary>
		/// used for Ring tests.
		/// </summary>
		public RollAndKeep Pool { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a1">attribut physique dont dépend l'anneau.</param>
		/// <param name="a2">attribut mental dont dépend l'anneau.</param>
		public Anneau( Attribut a1, Attribut a2 ) : base(a1, a2) {
			_a1 = a1;
			_a2 = a2;
			Pool = new RollAndKeep(this, this);
		}

		public override int BaseValue {
			get {
				return Math.Min(_a1.BaseValue, _a2.BaseValue);
			}
		}
	}
}
