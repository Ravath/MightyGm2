using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {
	public delegate bool JaugeEventHandler( IJauge sender );
	public delegate void ValueChangedEventHandler( IJauge sender, int oldValue, int newValue );
	public interface IJauge {
		int MaxValue { get; }
		int MinValue { get; }
		int CurrentValue { get; set; }
		void Increase( int sumValue );
		void Decrease( int soustractedValue );
		bool Full { get; }
		bool Empty { get; }
		event JaugeEventHandler OnIsMin;
		event JaugeEventHandler OnIsMax;
		event ValueChangedEventHandler CurrentValueChanged;
		event ValueChangedEventHandler MaxValueChanged;
		event ValueChangedEventHandler MinValueChanged;
	}
	/// <summary>
	/// Gere une valeur qui varie entre un minimum et un maximum.
	/// Si le maximum augmente, la valeur courante augmente d'autant.
	/// </summary>
	public class Jauge : IJauge {

		#region Members
		private Value _minValue = new Value();
		private Value _maxValue = new Value();
		/// <summary>
		/// valeur à soustraire au max pour obtenir la valeur courante (cad, si 0 => cur==max)
		/// </summary>
		private int _soustract;
		#endregion

		#region Events
		public event JaugeEventHandler OnIsMin;
		public event JaugeEventHandler OnIsMax;
		public event ValueChangedEventHandler CurrentValueChanged;
		public event ValueChangedEventHandler MaxValueChanged;
		public event ValueChangedEventHandler MinValueChanged;
		#endregion

		#region Init
		public Jauge( int min, int max ) {
			_minValue.BaseValue = min;
			_maxValue.BaseValue = max;
		}
		#endregion

		#region Properties
		public int MaxValue { get { return MaxTotalValue; }	}
		public int MinValue { get { return MinTotalValue; } }
		public int MaxBaseValue {
			get { return _maxValue.BaseValue; }
			set {
				if(_maxValue.BaseValue == value) { return; }
				int old = _maxValue.TotalValue;
				_maxValue.BaseValue = value;
				if(MaxValueChanged != null)
					MaxValueChanged(this, old, _maxValue.TotalValue);
				checkMinMax();
			}
		}
		public int MinBaseValue {
			get { return _minValue.BaseValue; }
			set {
				if(_minValue.BaseValue == value) { return; }
				int old = _minValue.TotalValue;
				_minValue.BaseValue = value;
				if(MinValueChanged != null)
					MinValueChanged(this, old, _minValue.TotalValue);
				checkMinMax();
			}
		}
		public int MaxTotalValue {
			get { return _maxValue.TotalValue; }
		}
		public int MinTotalValue {
			get { return _minValue.TotalValue; }
		}
		public int CurrentValue {
			get {
				return MaxTotalValue - _soustract;
			}
			set {
				if(value == CurrentValue) { return; }
				setCurrentValue(value);
            }
		}

		public bool Full { get { return CurrentValue>=MaxTotalValue; } }
		public bool Empty { get { return CurrentValue <= MinTotalValue; } }
		#endregion

		#region Functions
		/// <summary>
		/// Add a Modifier to the maximum value.
		/// </summary>
		/// <param name="mod"></param>
		public void AddModifier( IValue mod ) {
			_maxValue.AddModifier(mod);
			if(mod.TotalValue != 0 && MaxValueChanged != null)
				MaxValueChanged(this, MaxValue - mod.TotalValue, MaxValue);
			checkMinMax();
		}
		/// <summary>
		/// Remove a modifier from the maximum value.
		/// </summary>
		/// <param name="mod"></param>
		public void RemoveModifier( IValue mod ) {
			_maxValue.RemoveModifier(mod);
			if(mod.TotalValue != 0 && MaxValueChanged != null)
				MaxValueChanged(this, MaxValue + mod.TotalValue, MaxValue);
			checkMinMax();
		}
		/// <summary>
		/// Add a Modifier to the minimum value.
		/// </summary>
		/// <param name="mod"></param>
		public void AddMinModifier( IValue mod ) {
			_minValue.AddModifier(mod);
			if(mod.TotalValue != 0 && MinValueChanged != null)
				MinValueChanged(this, MinValue - mod.TotalValue, MinValue);
			checkMinMax();
		}
		/// <summary>
		/// Remove a modifier from the minimum value.
		/// </summary>
		/// <param name="mod"></param>
		public void RemoveMinModifier( IValue mod ) {
			_minValue.RemoveModifier(mod);
			if(mod.TotalValue != 0 && MinValueChanged != null)
				MinValueChanged(this, MinValue + mod.TotalValue, MinValue);
			checkMinMax();
		}
		private void setCurrentValue(int newval ) {
			int old = CurrentValue;

			if(newval == old) { return; }
			if(newval < MinValue)
				newval = MinValue;
			if(newval > MaxValue)
				newval = MaxValue;
			_soustract = MaxValue - newval;

            CurrentValueChanged?.Invoke(this, old, CurrentValue);
            checkMinMax();
		}

		/// <summary>
		/// decrease the current value.
		/// </summary>
		/// <param name="soustractedValue">Shall be positive.</param>
		public void Decrease( int soustractedValue ) {
			if(soustractedValue == 0) { return; }
			if(soustractedValue < 0)
				throw new ArgumentException("soustractedValue argument shall be positive.");
			setCurrentValue(CurrentValue - soustractedValue);
        }
		/// <summary>
		/// increase the current value.
		/// </summary>
		/// <param name="sumValue">Shall be positive.</param>
		public void Increase( int sumValue ) {
			if(sumValue == 0) { return; }
			if(sumValue < 0)
				throw new ArgumentException("sumValue argument shall be positive.");
			setCurrentValue(CurrentValue + sumValue);
		}
		#endregion
		public override string ToString() {
			return string.Format("{0}/{1}", CurrentValue, MaxTotalValue);
		}

		private void checkMinMax() {
			if(_soustract == 0 && OnIsMax!=null	) {
				OnIsMax(this);
            }
			if(_soustract == MaxTotalValue - MinTotalValue && OnIsMin != null) {
				OnIsMin(this);
			}
		}
	}
}
