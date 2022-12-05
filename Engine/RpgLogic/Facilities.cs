using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic
{
	public static class RpgMath
	{
		public static int RangeThreshold(int value, int min, int max)
		{
			return Math.Min(Math.Max(min, value), max);
		}
		public static int MinThreshold(int value, int min)
		{
			return (value < min) ? min : value;
		}
		public static int MaxThreshold(int value, int max)
		{
			return (value > max) ? max : value;
		}
	}
}
