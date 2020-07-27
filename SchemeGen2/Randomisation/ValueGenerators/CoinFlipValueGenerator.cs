using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.ValueGenerators
{
	/// <summary>
    /// An value generator that returns one of two pre-determined results with a set chance. 
    /// </summary>
	class CoinFlipValueGenerator : ValueGenerator
	{
		public CoinFlipValueGenerator(double headsChance, byte headsValue, byte tailsValue) :
            base()
        {
            _headsChance = headsChance;
            _headsValue = headsValue;
            _tailsValue = tailsValue;
        }

        protected override byte InternalGenerateByte(Random rng)
        {
            return rng.NextDouble() <= _headsChance ? _headsValue : _tailsValue;
        }

        public override bool DoesValueRangeOverlap(byte? min, byte? max)
		{
			return (!min.HasValue || _headsValue >= min.Value || _tailsValue >= max.Value)
                && (!max.HasValue || _headsValue <= max.Value || _tailsValue <= max.Value);
		}

        public override void GuaranteeValueRange(byte? min, byte? max)
        {
            if (max.HasValue)
            {
                if (_headsValue > max.Value && _tailsValue <= max.Value)
                {
                    _headsValue = _tailsValue;
                }
                else if (_tailsValue > max.Value && _headsValue <= max.Value)
                {
                    _tailsValue = _headsValue;
                }
                else if (_headsValue > max.Value && _tailsValue > max.Value)
                {
                    _tailsValue = _headsValue = max.Value;
                }
            }

            if (min.HasValue)
            {
                if (_headsValue < min.Value && _tailsValue >= min.Value)
                {
                    _headsValue = _tailsValue;
                }
                else if (_tailsValue < min.Value && _headsValue >= min.Value)
                {
                    _tailsValue = _headsValue;
                }
                else if (_headsValue < min.Value && _tailsValue < min.Value)
                {
                    _tailsValue = _headsValue = min.Value;
                }
            }
        }

        public override ValueGenerator DeepClone()
		{
			return new CoinFlipValueGenerator(_headsChance, _headsValue, _tailsValue);
		}

        double _headsChance;
        byte _headsValue;
        byte _tailsValue;
	}
}