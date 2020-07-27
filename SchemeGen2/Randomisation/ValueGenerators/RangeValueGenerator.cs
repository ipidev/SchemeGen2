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
        public RangeValueGenerator(byte min, byte max, int step = 1) :
            base()
        {
            _min = min;
            _max = max;
            Step = step;
        }

        public int Step { get; set; }

        protected override byte InternalGenerateByte(Random rng)
        {
            if (Step > 1)
            {
                int numberOfSteps = ((int)_max - (int)_min) / Step;
                return (byte)(_min + (Step * (byte)rng.Next(0, numberOfSteps)));
            }
            else
            {
                return (byte)rng.Next((int)_min, (int)_max);
            }
        }

        public override bool DoesValueRangeOverlap(byte? min, byte? max)
		{
            if (!min.HasValue && !max.HasValue)
                return true;

            if ((!min.HasValue || _max >= min.Value) &&
                (!max.HasValue || _min <= max.Value))
                return true;

			return _min <= min.Value && _max >= max.Value;
		}

        public override void GuaranteeValueRange(byte? min, byte? max)
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

        byte _min;
        byte _max;
	}
}
