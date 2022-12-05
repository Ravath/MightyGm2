using System;
using MightyGm2.RPG.L5R4.Data;
using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Model;
using System.Collections.Generic;
using Engine.Units;

namespace L5R.Model.Capacity {

    public class SpellUnitValue<V, U> : UnitValue<V, U>
        where V : struct
        where U : struct
    {
        // True if rank is a factor in the total result
        public bool UseRank { get; set; }

        public SpellUnitValue() : base() { }
        public SpellUnitValue(V val, bool useRank, U unit) : base(val, unit)
        {
            UseRank = useRank;
        }

        public override string ToString()
        {
            if(UseRank)
                return "Rank x " + Value + " " + Unit;
            if (Value.ToString() == "0")
                return Unit.ToString();
            return base.ToString();
        }
    }

	public class Sort : AbsRankedCapacity {

        SpellUnitValue<double, Portee> _range;
        SpellUnitValue<double, ZoneEffet> _zone;
        SpellUnitValue<double, Duree> _duration;
        protected TargetType _targetType;

		public bool Mahou { get; private set; }
		public bool Concentration { get; private set; }
		public ElementSort Element { get; protected set; }

		public SpellUnitValue<double, Portee> UnitRange { get => _range; }
        public SpellUnitValue<double, ZoneEffet> UnitZone { get => _zone; }
        public SpellUnitValue<double, Duree> UnitDuration { get => _duration; }


        public override double Range { get { return _range.Value; } }
		public override TargetType TargetType { get { return _targetType; } }

        private List<Augmentation> _augmentations = new List<Augmentation>();
        public IEnumerable<Augmentation> Augmentations { get => _augmentations; }

        private List<MotClefSort> _keys = new List<MotClefSort>();
        public IEnumerable<MotClefSort> Keys { get => _keys; }

        public Sort()
		{

		}

		public Sort(SortModel model) : this()
		{
			SetDataModel(model);
		}

		public void SetDataModel( SortModel model ) {
			Name = model.Name;
            Description = model.Description;
			Element = model.Element;
			Rank = model.Maitrise;
            Mahou = false;//TODO (model is MahouModel);
			Concentration = model.Concentration;
			_range = new SpellUnitValue<double, Portee>((double)model.FacteurPortee, model.PorteeXRang, model.Portee);
			_zone = new SpellUnitValue<double, ZoneEffet>((double)model.FacteurZone, model.ZoneXRang, model.ZoneEffet);
			_duration = new SpellUnitValue<double, Duree>((double)model.FacteurDuree, model.DureeXRang, model.Duree);

            // Keys
            _keys.Clear();
            if(model.MotClefs != null && model.MotClefs.Count>0)
                _keys.AddRange(model.MotClefs);

            // Augmentations
            _augmentations.Clear();
            foreach (var item in model.Augmentations)
            {
                _augmentations.Add(ModelFactory.Factory.InstantiateAugmentation(item));
            }

            // TODO : remove this bulshit
            Delegate = ModelFactory.Factory.InstantiateSpell(model.Tag);
            switch (model.Portee) {
				case Portee.Personnel:
				_targetType = TargetType.Self;
                break;
				case Portee.Contact:
				_targetType = TargetType.Agent;
				break;
				case Portee.PersonnelContact:
				_targetType = TargetType.Agent;
				break;
				case Portee.Metres:
				_targetType = TargetType.Place;
				break;
				case Portee.Kilometres:
				_targetType = TargetType.Place;
				break;
				case Portee.Special:
				_targetType = TargetType.Place;
				break;
				default:
				throw new NotImplementedException("SetSortModel not implemented for enum "+model);
			}
		}
	}
}
