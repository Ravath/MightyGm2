using System;
using L5R.Model.Object;
using L5R.Model.Trait;
using Engine.RpgLogic;
using L5R4.JdrCore;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;

namespace L5R.Model.Agent {
	public class Figurant : Agent {
		private Value _initRoll = new Value(0) { Label = "Roll Initiative" };
		private Value _initKeep = new Value(0) { Label = "Keep Initiative" };

		public Figurant()
		{
			Initiative = new RollAndKeep(_initRoll, _initKeep);
		}
		public Figurant(FigurantModel model)
		{
			SetPersonnage(model);
		}

		public override void SetPersonnage(FigurantModel model) {
			/* set initiative */
			_initRoll.BaseValue = model.Initiative.Roll;
			_initKeep.BaseValue = model.Initiative.Keep;
			/* vie */
			if(model.VieHumaine)
				Vie = new SeuilViePJ(this);
			else
				Vie = new SeuilVieCreature(this, model);
			/* souillure */
			Honneur.SetRank(0, 0);
			Status.SetRank(0, 0);
			Gloire.SetRank(0, 0);
			Souillure.SetRank(decimal.ToInt32(model.Souillure), (int)(10 * model.Souillure) % 10);

			//attaques
			Armes.SetCreatureModel(model);
			//défense
			Armures.SetCreatureModel(model);
			//pouvoirs de créature
			TraitsCreature.Clear();
			foreach(var item in model.Pouvoirs) {
				L5R_Trait tc = ModelFactory.Factory.InstantiateNaturalPower(item);
				TraitsCreature.AddTrait(tc);
			}
			/* clear all unused componants */
			//objets
			Inventaire.RemoveAll();
			//traits
			Techniques.Clear();
			PouvoirsOutremonde.Clear();
			Avantages.Clear();
			//capacités
			Katas.ClearCapacities();
			Kihos.ClearCapacities();
			Sorts.ClearCapacities();

			// base settings + event
			base.SetPersonnage(model);
		}

	}
}
