using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {
	/// <summary>
	/// The behaviour of a WearableModule when the limit of equiped value is reached.
	/// </summary>
	public enum WearableModuleBehaviour {
		RemoveFirsts, RemoveBiggests, RemoveLasts, Overloads, DoesntWear
	}

	public class WearableModule<O,P> where O :IModifier<P> where P : IAgent {

		#region Members
		private ListeObjets<O,P> _inventory;
		private ListeWearable<O,P> _equiped;
		#endregion

		#region Properties
		public IEnumerable<O> Inventory { get { return _inventory.AllStock; } }
		public IEnumerable<O> Equiped { get { return _equiped.AllStock; } }
		/// <summary>
		/// The maximum value of cumuled equiped objects.
		/// if 0 or less, doesn't use the limit value.
		/// </summary>
		public int MaxEquipedObject { get; protected set; }
		/// <summary>
		/// Returns the Overload of equiped objects value.
		/// Negative value means there is still some room.
		/// </summary>
		public int Overload { get { return GetEquipementLoad() - MaxEquipedObject; } }
		public WearableModuleBehaviour ReachedLimitBehaviour { get; protected set; }
		#endregion

		#region Init
		public WearableModule(P perso) {
			_inventory = new ListeObjets<O, P>(perso);
			_equiped = new ListeWearable<O, P>(perso);
			ReachedLimitBehaviour = WearableModuleBehaviour.Overloads;
			MaxEquipedObject = 0;
        }
		#endregion

		/// <summary>
		/// Add the object to the inventory
		/// </summary>
		/// <param name="obj">The manipulated object.</param>
		public void GetObject( O obj ) {
			_inventory.Add(obj);
		}
		/// <summary>
		/// Remove the object from the inventory (and desequip if equiped)
		/// </summary>
		/// <param name="obj">The manipulated object.</param>
		public void ThrowObject( O obj ) {
			_equiped.Remove(obj);
			_inventory.Remove(obj);
		}
		/// <summary>
		/// Equip the object, and get it from the inventory if it's in it.
		/// </summary>
		/// <param name="obj">The manipulated object.</param>
		/// <return>true if weared succesfuly, false otherwise.</return>
		public bool Wear( O obj ) {
			_inventory.Remove(obj);
			if(TestEquipementLoad(obj)) {
				_equiped.Add(obj);
			} else {
				//test if the object can't be equiped even if we remove other objects.
				if(GetEquipedValue(obj) > MaxEquipedObject
					|| ReachedLimitBehaviour == WearableModuleBehaviour.DoesntWear)
					return false;
				//remove objects until there is enought room
				do {
					switch(ReachedLimitBehaviour) {
						case WearableModuleBehaviour.RemoveFirsts:
						_equiped.RemoveFirst();
						break;
						case WearableModuleBehaviour.RemoveLasts:
						_equiped.RemoveLast();
						break;
						case WearableModuleBehaviour.RemoveBiggests:
						int max = Equiped.Max(e => GetEquipedValue(e));
						_equiped.Remove(
							Equiped.First( e=> GetEquipedValue(e)==max )
						);
                        break;
					}
				} while(!TestEquipementLoad(obj));
				//room has been made : equip object
				_equiped.Add(obj);
			}
			return true;
		}
		/// <summary>
		/// Check if the object can be equiped, according to the behaviour and loadLimits.
		/// </summary>
		/// <param name="obj">The object to equip.</param>
		/// <returns>True if the object can be equiped.</returns>
		public bool TestEquipementLoad( O obj ) {
			return ReachedLimitBehaviour == WearableModuleBehaviour.Overloads
				|| MaxEquipedObject <= 0
				|| MaxEquipedObject >= GetEquipementLoad() + GetEquipedValue(obj);
        }
		/// <summary>
		/// Desequip the object and put it in the inventory.
		/// Does nothing if weren't equiped in the first place.
		/// </summary>
		/// <param name="obj">The manipulated object.</param>
		public void Unwear( O obj ) {
			if(_equiped.Remove(obj)) {
				_inventory.Add(obj);
			}
		}
		/// <summary>
		/// Gets the equiped value of an object.
		/// </summary>
		/// <param name="obj">The equiped object.</param>
		/// <returns>1</returns>
		public virtual int GetEquipedValue(O obj ) {
			return 1;
		}
		/// <summary>
		/// Compute the sum of the equiped value of all the equiped objects.
		/// </summary>
		/// <returns>Sum of equiped objects's equipement value.</returns>
		public int GetEquipementLoad() {
			return Equiped.Sum(e => GetEquipedValue(e));
		}
		/// <summary>
		/// Remove everything from equipement and inventory.
		/// </summary>
		public void Clear() {
			_equiped.RemoveAll();
			_inventory.RemoveAll();
		}
	}
}
