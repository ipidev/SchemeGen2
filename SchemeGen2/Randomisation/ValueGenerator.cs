using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation
{
    /// <summary>
    /// The base class for a value generator. Objects deriving from this type
    /// can be used to generate values for use in the scheme.
    /// </summary>
    abstract class ValueGenerator
    {
        /// <summary>
        /// If set, a set seed used for generating reproducable results will be used.
        /// </summary>
        static bool UseDebugSeed = true;

        /// <summary>
        /// Constructs a new value generator.
        /// </summary>
        public ValueGenerator()
        {
            if (UseDebugSeed)
            {
                _rng = new Random(17);
            }
            else
            {
                _rng = new Random();
            }
        }

        /// <summary>
        /// When overridden in a deriving type, generates a byte for use in the scheme.
        /// </summary>
        /// <returns>A byte value.</returns>
        public abstract byte GenerateByte(); 

        //All value generators will use the same RNG so that a seed can be used
        //to give reproducible results.
        protected Random _rng;
    }
}
