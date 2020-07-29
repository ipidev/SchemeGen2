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

			_guarantees = new List<Guarantee>();
		}

		public Scheme GenerateScheme(Random rng)
		{
			Scheme scheme = new Scheme(false);

			//Generate values for every setting.
			for (int i = 0; i < _settingGenerators.Length; ++i)
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
			for (int i = 0; i < _weaponGenerators.Length; ++i)
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
		List<Guarantee> _guarantees;
	}
}
