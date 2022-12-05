using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MightyGm2.RPG.L5R4.Data;
using L5R.Model.Object;
using MightyGm2.Engine.Process;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Model;

namespace L5R4.Control
{
	/// <summary>
	/// Options to choose
	///		- traveler pack
	/// </summary>
	public class BackpackStep : IProcessStep
	{
		#region Members
		private PersonnageProcess _pross;
		private List<L5R_Object> _assignedObjects = new List<L5R_Object>();
		#endregion

		#region Properties
		public IEnumerable<RpgDataElement> Items { get; private set; }
		public IEnumerable<ObjetModel> SelectedItems { get; set; }
		#endregion

		public bool CanProgress(out string ErrorMessageTag)
		{
			int diff = (SelectedItems?.Count() ?? 0) - 10;
			if (diff != 0)
			{
				ErrorMessageTag = String.Format("You must select {0} items {1}", Math.Abs(diff), diff > 0 ? "less" : "more");
			}
			else ErrorMessageTag = "OK";
			return diff == 0;
		}

		public void Init(IProcess process)
		{
			_pross = (PersonnageProcess)process;

            Items = _pross.Data.GetElements((int)L5R4_DataCategory.Object).
                OrderBy(o => o.Name);
            //TODO
    //        Items = _pross.Data.GetTable<ObjetModel>().
				//Where(o => o.NecessaireVoyage == true).
				//OrderBy(o => o.Name);
		}

		public void Reset() { /* Nothing */ }

		public void PreprossNext()
		{
			_assignedObjects.Clear();
			foreach (var obj in SelectedItems)
			{
				L5R_Object o = ModelFactory.Factory.InstantiateEquipment(obj.Tag);
				_pross.Personnage.Inventaire.Add(o);
				_assignedObjects.Add(o);
			}
		}

		public void PreprossReset()
		{
			_pross.Personnage.Inventaire.Remove(_assignedObjects);
		}

		public string GetStepMessage()
		{
			return "Choose 10 bagpack items";
		}
	}
}
