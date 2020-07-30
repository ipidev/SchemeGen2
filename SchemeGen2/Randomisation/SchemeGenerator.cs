using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeGen2.Randomisation.Guarantees;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2.Randomisation
{
	/// <summary>
	/// A class that generates schemes once all of its value generators are
	/// initialised.
	/// </summary>
	class SchemeGenerator
	{
		public SchemeGenerator()
		{
			_settingGenerators = new ValueGenerator[SchemeTypes.NumberOfNonWeaponSettings];

			_weaponGenerators = new WeaponGenerator[SchemeTypes.NumberOfWeapons];
			for (int i = 0; i < SchemeTypes.NumberOfWeapons; ++i)
			{
				_weaponGenerators[i] = new WeaponGenerator();
			}

			_extendedOptionGenerators = new ValueGenerator[SchemeTypes.NumberOfExtendedOptions];
			ExtendedOptionsDataVersion = 0;

			_guarantees = new List<Guarantee>();
		}

		public Scheme GenerateScheme(Random rng, SchemeVersion version)
		{
			Scheme scheme = new Scheme(version, ExtendedOptionsDataVersion);

			//Generate values for every setting.
			int settingsCount = Math.Min(scheme.Settings.Length, _settingGenerators.Length);
			for (int i = 0; i < settingsCount; ++i)
			{
				ValueGenerator valueGenerator = _settingGenerators[i];

				if (valueGenerator != null)
				{
					SettingTypes settingType = (SettingTypes)i;
					Setting setting = scheme.Access(settingType);
					Debug.Assert(setting != null);

					setting.SetValue(valueGenerator.GenerateValue(rng), valueGenerator);
				}
			}

			//Generate values for every weapon.
			int weaponsCount = Math.Min(scheme.Weapons.Length, _weaponGenerators.Length);
			for (int i = 0; i < weaponsCount; ++i)
			{
				WeaponGenerator weaponGenerator = _weaponGenerators[i];

				for (int j = 0; j < (int)WeaponSettings.Count; ++j)
				{
					WeaponSettings weaponSetting = (WeaponSettings)j;
					ValueGenerator valueGenerator = weaponGenerator.Get(weaponSetting);

					if (valueGenerator != null)
					{
						WeaponTypes weaponType = (WeaponTypes)i;
						Weapon weapon = scheme.Access(weaponType);
						Debug.Assert(weapon != null);

						Setting setting = weapon.Access(weaponSetting);
						Debug.Assert(setting != null);

						//Check value generator range (range check is not done at XML parsing-time for default values).
						if (!valueGenerator.IsValueRangeWithinLimits(setting.Limits))
						{
							throw new Exception(String.Format("Generatable values for setting '{0}' must be within the range(s): {1}.",
								setting.Name, setting.Limits.ToString()));
						}

						setting.SetValue(valueGenerator.GenerateValue(rng), valueGenerator);
					}
				}
			}

			//Generate values for every extended option.
			if (version >= SchemeVersion.Armageddon3)
			{
				int optionsCount = Math.Min(scheme.ExtendedOptions.Length, _extendedOptionGenerators.Length);
				for (int i = 0; i < optionsCount; ++i)
				{
					ValueGenerator valueGenerator = _extendedOptionGenerators[i];
					if (valueGenerator != null)
					{
						ExtendedOptionTypes extendedOption = (ExtendedOptionTypes)i;
						Setting setting = scheme.Access(extendedOption);
						Debug.Assert(setting != null);

						setting.SetValue(valueGenerator.GenerateValue(rng), valueGenerator);
					}
				}
			}

			//Handle guarantees.
			foreach (Guarantee guarantee in _guarantees)
			{
				guarantee.ApplyGuarantee(scheme, rng);
			}

			return scheme;
		}

		///////////////////////////////////////////////////////////////////////
		// Accessors / Mutators

		/// <summary>
		/// Sets the given setting's value generator.
		/// </summary>
		public void Set(SettingTypes setting, ValueGenerator valueGenerator)
		{
			Debug.Assert(setting < SettingTypes.Count);

			_settingGenerators[(int)setting] = valueGenerator;
		}

		/// <summary>
		/// Sets the given weapon settings's value generator.
		/// </summary>
		public void Set(WeaponTypes weapon, WeaponSettings weaponSetting, ValueGenerator valueGenerator)
		{
			Debug.Assert(weaponSetting < WeaponSettings.Count);
			if (weaponSetting < WeaponSettings.Count)
			{
				Debug.Assert(weapon < WeaponTypes.Count);
				if (weapon < WeaponTypes.Count)
				{
					_weaponGenerators[(int)weapon].Set(weaponSetting, valueGenerator);
				}
			}
		}

		/// <summary>
		/// Sets a copy of the given value generator for all applicable weapons.
		/// </summary>
		public void SetDefault(WeaponSettings weaponSetting, ValueGenerator valueGenerator)
		{
			for (int i = 0; i < (int)WeaponTypes.Count; ++i)
			{
				if (!SchemeTypes.CanApplyWeaponSetting((WeaponTypes)i, weaponSetting))
					continue;

				_weaponGenerators[i].Set(weaponSetting, valueGenerator.DeepClone());
			}
		}

		/// <summary>
		/// Sets the given extended option's value generator.
		/// </summary>
		public void Set(ExtendedOptionTypes extendedOption, ValueGenerator valueGenerator)
		{
			Debug.Assert(extendedOption < ExtendedOptionTypes.Count);

			_extendedOptionGenerators[(int)extendedOption] = valueGenerator;
		}

		/// <summary>
		/// Adds the given guarantee.
		/// </summary>
		public void AddGuarantee(Guarantee guarantee)
		{
			Debug.Assert(guarantee != null);
			Debug.Assert(!_guarantees.Contains(guarantee));

			_guarantees.Add(guarantee);
		}

		ValueGenerator[] _settingGenerators;
		WeaponGenerator[] _weaponGenerators;

		ValueGenerator[] _extendedOptionGenerators;
		public int ExtendedOptionsDataVersion { get; set; }

		List<Guarantee> _guarantees;
	}
}
