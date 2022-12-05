using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {

	public interface ITimedEffect {
		RoundMoment UpdateMoment { get; set; }
		int Duration { get; set; }
		bool Permanent { get; }
	}

	public enum RoundMoment {
		BeginningTurn, EndTurn, BeginningRound, EndRound
	}

	/// <summary>
	/// Un état d'alteration
	/// </summary>
	public abstract class AlterationState : IModifier<IAgent>, ITimedEffect {
		/// <summary>
		/// Number of round the alteration affects the target.
		/// </summary>
		public int Duration { get; set; }
		/// <summary>
		/// The name of the alteration.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// True if the effect will last forever.
		/// </summary>
		public bool Permanent {
			get { return Duration < 0; }
		}
		/// <summary>
		/// The moment when the duration will be updated.
		/// </summary>
		public RoundMoment UpdateMoment { get; set; }

		public abstract void AffectAgent( IAgent a );

		public abstract void UnaffectAgent( IAgent a );
	}
	public abstract class TickingEffect : ITimedEffect {
		/// <summary>
		/// The moment when the alteration will affect the target.
		/// </summary>
		public RoundMoment TickingMoment { get; set; }
		/// <summary>
		/// Number of tick affecting the target before it stops.
		/// permanent if negative;
		/// </summary>
		public int Duration { get; set; }
		/// <summary>
		/// True if the effect will last forever.
		/// </summary>
		public bool Permanent {
			get { return Duration < 0; }
		}
		/// <summary>
		/// Time between 2 effects.
		/// </summary>
		public int Period { get; set; }
		/// <summary>
		/// The moment when the duration will be updated.
		/// </summary>
		public RoundMoment UpdateMoment { get; set; }

		public abstract void Tick( IAgent agent );
	}
}
