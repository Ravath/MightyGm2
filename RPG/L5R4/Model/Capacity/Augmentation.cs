using MightyGm2.RPG.L5R4.Data;
using L5R4.JdrCore;
using System;

namespace L5R.Model.Capacity
{
    /// <summary>
    /// Augmentation Default class
    /// </summary>
	public class Augmentation
	{
        private readonly string _descModel;
        private object[] _args;

        public string Description
        {
            get
            {
                if (_args.Length== 0) return _descModel;
                return String.Format(_descModel, _args);
            }
        }
		public int Cout { get; set; }

		public Augmentation(AugmentationSortModel model)
		{
			Cout = 1;
            _descModel = model.Description;
		}

        public void SetExemplar(AugmentationSortExemplar ae)
        {
            _args = GetComplement(new FiveRingsComplementParser(ae.Complement));
        }

		public virtual object[] GetComplement(FiveRingsComplementParser cp)
		{
			return cp.Values;
		}
	}

    // ========== Augmentation specific implementations ========== //

    public class PorteeAugmentation : Augmentation
	{
		public double Bonus { get; set; }
		public PorteeAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			Bonus = cp.GetDouble(0, 1.5);
			Cout = cp.GetInt(1, 1);
			return new object[] { Bonus, Cout };
		}
	}

	public class DureeAugmentation : Augmentation
	{
		public int Duree { get; set; }
		public DureeAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			Duree = cp.GetInt(0, 1);
			Cout = cp.GetInt(1, 1);
			return new object[] { Duree, Cout };
		}
	}

	public class JourAugmentation : DureeAugmentation
	{
		public JourAugmentation(AugmentationSortModel model) : base(model) { }
	}

	public class ZoneAugmentation : Augmentation
	{
		public double Zone { get; set; }
		public ZoneAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			Zone = cp.GetDouble(0, 1.5);
			Cout = cp.GetInt(1, 1);
			return new object[] { Zone, Cout };
		}
	}

	public class CibleAugmentation : Augmentation
	{
		public int Cible { get; set; }
		public CibleAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			Cible = cp.GetInt(0, 1);
			Cout = cp.GetInt(1, 1);
			return new object[] { Cible, Cout };
		}
	}

	public class DommageAugmentation : Augmentation
	{
		public RollAndKeep Dommages{ get; set; }
		public DommageAugmentation(AugmentationSortModel model) : base(model){}

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			Dommages = cp.GetPool(0);
			Cout = cp.GetInt(1, 1);
			return new object[] { Dommages, Cout };
		}
	}

	public class NDAugmentation : Augmentation
	{
		public int ND { get; set; }
		public NDAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			ND = cp.GetInt(0, 5);
			return new object[] { ND };
		}
	}

	public abstract class AugmentationCostOnly : Augmentation
	{
		public AugmentationCostOnly(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			Cout = cp.GetInt(0, 1);
			return new object[] { Cout };
		}
	}

	public class CiblerAutruiAugmentation : AugmentationCostOnly
	{
		public CiblerAutruiAugmentation(AugmentationSortModel model) : base(model) { }
	}
	
	public class CiblerAutruiSupplementaireAugmentation : AugmentationCostOnly
	{
		public CiblerAutruiSupplementaireAugmentation(AugmentationSortModel model) : base(model) { }
	}

	public class AnneauAugmentation : Augmentation
	{
		public int BonusAnneau { get; set; }
		public Anneau Anneau { get; set; }

		public AnneauAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			BonusAnneau = cp.GetInt(0, 1);
			Anneau = cp.GetEnum<Anneau>(1);
			Cout = cp.GetInt(2, 1);
			return new object[] { BonusAnneau, Anneau, Cout };
		}
	}

	public class OppositionAugmentation : Augmentation
	{
		public RollAndKeep OppositionBonus { get; set; }
		public OppositionAugmentation(AugmentationSortModel model) : base(model) { }

		public override object[] GetComplement(FiveRingsComplementParser cp)
		{
			OppositionBonus = cp.GetPool(0);
			Cout = cp.GetInt(1, 1);
			return new object[] { OppositionBonus, Cout };
		}
	}
}
