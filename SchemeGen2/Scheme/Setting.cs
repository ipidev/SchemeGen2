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
		public Setting(string name, SettingLimits limits, SettingSize size = SettingSize.Byte)
		{
			Name = name;
			Limits = limits;
			Size = size;
		}

		/// <summary>
		/// The current value of this setting.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The current value of this setting.
		/// </summary>
		public int Value { get; private set; }

		/// <summary>
		/// The minimum and maximum value that this setting can take.
		/// </summary>
		public SettingLimits Limits { get; private set; }

		/// <summary>
		/// The number of bytes that this setting occupies in the output file.
		/// </summary>
		public SettingSize Size { get; private set; }

		/// <summary>
		/// The value generator responsible for this setting's value.
		/// </summary>
		public ValueGenerator ValueGenerator { get; private set; }

		/// <summary>
		/// Sets the current value of this setting.
		/// </summary>
		/// <param name="value">The new value.</param>
		/// <param name="valueGenerator">The value generator that created this value, if applicable.</param>
		public void SetValue(int value, ValueGenerator valueGenerator = null)
		{
			if (Limits != null && !Limits.IsInRange(value))
			{
				throw new ArgumentOutOfRangeException("value", value, "Value for setting '" + Name + "' is out of range.");
			}

			Value = value;
			ValueGenerator = valueGenerator;
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

			stream.Write(bytes, 0, bytes.Length);
		}

		public override string ToString()
		{
			return String.Format("{0} - Value: {1}", Name, Value);
		}
	}
}
