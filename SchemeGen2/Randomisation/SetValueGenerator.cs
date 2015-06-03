using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation
{
    /// <summary>
    /// An value generator that only returns the value it was constructed with. 
    /// </summary>
    class SetValueGenerator : ValueGenerator
    {
        public SetValueGenerator(byte value) :
            base()
        {
            _value = value;
        }

        public override byte GenerateByte()
        {
            return _value;
        }

        byte _value;
    }
}
