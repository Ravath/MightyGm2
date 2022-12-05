using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Engine.RpgLogic {

	public delegate void ValueChangedHandler( IValue ival, int newValue, int oldValue );

	public interface IValue {
		int BaseValue { get; }
		int TotalValue { get; }
		IEnumerable<IValue> Modifiers { get; }
		void AddModifier( IValue mod );
		void RemoveModifier( IValue mod );
		string Label { get; set; }
		event ValueChangedHandler ValueChanged;
	}

	public abstract class DerivedValue : IValue {
		private HashSet<IValue> _values = new HashSet<IValue>();
		private List<IValue> _modifiers = new List<IValue>();

		public DerivedValue( params IValue[] vals ) {
			AddDerivedValue(vals);
        }
		/// <summary>
		/// Add the instance to the valueChanged Event of the values it is calculated from.
		/// </summary>
		/// <param name="vals">Values used for computation of the value.</param>
		protected void AddDerivedValue( params IValue[] vals ) {
			foreach(IValue v in vals) {
				v.ValueChanged += DerivedValBaseChanged;
				_values.Add(v);
			}
		}
		/// <summary>
		/// Add the instance from the valueChanged Event of the values because it don't use them any more for computation.
		/// </summary>
		/// <param name="vals">Values no more used used for computation of the value.</param>
		protected void RemoveDerivedValue( params IValue[] vals ) {
			foreach(IValue v in vals) {
				v.ValueChanged -= DerivedValBaseChanged;
				_values.Remove(v);
			}
		}

		private void DerivedValBaseChanged( IValue ival, int newValue, int oldValue ) {
			if(ValueChanged != null) {
				int tot = TotalValue;
                int old = tot - newValue + oldValue;
				ValueChanged(this, tot, old);
			}
        }

		public virtual string Label { get; set; }
		public IEnumerable<IValue> Modifiers { get { return _modifiers; } }
		
		public int TotalValue { get { return _modifiers.Sum(p => p.TotalValue) + BaseValue; } }
		public abstract int BaseValue { get; }
		
		public event ValueChangedHandler ValueChanged;

		public void AddModifier( IValue mod ) {
			int tot = TotalValue;
			int modTot = mod.TotalValue;
			_modifiers.Add(mod);
			mod.ValueChanged += DerivedValBaseChanged;
			if(modTot != 0 && ValueChanged != null) {
				ValueChanged(this, tot, tot+ modTot);
            }
        }

		public void RemoveModifier( IValue mod ) {
			int tot = TotalValue;
			int modTot = mod.TotalValue;
            _modifiers.Remove(mod);
			mod.ValueChanged -= DerivedValBaseChanged;
            if(modTot != 0 && ValueChanged != null) {
				ValueChanged(this, tot, tot- modTot);
			}
		}
	}
	/// <summary>
	/// The base value is the Total value from the source, but the source can be changed.
	/// </summary>
	public class IndirectValue : DerivedValue {
		private IValue _iv;
		public IndirectValue(IValue val) : base(val){
			_iv = val;
        }
		/// <summary>
		/// Base indirect value.
		/// </summary>
		public override int BaseValue {
			get { return _iv.TotalValue; }
		}
		/// <summary>
		/// Change the source
		/// </summary>
		/// <param name="val"></param>
		public void SetValue( IValue val ) {
			if(val == _iv) { return; }
			if(_iv != null)
				RemoveDerivedValue(_iv);
			if(val != null)
				AddDerivedValue(val);
			_iv = val;
		}
    }

    public class FactorValue : DerivedValue
    {
        private IValue _val;
        public double Factor { get; set; }
        public override int BaseValue
        {
            get
            {
                return (int)(_val.TotalValue * Factor);
            }
        }

        public FactorValue(IValue value, double factor) : base(value)
        {
            _val = value;
            Factor = factor;
        }
    }

    public class Value : IValue {

		#region Members
		private int _base;
		private List<IValue> _modifiers = new List<IValue>();
		#endregion

		#region Properties
		public virtual string Label { get; set; }


		public virtual int BaseValue {
			get { return _base; }
			set {
				if(value == BaseValue) { return; }
				int old = BaseValue;
				_base = value;
				if(ValueChanged != null) {
					int tot = TotalValue;
					ValueChanged(this, tot, tot + old - value);
				}
			}
		}

		public virtual int TotalValue {
			get {
				return _modifiers.Sum(p => p.TotalValue) + BaseValue;
			}
		}
		public IEnumerable<IValue> Modifiers {
			get { return _modifiers; }
		}
		#endregion

		#region Init
		public Value() {

		}
		public Value( int val ) {
			BaseValue = val;
		}
		public Value( IValue val ) {
			BaseValue = val.BaseValue;
			foreach(var item in val.Modifiers) {
				AddModifier(item);
			}
		}
		#endregion

		#region Events
		public event ValueChangedHandler ValueChanged;
		#endregion

		#region Functions
		public virtual void AddModifier( IValue mod ) {
			_modifiers.Add(mod);
			mod.ValueChanged += ModifierChanged;
			int modTot = mod.TotalValue;
			if(ValueChanged != null && modTot != 0) {
				int tot = TotalValue;
				ValueChanged(this, tot, tot - modTot);
			}
		}
		public virtual void RemoveModifier( IValue mod ) {
			_modifiers.Remove(mod);
			mod.ValueChanged -= ModifierChanged;
			int modTot = mod.TotalValue;
			if(ValueChanged != null && modTot!=0) {
				int tot = TotalValue;
				ValueChanged(this, tot, tot + modTot);
			}
		} 
		#endregion

		private void ModifierChanged(IValue mod, int newVal, int oldVal ) {
			if(ValueChanged != null) {
				int tot = TotalValue;
				ValueChanged(this, tot, tot + oldVal - newVal);
			}
		}

		public override string ToString() {
			return string.Format("{0}({1})", BaseValue,TotalValue);
		}
	}

}
