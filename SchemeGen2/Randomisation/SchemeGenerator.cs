using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation
{
    /// <summary>
    /// Stores a set of value generators for each setting applicable to weapons.
    /// </summary>
    struct WeaponGenerator
    {
        public ValueGenerator ammo;
        public ValueGenerator power;
        public ValueGenerator delay;
        public ValueGenerator crate;

        /// <summary>
        /// Gets the given setting's value generator. May return null.
        /// </summary>
        public ValueGenerator Get(WeaponSettings weaponSetting)
        {
            switch (weaponSetting)
            {
            case WeaponSettings.Ammo:
                return ammo;

            case WeaponSettings.Power:
                return power;

            case WeaponSettings.Delay:
                return delay;

            case WeaponSettings.Crate:
                return crate;

            default:
                Debug.Assert(false, "Invalid enum.");
                return null;
            }
        }

        /// <summary>
        /// Sets the given setting's value generator.
        /// </summary>
        public void Set(WeaponSettings weaponSetting, ValueGenerator valueGenerator)
        {
            switch (weaponSetting)
            {
            case WeaponSettings.Ammo:
                ammo = valueGenerator;
                break;

            case WeaponSettings.Power:
                power = valueGenerator;
                break;

            case WeaponSettings.Delay:
                delay = valueGenerator;
                break;

            case WeaponSettings.Crate:
                crate = valueGenerator;
                break;

            default:
                Debug.Assert(false, "Invalid enum.");
                break;
            }
        }

        /// <summary>
        /// Shorthand accessor/mutator for a value generator. May return null.
        /// </summary>
        public ValueGenerator this[WeaponSettings weaponSetting]
        {
            get { return Get(weaponSetting); }
            set { Set(weaponSetting, value); }
        }
    }

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
        }

        public Scheme GenerateScheme()
        {
            Scheme scheme = new Scheme(true);

            //Generate values for every setting.
            for (int i = 0; i < _settingGenerators.Length; ++i)
            {
                ValueGenerator valueGenerator = _settingGenerators[i];

                if (valueGenerator != null)
                {
                    SettingTypes settingType = (SettingTypes)i;
                    Setting setting = scheme[settingType];
                    Debug.Assert(setting != null);

                    setting.Value = valueGenerator.GenerateByte();
                }
            }

            //Generate values for every weapon.
            for (int i = 0; i < _settingGenerators.Length; ++i)
            {
                WeaponGenerator weaponGenerator = _weaponGenerators[i];

                for (int j = 0; j < (int)WeaponSettings.Count; ++j)
                {
                    WeaponSettings weaponSetting = (WeaponSettings)j;
                    ValueGenerator valueGenerator = weaponGenerator[weaponSetting];

                    if (valueGenerator != null)
                    {
                        WeaponTypes weaponType = (WeaponTypes)i;
                        Weapon weapon = scheme[weaponType];
                        Debug.Assert(weapon != null);

                        Setting setting = weapon[weaponSetting];
                        Debug.Assert(setting != null);
                        setting.Value = valueGenerator.GenerateByte();
                    }
                }
            }

            return scheme;
        }

        ///////////////////////////////////////////////////////////////////////
        // Accessors / Mutators

        /// <summary>
        /// Gets the given setting's value generator. May return null.
        /// </summary>
        public ValueGenerator Get(SettingTypes setting)
        {
            Debug.Assert(setting < SettingTypes.Count);

            return _settingGenerators[(int)setting];
        }

        /// <summary>
        /// Gets the given weapon's value generator collection.
        /// </summary>
        public WeaponGenerator Get(WeaponTypes weapon)
        {
            Debug.Assert(weapon < WeaponTypes.Count);

            return _weaponGenerators[(int)weapon];
        }

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
            Debug.Assert(weapon < WeaponTypes.Count);
            Debug.Assert(weaponSetting < WeaponSettings.Count);

            _weaponGenerators[(int)weapon].Set(weaponSetting, valueGenerator);
        }

        /// <summary>
        /// Shorthand accessor/mutator for a value generator. May return null.
        /// </summary>
        public ValueGenerator this[SettingTypes setting]
        {
            get { return Get(setting); }
            set { Set(setting, value); }
        }

        /// <summary>
        /// Shorthand accessor for a weapon generator. 
        /// </summary>
        public WeaponGenerator this[WeaponTypes weapon]
        {
            get { return Get(weapon); }
        }

        ValueGenerator[] _settingGenerators;
        WeaponGenerator[] _weaponGenerators;
    }
}
