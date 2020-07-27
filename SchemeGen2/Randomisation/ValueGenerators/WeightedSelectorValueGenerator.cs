using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.ValueGenerators
{
    struct WeightedSelection
    {
        public WeightedSelection(ValueGenerator valueGenerator, double weight)
        {
            ValueGenerator = valueGenerator;
            Weight = weight;
        }

        public ValueGenerator ValueGenerator { get; set; }
        public double Weight { get; set; }
    }

	class WeightedSelectorValueGenerator : ValueGenerator
	{
		public WeightedSelectorValueGenerator()
		{
		}

        public void AddSelection(ValueGenerator valueGenerator, double weight)
        {
            _selections.Add(new WeightedSelection(valueGenerator, weight));
            _totalWeight += weight;
        }

		protected override byte InternalGenerateByte(Random rng)
        {
            double targetWeight = rng.NextDouble() * _totalWeight;
            double accumulatedWeight = 0.0;

            foreach (WeightedSelection selection in _selections)
            {
                accumulatedWeight += selection.Weight;
                if (accumulatedWeight >= targetWeight)
                {
                    if (selection.ValueGenerator != null)
                        return selection.ValueGenerator.GenerateByte(rng);
                    else
                        return 0;
                }
            }

            return 0;
        }

        public override bool DoesValueRangeOverlap(byte? min, byte? max)
		{
            foreach (WeightedSelection selection in _selections)
            {
                if (selection.ValueGenerator != null && selection.ValueGenerator.DoesValueRangeOverlap(min, max))
                    return true;
            }

            return false;
		}

        public override void GuaranteeValueRange(byte? min, byte? max)
        {
            //Reduce our selections down to ones that overlap the given range.
            List<WeightedSelection> selectionsWithinRange = new List<WeightedSelection>();
            double totalWeightWithinRange = 0.0;

            foreach (WeightedSelection selection in _selections)
            {
                if (selection.ValueGenerator != null && selection.ValueGenerator.DoesValueRangeOverlap(min, max))
                {
                    selectionsWithinRange.Add(selection);
                    totalWeightWithinRange += selection.Weight;
                }
            }

            if (selectionsWithinRange.Count > 0)
            {
                _selections = selectionsWithinRange;
                _totalWeight = totalWeightWithinRange;
            }

            foreach (WeightedSelection selection in _selections)
            {
                selection.ValueGenerator.GuaranteeValueRange(min, max);
            }
        }

		public override ValueGenerator DeepClone()
		{
			WeightedSelectorValueGenerator clone = new WeightedSelectorValueGenerator();

            foreach (WeightedSelection selection in _selections)
            {
                clone.AddSelection(selection.ValueGenerator.DeepClone(), selection.Weight);
            }

            return clone;
		}

		List<WeightedSelection> _selections = new List<WeightedSelection>();
        double _totalWeight = 0.0;
	}
}
