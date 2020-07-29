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
        public ConstantValueGenerator(int value) :
            base()
        {
            _value = value;
        }

        protected override int InternalGenerateValue(Random rng)
        {
            return _value;
        }

        public override bool IsValueRangeWithinLimits(SettingLimits settingLimits)
		{
			return settingLimits.Minimum <= _value && _value <= settingLimits.Maximum;
		}

		public override bool DoesValueRangeOverlap(int? min, int? max)
		{
			return (!min.HasValue || _value >= min.Value)
                && (!max.HasValue || _value <= max.Value);
		}

		public override void GuaranteeValueRange(int? min, int? max)
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

		int _value;
    }
}
