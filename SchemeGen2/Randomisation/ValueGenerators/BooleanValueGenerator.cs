using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.ValueGenerators
{
	/// <summary>
    /// An value generator that returns true or false with a set chance. 
    /// </summary>
	class BooleanValueGenerator : CoinFlipValueGenerator
	{
		public BooleanValueGenerator(double chance) :
            base(chance, SchemeTypes.True, SchemeTypes.False)
        {
        }
	}
}