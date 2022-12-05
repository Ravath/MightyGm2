using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {
	/// <summary>
	/// The type of target that can be affected.
	/// Place is a place on the map.
	/// Void is : no target.
	/// Self : can only affect the caster.
	/// </summary>
	public enum TargetType {
		Agent, Object, Place, Void, Self
	}

	public interface ITargeting {
		/// <summary>
		/// The type of target that can be affected.
		/// </summary>
		TargetType TargetType { get;}
		/// <summary>
		/// Range in meters
		/// </summary>
		double Range { get; }
		/// <summary>
		/// If true, can affect multiple targets.
		/// </summary>
		bool CanAffectMultipleTargets { get; }
	}

	public interface ITargetable {
		/// <summary>
		/// The type of the target. Should not be void or self.
		/// </summary>
		TargetType TargetType { get; }

	}

}
