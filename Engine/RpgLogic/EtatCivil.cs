using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {

	public class EtatCivil {
		public string Name { get; set; }
		public bool Male { get; set; }
		public int Taille { get; set; }
		public int Age { get; set; }
		public int Poids { get; set; }
		public string Cheveux { get; set; }
		public string Carrure { get; set; }
		public string Yeux { get; set; }
		public string Description { get; set; }
	}

	public class Famille<P> where P : PNJMineur {

		#region Members
		private List<P> _soeurs = new List<P>();
		private List<P> _freres = new List<P>();
		private List<P> _fils = new List<P>();
		private List<P> _filles = new List<P>();
		#endregion

		#region Properties
		public P Pere { get; set; }
		public P Mere { get; set; }
		public P Conjoint { get; set; }
		public IEnumerable<P> Soeurs { get; }
		public IEnumerable<P> Freres { get; }
		public IEnumerable<P> Fils { get; }
		public IEnumerable<P> Filles { get; } 
		#endregion

		public void AddSoeur( P soeur ) {
			_soeurs.Add(soeur);
		}
		public void AddFrerer( P frere ) {
			_freres.Add(frere);
		}
		public void AddFils( P fils ) {
			_fils.Add(fils);
		}
		public void AddFille( P fille ) {
			_filles.Add(fille);
		}

		public void RemoveSoeur( P soeur ) {
			_soeurs.Remove(soeur);
		}
		public void RemoveFrerer( P frere ) {
			_freres.Remove(frere);
		}
		public void RemoveFils( P fils ) {
			_fils.Remove(fils);
		}
		public void RemoveFille( P fille ) {
			_filles.Remove(fille);
		}
	}

	public class PNJMineur {
		public string Name { get; set; }
		public int Age { get; set; }
	}
}
