using L5R.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L5R.Model.Attack;
using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;

namespace L5R.Model.Object {
	public class Inventaire : ListeObjets<L5R_Object, Agent.Agent> {

		public Inventaire( Agent.Agent agent ) : base(agent) {

		}
	}

	public class BaseArmorND : DerivedValue {
		private Attribut _ref;
		public BaseArmorND(Attribut reflexe) : base(reflexe) { _ref = reflexe; }

		public override int BaseValue {
			get {
				return NdFromReflexe(_ref.TotalValue);
			}
		}

		public static int NdFromReflexe(int reflexe) { return reflexe * 5 + 5; }
	}

	public class InventaireArmure : WearableModule<Armure, Agent.Agent> {
		#region Members
		private Value _naturalND;
		#endregion
		#region Properties
		public BaseArmorND ND { get; }
		public Value Reduction { get; }
		#endregion

		public InventaireArmure( Agent.Agent agent ) : base(agent) {
			this.MaxEquipedObject = 1;
			this.ReachedLimitBehaviour = WearableModuleBehaviour.RemoveFirsts;

			_naturalND = new Value(0) { Label = "Bonus Naturel" };
			Reduction = new Value(0) { Label = "Reduction" };
			ND = new BaseArmorND(agent.Attributs.Reflexes) { Label = "ND d'Armure" };
			ND.AddModifier(_naturalND);
        }

		public void SetPersonnageModel(PersonnageJoueurModel model)
		{
			//Apply Changes
			Clear();
			Reduction.BaseValue = 0;
			_naturalND.BaseValue = 0;

			// Add and wear armors
			/*TODO When PJ model works
			 * Armure armor = new Armure();
			armor.SetArmure(model.Armure);
			Wear(armor);*/
		}

		public void SetCreatureModel(FigurantModel model)
		{
			//Apply changes
			Clear();
			Reduction.BaseValue = model.Reduction;
			_naturalND.BaseValue = model.NDArmure - BaseArmorND.NdFromReflexe(model.Reflexes);
			//TODO : do not forget armor implication in prec line.
		}
	}

	public class InventaireArme : WearableModule<Arme, Agent.Agent>
	{
		private List<IAttaque> _attacks = new List<IAttaque>();
		public IEnumerable<IAttaque> WeaponAttacks { get { return _attacks; } }

		private List<IAttaque> _naturalAttacks = new List<IAttaque>();
		public IEnumerable<IAttaque> NaturalAttacks { get { return _naturalAttacks; } }

        public IEnumerable<IAttaque> Attacks
        {
            get { return WeaponAttacks.Concat(NaturalAttacks); }
        }
        
        public InventaireArme( Agent.Agent agent ) : base(agent) {
			this.MaxEquipedObject = 1;
			this.ReachedLimitBehaviour = WearableModuleBehaviour.DoesntWear;
		}

		#region Attacks collection
		public void AddAttack(IAttaque att)
		{
			_attacks.Add(att);
		}
		public void RemoveAttack(IAttaque att)
		{
			_attacks.Remove(att);
		}
		public void ClearAttacks()
		{
			_attacks.Clear();
		}
		#endregion

		#region Natural Attacks collection
		public void AddNaturalAttack(IAttaque att)
		{
			_naturalAttacks.Add(att);
		}
		public void RemoveNaturalAttack(IAttaque att)
		{
			_naturalAttacks.Remove(att);
		}
		public void ClearNaturalAttacks()
		{
			_naturalAttacks.Clear();
		} 
		#endregion

		#region Model Settings
		private void SetAgentModel(PersonnageModel model)
		{
			Clear();
			ClearNaturalAttacks();

			foreach (var item in model.Armes)
			{
                Arme weapon = (Arme)ModelFactory.Factory.InstantiateEquipment(item.Tag);
				//TODO why ArmeExemplar?
				GetObject(weapon);
			}
		}

		public void SetPersonnageModel(PersonnageJoueurModel model)
		{
			SetAgentModel(model);
		}

		public void SetCreatureModel(FigurantModel model)
		{
			SetAgentModel(model);

			//Creature specific : default attacks
			foreach (AttaqueFigurant att in model.Attaques)
			{
                AddNaturalAttack(new AttaqueCreature(att));
			}
		} 
		#endregion
	}
}
