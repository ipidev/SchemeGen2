using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2.Randomisation
{
	/// <summary>
    /// Stores a set of value generators for each setting applicable to weapons.
    /// </summary>
    class WeaponGenerator
    {
        public WeaponGenerator()
        {
        }

        public ValueGenerator Ammo { get; private set; }
        public ValueGenerator Power { get; private set; }
        public ValueGenerator Delay { get; private set; }
        public ValueGenerator Crate { get; private set; }

        /// <summary>
        /// Gets the given setting's value generator. May return null.
        /// </summary>
        public ValueGenerator Get(WeaponSettings weaponSetting)
        {
            switch (weaponSetting)
            {
            case WeaponSettings.Ammo:
                return Ammo;

            case WeaponSettings.Power:
                return Power;

            case WeaponSettings.Delay:
                return Delay;

            case WeaponSettings.Crate:
                return Crate;

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
                Ammo = valueGenerator;
                break;

            case WeaponSettings.Power:
                Power = valueGenerator;
                break;

            case WeaponSettings.Delay:
                Delay = valueGenerator;
                break;

            case WeaponSettings.Crate:
                Crate = valueGenerator;
                break;

            default:
                Debug.Assert(false, "Invalid enum.");
                break;
            }
        }
    }
}
