using Engine.RpgLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R.Model.Attribute {
	/// <summary>
	/// Used for Honnor, Glory, Status and Taint.
	/// </summary>
	public class RankedCarac : DerivedValue {

		Value _points;

        public Value Points { get { return _points; } }

        public RankedCarac(Value points) : base(points){
			_points = points;
        }
		/// <summary>
		/// Le rang de la caractéristique.
		/// </summary>
		public override int BaseValue {
			get {
				return _points.TotalValue / 10;
			}
        }
        /// <summary>
        /// Le reste de la caractéristique.
        /// </summary>
        public int RemainingPoints
        {
            get
            {
                return _points.TotalValue % 10;
            }
        }

        public void AddRank( int mod ) {
			_points.BaseValue += mod * 10;
        }
		public void AddPoints( int mod ) {
			_points.BaseValue += mod;
		}
		public void SetRank(int rank, int points ) {
			_points.BaseValue = rank * 10 + points;
		}
		public void SetRank( RankedCarac rank ) {
			_points.BaseValue = rank._points.BaseValue;
		}
	}
}
