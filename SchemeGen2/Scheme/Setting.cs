using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2
{
	/// <summary>
	/// A single byte value in a scheme.
	/// </summary>
	class Setting
	{
		//TODO: Probably don't need the range to be here.

		public Setting()
			: this(0x00, 0xFF)
		{
		}

		public Setting(byte minimumValue, byte maximumValue)
		{
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
		}

		/// <summary>
		/// The current value of this setting.
		/// </summary>
		public byte Value { get; private set; }

		/// <summary>
		/// The value generator responsible for this setting's value.
		/// </summary>
		public ValueGenerator ValueGenerator { get; private set; }

		/// <summary>
		/// The minimum valid value of this setting, inclusive.
		/// </summary>
		public byte MinimumValue { get; private set; }

		/// <summary>
		/// The maximum valid value of this setting, inclusive.
		/// </summary>
		public byte MaximumValue { get; private set; }

		/// <summary>
		/// Sets the range of valid values for this setting. Does not readjust the current value to suit.
		/// </summary>
		/// <param name="min">The minimum valid value, inclusive.</param>
		/// <param name="max">The maximum valid value, inclusive.</param>
		public void SetRange(byte min, byte max)
		{
			Debug.Assert(max >= min);
			MinimumValue = min;
			MaximumValue = max;
		}

		/// <summary>
		/// Sets the range of valid values to represent boolean (0x00 or 0x01).
		/// </summary>
		public void SetRangeToBoolean()
		{
			SetRange(0x00, 0x01);
		}

		/// <summary>
		/// Sets the current value of this setting.
		/// </summary>
		/// <param name="value">The new value.</param>
		/// <param name="valueGenerator">The value generator that created this value, if applicable.</param>
		public void SetValue(byte value, ValueGenerator valueGenerator = null)
		{
			IsInRangeWithThrow(value);
			Value = value;
			ValueGenerator = valueGenerator;
		}

		/// <summary>
		/// Checks if the given value is within the valid range for this setting.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>Returns true if the value is in range, false otherwise.</returns>
		public bool IsInRange(byte value)
		{
			return (value >= MinimumValue) && (value <= MaximumValue);
		}

		/// <summary>
		/// Checks if the current value for this setting is within its valid range.
		/// </summary>
		/// <returns>Returns true if the value is in range, false otherwise.</returns>
		public bool IsInRange()
		{
			return IsInRange(Value);
		}

		/// <summary>
		/// Throws an exception if the given value is out of range.
		/// </summary>
		protected void IsInRangeWithThrow(byte value)
		{
			if (!IsInRange(value))
			{
				throw new ArgumentOutOfRangeException("value",
					"The valid range for this setting is [" + MinimumValue.ToString() + " - " + MaximumValue.ToString() + "].");
			}
		}

		public override string ToString()
		{
			return String.Format("Value: {0}, Range: [{1} - {2}]", Value, MinimumValue, MaximumValue);
		}
	}
}
