using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.ValueGenerators
{
	/// <summary>
	/// An value generator that returns a random value within a set range. 
	/// </summary>
	class RangeValueGenerator : ValueGenerator
	{
		public RangeValueGenerator(int min, int max, int step = 1) :
			base()
		{
			_min = min;
			_max = max;
			Step = step;
		}

		public int Step { get; set; }

		protected override int InternalGenerateValue(Random rng)
		{
			if (Step > 1)
			{
				int numberOfSteps = (_max - _min) / Step;
				return _min + (Step * rng.Next(0, numberOfSteps));
			}
			else
			{
				return rng.Next(_min, _max);
			}
		}

		public override bool IsValueRangeWithinLimits(SettingLimits settingLimits)
		{
			return settingLimits.IsInRange(_min, _max);
		}

		public override bool DoesValueRangeOverlap(int? min, int? max)
		{
			if (!min.HasValue && !max.HasValue)
				return true;

			if (min.HasValue && _max >= min.Value)
				return true;

			if (max.HasValue && _min <= max.Value)
				return true;

			return false;
		}

		public override void GuaranteeValueRange(int? min, int? max)
		{
			if (min.HasValue && _min < min.Value)
			{
				_min = min.Value;
			}
			
			if (max.HasValue && _max > max.Value)
			{
				_max = max.Value;
			}
		}

		public override ValueGenerator DeepClone()
		{
			return new RangeValueGenerator(_min, _max, Step);
		}

		int _min;
		int _max;
	}
}
