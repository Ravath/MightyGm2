using MightyGm2.RPG.L5R4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R.Model.Skill {
	/// <summary>
	/// Une specialisation de compétence.
	/// </summary>
	public class Specialisation {
		/// <summary>
		/// Le nom de la spécailisation.
		/// </summary>
		public string Name { get; set; }
		public bool Degradante { get; set; }
		public bool Noble { get; set; }
		public TraitCompetence? Trait { get; set; }
		public string Tag { get; private set; }

		public Specialisation(SpecialisationModel model)
		{
			SetSpecialisation(model);
		}
		public Specialisation(){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		public void SetSpecialisation( SpecialisationModel model ) {
			Name = model.Name;
			Degradante = model.Degradante;
			Noble = false;
			Trait = null;
			Tag = model.Tag;

		}
	}
}
