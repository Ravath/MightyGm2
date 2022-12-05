using Engine.RpgLogic;
using L5R.Model.Agent;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;
using System;

namespace L5R.Model.Trait
{
	//"Tag": "SPE0001" : Enorme
	//"Tag": "SPE0002" : Esprit
	//"Tag": "SPE0003" : Invulnérabilité
	//"Tag": "SPE0004" : Invulnérabilité Partielle
	//"Tag": "SPE0005" : Invulnérabilité Supérieure
	//"Tag": "SPE0006" : Mort Vivant
	//"Tag": "SPE0007" : Peur
	//"Tag": "SPE0008" : Résistance Magique
	//"Tag": "SPE0009" : Vivacité
	public class Vivacity : DefaultAgentEffect
	{
		public Value Bonus { get; set; } = new Value() { Label = "Vivacité" };

		public override void Affect(Agent.Agent a)
		{
			a.Movement.MovementFactor.AddModifier(Bonus);
		}

		public override void Unaffect(Agent.Agent a)
        {
            a.Movement.MovementFactor.RemoveModifier(Bonus);
		}

		public override void SetComplement(string complement)
		{
			string[] args = complement.Split(';');
            if (args.Length < 1 || args.Length > 2)
            {
			    ModelFactory.ReportError_ArgumentNumber(args.Length, 1, 2, complement);
            }
            if (args.Length >= 1 && int.TryParse(args[0], out int result))
            {
                Bonus.BaseValue = result;
            }
            //TODO use optional word for effect condition
            //CHARGE, FLY
		}
	}
	//"Tag": "SPE0010" : Charge Furieuse
	//"Tag": "SPE0011" : Odorat
	//"Tag": "SPE0012" : Charge
	//"Tag": "SPE0013" : Attaque aux Yeux
	//"Tag": "SPE0014" : Drainage Sanguin(Gaki)
	//"Tag": "SPE0015" : Immortalité(Gaki)
	//"Tag": "SPE0016" : Invisibilité(Gaki)
	//"Tag": "SPE0017" : Métamorphose(Gaki)
	//"Tag": "SPE0018" : Possession(Gaki)
	//"Tag": "SPE0019" : Invulnérabilité(Gaki)
	//"Tag": "SPE0020" : Immunité à la Souillure
	//"Tag": "SPE0021" : Nom
	//"Tag": "SPE0022" : Duperie
	//"Tag": "SPE0023" : Eau Vitale
	//"Tag": "SPE0024" : Immunité aux sorts
	//"Tag": "SPE0025" : Immunité aux flèches
	//"Tag": "SPE0026" : Décapitation
	//"Tag": "SPE0027" : Aquatique
	//"Tag": "SPE0028" : Maladie
	//"Tag": "SPE0029" : Modification d'apparence
	//"Tag": "SPE0030" : Lame Tsuno
	//"Tag": "SPE0031" : Attaques Spectrales
}
