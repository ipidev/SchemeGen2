using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.ValueGenerators
{
    /// <summary>
    /// An value generator that only returns the value it was constructed with. 
    /// </summary>
    class ConstantValueGenerator : ValueGenerator
    {
        public ConstantValueGenerator(byte value) :
            base()
        {
            _value = value;
        }

        protected override byte InternalGenerateByte(Random rng)
        {
            return _value;
        }

		public override bool DoesValueRangeOverlap(byte? min, byte? max)
		{
			return (!min.HasValue || _value >= min.Value)
                && (!max.HasValue || _value <= max.Value);
		}

		public override void GuaranteeValueRange(byte? min, byte? max)
        {
            if (min.HasValue && _value < min.Value)
            {
                _value = min.Value;
            }
            else if (max.HasValue && _value > max.Value)
            {
                _value = max.Value;
            }
        }

		public override ValueGenerator DeepClone()
		{
			return new ConstantValueGenerator(_value);
		}

		byte _value;
    }
}
