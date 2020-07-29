using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
	class SettingLimits
	{
		public SettingLimits(int minimum, int maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public int Minimum { get; set; }
		public int Maximum { get; set; }

		public bool IsInRange(int value) { return Minimum <= value && value <= Maximum; }
	}

	class WeaponLimits
	{
		public WeaponLimits()
		{
		}

		public SettingLimits Ammo { get; set; }
		public SettingLimits Power { get; set; }
		public SettingLimits Delay { get; set; }
		public SettingLimits Crate { get; set; }
	}

	static class SchemeLimits
	{
		static SchemeLimits()
		{
			for (int i = 0; i < (int)SettingTypes.Count; ++i)
			{
				_settingLimits[i] = InitialiseSettingLimit((SettingTypes)i);
			}

			for (int i = 0; i < (int)WeaponTypes.Count; ++i)
			{
				_weaponLimits[i] = new WeaponLimits
				{
					Ammo = InitialiseWeaponAmmoLimit((WeaponTypes)i),
					Power = InitialiseWeaponPowerLimit((WeaponTypes)i),
					Delay = InitialiseWeaponDelayLimit((WeaponTypes)i),
					Crate = InitialiseWeaponCrateLimit((WeaponTypes)i)
				};
			}
		}

		static SettingLimits InitialiseSettingLimit(SettingTypes settingType)
		{
			switch (settingType)
			{
			case SettingTypes.Version:
				return new SettingLimits(1, 3);

			//Boolean properties.
			case SettingTypes.DisplayTotalRoundTime:
			case SettingTypes.AutomaticReplays:
			case SettingTypes.ArtilleryMode:
			case SettingTypes.DonorCards:
			case SettingTypes.DudMines:
			case SettingTypes.InitialWormPlacement:
			case SettingTypes.Blood:
			case SettingTypes.AquaSheep:
			case SettingTypes.SheepHeaven:
			case SettingTypes.GodWorms:
			case SettingTypes.IndestructibleTerrain:
			case SettingTypes.UpgradedGrenade:
			case SettingTypes.UpgradedShotgun:
			case SettingTypes.UpgradedCluster:
			case SettingTypes.UpgradedLongbow:
			case SettingTypes.TeamWeapons:
			case SettingTypes.SuperWeapons:
				return new SettingLimits(0, 1);

			//Enum-based properties.
			case SettingTypes.StockpilingMode:
				return new SettingLimits(0, (int)StockpilingModes.Count);

			case SettingTypes.WormSelect:
				return new SettingLimits(0, (int)WormSelectModes.Count);
			
			//All quartary properties.
			case SettingTypes.SuddenDeathEvent:
				return new SettingLimits(0, (int)SuddenDeathEvents.Count);

			//Everything else has full byte range.
			default:
				return new SettingLimits(0, 255);
			}
		}

		static SettingLimits InitialiseWeaponAmmoLimit(WeaponTypes weaponType)
		{
			if (SchemeTypes.CanApplyWeaponSetting(weaponType, WeaponSettings.Ammo))
			{
				return new SettingLimits(0, 255);
			}
			else
			{
				return new SettingLimits(0, 0);
			}
		}

		static SettingLimits InitialiseWeaponPowerLimit(WeaponTypes weaponType)
		{
			if (!SchemeTypes.CanApplyWeaponSetting(weaponType, WeaponSettings.Power))
			{
				return new SettingLimits(0, 0);
			}

			switch (weaponType)
			{
			case WeaponTypes.JetPack:
			case WeaponTypes.Girder:
				return new SettingLimits(0, 255);

			case WeaponTypes.Mortar:
			case WeaponTypes.ClusterBomb:
			case WeaponTypes.BananaBomb:
			case WeaponTypes.AirStrike:
			case WeaponTypes.NapalmStrike:
				return new SettingLimits(0, 14);

			case WeaponTypes.Skunk:
			case WeaponTypes.Longbow:
				return new SettingLimits(0, 9);

			case WeaponTypes.BattleAxe:
			case WeaponTypes.NinjaRope:
				return new SettingLimits(0, 4);

			default:
				return new SettingLimits(0, 19);
			}
		}

		static SettingLimits InitialiseWeaponDelayLimit(WeaponTypes weaponType)
		{
			if (SchemeTypes.CanApplyWeaponSetting(weaponType, WeaponSettings.Delay))
			{
				return new SettingLimits(0, 255);
			}
			else
			{
				return new SettingLimits(0, 0);
			}
		}

		static SettingLimits InitialiseWeaponCrateLimit(WeaponTypes weaponType)
		{
			if (SchemeTypes.CanApplyWeaponSetting(weaponType, WeaponSettings.Crate))
			{
				return new SettingLimits(0, 255);
			}
			else
			{
				return new SettingLimits(0, 0);
			}
		}

		public static SettingLimits GetSettingLimits(SettingTypes settingType)
		{
			Debug.Assert(settingType < SettingTypes.Count);
			return settingType < SettingTypes.Count ? _settingLimits[(int)settingType] : null;
		}

		public static SettingLimits GetWeaponSettingLimits(WeaponTypes weaponType, WeaponSettings weaponSetting)
		{
			if (weaponType == WeaponTypes.Default)
			{
				return new SettingLimits(0, 255);
			}
			else
			{
				Debug.Assert(weaponType < WeaponTypes.Count);
				if (weaponType < WeaponTypes.Count)
				{
					WeaponLimits weaponLimits = _weaponLimits[(int)weaponType];

					switch (weaponSetting)
					{
					case WeaponSettings.Ammo:
						return weaponLimits.Ammo;

					case WeaponSettings.Power:
						return weaponLimits.Power;

					case WeaponSettings.Delay:
						return weaponLimits.Delay;

					case WeaponSettings.Crate:
						return weaponLimits.Crate;
					}
				}
			}

			return null;
		}

		static SettingLimits[] _settingLimits = new SettingLimits[(int)SettingTypes.Count];
		static WeaponLimits[] _weaponLimits = new WeaponLimits[(int)WeaponTypes.Count];
	}
}
