using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
    /// <summary>
    /// A single byte value in a scheme.
    /// </summary>
    class Setting
    {
        //TODO: Multiple ranges.

        public Setting()
        {
            _value = 0x00;
            _min = 0x00;
            _max = 0xFF;
        }

        public Setting(byte min, byte max)
        {
            _value = 0x00;
            SetRange(min, max);
        }

        /// <summary>
        /// The value of this setting. Throws an exception if the value is out of range.
        /// </summary>
        public byte Value
        {
            get { return _value; }
            set { IsInRangeWithThrow(value); _value = value; }
        }

        /// <summary>
        /// The minimum valid value of this setting, inclusive.
        /// </summary>
        public byte MinimumValue
        {
            get { return _min; }
        }

        /// <summary>
        /// The maximum valid value of this setting, inclusive.
        /// </summary>
        public byte MaximumValue
        {
            get { return _max; }
        }

        /// <summary>
        /// Sets the range of valid values for this setting. Does not readjust the current value to suit.
        /// </summary>
        /// <param name="min">The minimum valid value, inclusive.</param>
        /// <param name="max">The maximum valid value, inclusive.</param>
        public void SetRange(byte min, byte max)
        {
            Debug.Assert(max >= min);
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Sets the range of valid values to represent boolean (0x00 or 0x01).
        /// </summary>
        public void SetRangeToBoolean()
        {
            SetRange(0x00, 0x01);
        }

        /// <summary>
        /// Checks if the given value is within the valid range for this setting.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>Returns true if the value is in range, false otherwise.</returns>
        public bool IsInRange(byte value)
        {
            return (value >= _min) && (value <= _max);
        }

        /// <summary>
        /// Checks if the current value for this setting is within its valid range.
        /// </summary>
        /// <returns>Returns true if the value is in range, false otherwise.</returns>
        public bool IsInRange()
        {
            return IsInRange(_value);
        }

        /// <summary>
        /// Throws an exception if the given value is out of range.
        /// </summary>
        protected void IsInRangeWithThrow(byte value)
        {
            if (!IsInRange(value))
            {
                throw new ArgumentOutOfRangeException("value",
                    "The valid range for this setting is [" + _min.ToString() + " - " + _max.ToString() + "].");
            }
        }

        byte _value;
        byte _min, _max;
    }
}
