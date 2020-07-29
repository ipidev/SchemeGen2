using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2
{
	enum SettingSize
	{
		Byte,
		TwoBytes,
		FourBytes
	}

	/// <summary>
	/// A single byte value in a scheme.
	/// </summary>
	class Setting
	{
		//TODO: Probably don't need the range to be here.

		public Setting()
			: this(0, 255, SettingSize.Byte)
		{
		}

		public Setting(int minimumValue, int maximumValue, SettingSize size)
		{
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;
			Size = size;
		}

		/// <summary>
		/// The current value of this setting.
		/// </summary>
		public int Value { get; private set; }

		/// <summary>
		/// The value generator responsible for this setting's value.
		/// </summary>
		public ValueGenerator ValueGenerator { get; private set; }

		/// <summary>
		/// The minimum valid value of this setting, inclusive.
		/// </summary>
		public int MinimumValue { get; private set; }

		/// <summary>
		/// The maximum valid value of this setting, inclusive.
		/// </summary>
		public int MaximumValue { get; private set; }

		/// <summary>
		/// The number of bytes that this setting occupies in the output file.
		/// </summary>
		public SettingSize Size { get; private set; }

		/// <summary>
		/// Sets the range of valid byte values for this setting. Does not readjust the current value to suit.
		/// </summary>
		/// <param name="min">The minimum valid value, inclusive.</param>
		/// <param name="max">The maximum valid value, inclusive.</param>
		public void SetRange(byte min, byte max)
		{
			Debug.Assert(max >= min);
			MinimumValue = min;
			MaximumValue = max;
			Size = SettingSize.Byte;
		}

		/// <summary>
		/// Sets the range of valid values for this setting. Does not readjust the current value to suit.
		/// </summary>
		/// <param name="min">The minimum valid value, inclusive.</param>
		/// <param name="max">The maximum valid value, inclusive.</param>
		/// <param name="size">The number of bytes this setting occupies in the output file.</param>
		public void SetRange(int min, int max, SettingSize size)
		{
			Debug.Assert(max >= min);
			MinimumValue = min;
			MaximumValue = max;
			Size = size;
		}

		/// <summary>
		/// Sets the range of valid values to represent boolean (0x00 or 0x01).
		/// </summary>
		public void SetRangeToBoolean()
		{
			SetRange(0, 1);
		}

		/// <summary>
		/// Sets the current value of this setting.
		/// </summary>
		/// <param name="value">The new value.</param>
		/// <param name="valueGenerator">The value generator that created this value, if applicable.</param>
		public void SetValue(int value, ValueGenerator valueGenerator = null)
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
		public bool IsInRange(int value)
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
		protected void IsInRangeWithThrow(int value)
		{
			if (!IsInRange(value))
			{
				throw new ArgumentOutOfRangeException("value",
					"The valid range for this setting is [" + MinimumValue.ToString() + " - " + MaximumValue.ToString() + "].");
			}
		}

		public void Serialise(System.IO.Stream stream)
		{
			if (Size == SettingSize.Byte)
			{
				stream.WriteByte((byte)Value);
				return;
			}

			byte[] bytes = null;
			switch (Size)
			{
			case SettingSize.TwoBytes:
				bytes = BitConverter.GetBytes((short)Value);
				break;

			case SettingSize.FourBytes:
				bytes = BitConverter.GetBytes(Value);
				break;
			}

			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}

			stream.Write(bytes, (int)stream.Position, bytes.Length);
		}

		public override string ToString()
		{
			return String.Format("Value: {0}, Range: [{1} - {2}]", Value, MinimumValue, MaximumValue);
		}
	}
}
