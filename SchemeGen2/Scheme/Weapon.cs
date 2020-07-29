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
		public Weapon(WeaponTypes weaponType)
		{
			WeaponType = weaponType;
			Ammo = new Setting(weaponType.ToString() + ".Ammo", SchemeLimits.GetWeaponSettingLimits(weaponType, WeaponSettings.Ammo));
			Power = new Setting(weaponType.ToString() + ".Power", SchemeLimits.GetWeaponSettingLimits(weaponType, WeaponSettings.Power));
			Delay = new Setting(weaponType.ToString() + ".Delay", SchemeLimits.GetWeaponSettingLimits(weaponType, WeaponSettings.Delay));
			Crate = new Setting(weaponType.ToString() + ".Crate", SchemeLimits.GetWeaponSettingLimits(weaponType, WeaponSettings.Crate));
		}

		public WeaponTypes WeaponType { get; private set; }

		public Setting Ammo { get; private set; }

		public Setting Power { get; private set; }

		public Setting Delay { get; private set; }

		public Setting Crate { get; private set; }

		/// <summary>
		/// Returns whether or not the weapon has 1 or more ammo set.
		/// </summary>
		/// <returns>Returns true if the weapon has 1 or more ammo set.</returns>
		public bool HasStartingAmmo()
		{
			return Ammo.Value > 0x00;
		}

		/// <summary>
		/// Returns whether or not the weapon has infinite ammo set.
		/// </summary>
		/// <returns>Returns true if the weapon has infinite ammo set.</returns>
		public bool HasInfiniteAmmo()
		{
			return (Ammo.Value == 0x0A) || (Ammo.Value >= 0x80);
		}

		/// <summary>
		/// Returns whether or not the weapon can be chosen on the first turn.
		/// </summary>
		/// <returns>Returns true if the weapon has ammo and has zero delay.</returns>
		public bool CanBeChosenOnFirstTurn()
		{
			return Delay.Value == 0x00 && HasStartingAmmo();
		}

		/// <summary>
		/// Returns whether or not the weapon has infinite delay set.
		/// </summary>
		/// <returns>Returns true if the weapon has infinite delay set.</returns>
		public bool HasInfiniteDelay()
		{
			return Delay.Value >= 0x80;
		}

		/// <summary>
		/// Returns whether or not this weapon can appear in crates, ignoring the other scheme settings and crate showers.
		/// </summary>
		/// <returns>Returns true if the weapon can appear in crates.</returns>
		public bool CanAppearInCrates()
		{
			return Crate.Value > 0x00 && !HasInfiniteAmmo() && !HasInfiniteDelay();
		}

		/// <summary>
		/// Returns whether or not this weapon will ever appear given its current settings.
		/// </summary>
		/// <returns>Retruns true if the weapon has ammo or can appear in crates.</returns>
		public bool CanAppearAtAll()
		{
			return HasStartingAmmo() || CanAppearInCrates();
		}

		/// <summary>
		/// Gets the given weapon setting by reference.
		/// </summary>
		public Setting Access(WeaponSettings weaponSetting)
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
				throw new ArgumentException("Invalid enum " + weaponSetting.ToString());
			}
		}

		public void Serialise(System.IO.Stream stream)
		{
			Ammo.Serialise(stream);
			Power.Serialise(stream);
			Delay.Serialise(stream);
			Crate.Serialise(stream);
		}

		public override string ToString()
		{
			return String.Format("{0} - Ammo: {1}, Power: {2}, Delay: {3}, Crate: {4}", WeaponType.ToString(), Ammo.Value, Power.Value, Delay.Value, Crate.Value);
		}
	}
}
