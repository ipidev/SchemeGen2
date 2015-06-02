using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
    /// <summary>
    /// A collection of 4 settings that represents a weapon in the scheme.
    /// </summary>
    class Weapon
    {
        public Weapon(bool isSuperWeapon)
        {
            _ammo = new Setting();
            _power = new Setting();
            _delay = new Setting();
            _crate = new Setting();
            _isSuperWeapon = isSuperWeapon;
        }

        public Weapon() : this(false)
        {
        }

        public Setting Ammo
        {
            get { return _ammo; }
        }

        public Setting Power
        {
            get { return _power; }
        }

        public Setting Delay
        {
            get { return _delay; }
        }

        public Setting Crate
        {
            get { return _crate; }
        }

        public bool IsSuperWeapon
        {
            get { return _isSuperWeapon; }
            set { _isSuperWeapon = value; }
        }

        /// <summary>
        /// Returns whether or not the weapon has 1 or more ammo set.
        /// </summary>
        /// <returns>Returns true if the weapon has 1 or more ammo set.</returns>
        public bool HasStartingAmmo()
        {
            return _ammo.Value > 0x00;
        }

        /// <summary>
        /// Returns whether or not the weapon has infinite ammo set.
        /// </summary>
        /// <returns>Returns true if the weapon has infinite ammo set.</returns>
        public bool HasInfiniteAmmo()
        {
            return (_ammo.Value == 0x0A) || (_ammo.Value >= 0x80);
        }

        /// <summary>
        /// Returns whether or not the weapon can be chosen on the first turn.
        /// </summary>
        /// <returns>Returns true if the weapon has ammo and has zero delay.</returns>
        public bool CanBeChosenOnFirstTurn()
        {
            return _delay.Value == 0x00 && HasStartingAmmo();
        }

        /// <summary>
        /// Returns whether or not the weapon has infinite delay set.
        /// </summary>
        /// <returns>Returns true if the weapon has infinite delay set.</returns>
        public bool HasInfiniteDelay()
        {
            return _delay.Value >= 0x80;
        }

        /// <summary>
        /// Returns whether or not this weapon can appear in crates, ignoring the other scheme settings and crate showers.
        /// </summary>
        /// <returns>Returns true if the weapon can appear in crates.</returns>
        public bool CanAppearInCrates()
        {
            //TODO: Consider the super weapons on/off setting and global crate chance.
            return (_crate.Value > 0x00 || _isSuperWeapon) && !HasInfiniteAmmo() && !HasInfiniteDelay();
        }

        /// <summary>
        /// Returns whether or not this weapon will ever appear given its current settings.
        /// </summary>
        /// <returns>Retruns true if the weapon has ammo or can appear in crates.</returns>
        public bool CanAppearAtAll()
        {
            return HasStartingAmmo() || CanAppearInCrates();
        }

        public Setting Get(WeaponSettings weaponSetting)
        {
            switch (weaponSetting)
            {
            case WeaponSettings.Ammo:
                return _ammo;

            case WeaponSettings.Power:
                return _power;

            case WeaponSettings.Delay:
                return _delay;

            case WeaponSettings.Crate:
                return _crate;

            default:
                throw new ArgumentException("Invalid enum " + weaponSetting.ToString());
            }
        }

        public Setting this[WeaponSettings weaponSetting]
        {
            get { return Get(weaponSetting); }
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[(int)WeaponSettings.Count];

            bytes[(int)WeaponSettings.Ammo] = _ammo.Value;
            bytes[(int)WeaponSettings.Power] = _power.Value;
            bytes[(int)WeaponSettings.Delay] = _delay.Value;
            bytes[(int)WeaponSettings.Crate] = _crate.Value;

            return bytes;
        }

        Setting _ammo;
        Setting _power;
        Setting _delay;
        Setting _crate;

        bool _isSuperWeapon;
    }
}
