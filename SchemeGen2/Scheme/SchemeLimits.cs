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
		public SettingLimits()
		{
			Ranges = new List<Tuple<int, int>>();
		}

		public SettingLimits(int minimum, int maximum)
			: this()
		{
			Ranges.Add(new Tuple<int, int>(minimum, maximum));
		}

		public SettingLimits(int min1, int max1, int min2, int max2)
			: this()
		{
			Ranges.Add(new Tuple<int, int>(min1, max1));
			Ranges.Add(new Tuple<int, int>(min2, max2));
		}

		public List<Tuple<int, int>> Ranges { get; private set; }

		public bool IsInRange(int value)
		{
			foreach (Tuple<int, int> range in Ranges)
			{
				if (range.Item1 <= value && value <= range.Item2)
					return true;
			}

			return false;
		}

		public bool IsInRange(int min, int max)
		{
			foreach (Tuple<int, int> range in Ranges)
			{
				if (range.Item1 <= min && max <= range.Item2)
					return true;
			}

			return false;
		}

		public override string ToString()
		{
			string output = "";

			for (int i = 0; i < Ranges.Count; ++i)
			{
				if (i < Ranges.Count - 1)
				{
					output += String.Format("[{0} - {1}], ", Ranges[i].Item1, Ranges[i].Item2);
				}
				else
				{
					output += String.Format("[{0} - {1}]", Ranges[i].Item1, Ranges[i].Item2);
				}
			}

			return output;
		}
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

			for (int i = 0; i < (int)ExtendedOptionTypes.Count; ++i)
			{
				_extendedOptionLimits[i] = InitialiseExtendedOptionLimit((ExtendedOptionTypes)i);
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
				return new SettingLimits(0, (int)StockpilingModes.Count - 1);

			case SettingTypes.WormSelect:
				return new SettingLimits(0, (int)WormSelectModes.Count - 1);
			
			//All quartary properties.
			case SettingTypes.SuddenDeathEvent:
				return new SettingLimits(0, (int)SuddenDeathEvents.Count - 1);

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
				return new SettingLimits(0, 4, 241, 255);

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

		static SettingLimits InitialiseExtendedOptionLimit(ExtendedOptionTypes extendedOption)
		{
			switch (extendedOption)
			{
			case ExtendedOptionTypes.DataVersion:
				return new SettingLimits(0, 0);

			//Boolean properties.
			case ExtendedOptionTypes.ConstantWind:
			case ExtendedOptionTypes.UnrestrictRope:
			case ExtendedOptionTypes.AutoPlaceWormsByAlly:
			case ExtendedOptionTypes.SuddenDeathDisablesWormSelect:
			case ExtendedOptionTypes.CircularAim:
			case ExtendedOptionTypes.AntiLockAim:
			case ExtendedOptionTypes.AntiLockPower:
			case ExtendedOptionTypes.WormSelectionDoesntEndHotSeat:
			case ExtendedOptionTypes.WormSelectionNeverCancelled:
			case ExtendedOptionTypes.BattyRope:
			case ExtendedOptionTypes.KeepControlAfterBumpingHead:
			case ExtendedOptionTypes.FallDamageTriggeredByExplosions:
			case ExtendedOptionTypes.PauseTimerWhileFiring:
			case ExtendedOptionTypes.LossOfControlDoesntEndTurn:
			case ExtendedOptionTypes.WeaponUseDoesntEndTurn:
			case ExtendedOptionTypes.WeaponUseDoesntEndTurnDoesntBlockWeapons:
			case ExtendedOptionTypes.GirderRadiusAssist:
			case ExtendedOptionTypes.JetPackBungeeGlitch:
			case ExtendedOptionTypes.AngleCheatGlitch:
			case ExtendedOptionTypes.GlideGlitch:
			case ExtendedOptionTypes.FloatingWeaponGlitch:
			case ExtendedOptionTypes.RubberWormAirViscosityAppliesToWorms:
			case ExtendedOptionTypes.RubberWormWindInfluenceAppliesToWorms:
			case ExtendedOptionTypes.RubberWormCrateShower:
			case ExtendedOptionTypes.RubberWormAntiSink:
			case ExtendedOptionTypes.RubberWormRememberWeapons:
			case ExtendedOptionTypes.RubberWormExtendedFuses:
			case ExtendedOptionTypes.RubberWormAntiLockAim:
			case ExtendedOptionTypes.FractionalRoundTimer:
			case ExtendedOptionTypes.AutomaticEndOfTurnRetreat:
			case ExtendedOptionTypes.ConserveInstantUtilities:
			case ExtendedOptionTypes.ExpediteInstantUtilities:
				return new SettingLimits(0, 1);

			//Tri-state properties.
			case ExtendedOptionTypes.ExplosionsPushAllObjects:
			case ExtendedOptionTypes.UndeterminedCrates:
			case ExtendedOptionTypes.UndeterminedFuses:
			case ExtendedOptionTypes.PnuematicDrillImpartsVelocity:
			case ExtendedOptionTypes.IndianRopeGlitch:
			case ExtendedOptionTypes.HerdDoublingGlitch:
			case ExtendedOptionTypes.TerrainOverlapPhasingGlitch:
				return new SettingLimits((int)ExtendedOptionsTriState.False, (int)ExtendedOptionsTriState.True,
					(int)ExtendedOptionsTriState.Default, (int)ExtendedOptionsTriState.Default);

			//Byte properties.
			case ExtendedOptionTypes.WindBias:
			case ExtendedOptionTypes.RopeKnocking:
			case ExtendedOptionTypes.BloodLevel:
			case ExtendedOptionTypes.SuddenDeathWormDamagePerTurn:
			case ExtendedOptionTypes.RubberWormCrateRate:
			case ExtendedOptionTypes.DoubleTimeStackLimit:
				return new SettingLimits(Byte.MinValue, Byte.MaxValue);

			//Integer-wide properties.
			case ExtendedOptionTypes.Wind:
				return new SettingLimits(Int16.MinValue, Int16.MaxValue);

			case ExtendedOptionTypes.MaximumCrateCount:
			case ExtendedOptionTypes.PetrolTurnDecay:
				return new SettingLimits(UInt16.MinValue, UInt16.MaxValue);

			//Enum properties.
			case ExtendedOptionTypes.AlliedPhasedWorms:
			case ExtendedOptionTypes.EnemyPhasedWorms:
				return new SettingLimits(0, (int)PhasedWormsModes.Count - 1);

			case ExtendedOptionTypes.RopeRollDrops:
				return new SettingLimits(0, (int)RopeRollDropModes.Count - 1);

			case ExtendedOptionTypes.XImpactLossOfControl:
				return new SettingLimits((int)XImpactLossOfControlModes.LossOfControl, (int)XImpactLossOfControlModes.LossOfControl,
					(int)XImpactLossOfControlModes.NoLossOfControl, (int)XImpactLossOfControlModes.NoLossOfControl);

			case ExtendedOptionTypes.KeepControlAfterSkimming:
				return new SettingLimits(0, (int)KeepControlAfterSkimmingModes.Count - 1);

			case ExtendedOptionTypes.Skipwalking:
				return new SettingLimits(0, (int)SkipwalkingModes.Faciliated,
					(int)SkipwalkingModes.Disabled, (int)SkipwalkingModes.Disabled);

			case ExtendedOptionTypes.BlockRoofing:
				return new SettingLimits(0, (int)BlockRoofingModes.Count - 1);

			case ExtendedOptionTypes.RubberWormGravityType:
				return new SettingLimits(0, (int)RubberWormGravityTypes.Count - 1);

			case ExtendedOptionTypes.HealthCratesCurePoison:
				return new SettingLimits(0, (int)HealthCratesCurePoisonModes.Allies,
					(int)HealthCratesCurePoisonModes.None, (int)HealthCratesCurePoisonModes.None);

			case ExtendedOptionTypes.RubberWormKaosMod:
				return new SettingLimits(0, (int)RubberWormKaosMods.Count - 1);

			//Speed properties.
			case ExtendedOptionTypes.MaximumProjectileSpeed:
			case ExtendedOptionTypes.MaximumRopeSpeed:
			case ExtendedOptionTypes.MaximumJetPackSpeed:
				return new SettingLimits(0, 0x7FFFFFFF);

			//Other properties.
			case ExtendedOptionTypes.Gravity:
				return new SettingLimits(0x01, 0xC80000);

			case ExtendedOptionTypes.Friction:
				return new SettingLimits(0, 0x28CCC);

			case ExtendedOptionTypes.NoCrateProbability:
				return new SettingLimits(0, 100, 255, 255);

			case ExtendedOptionTypes.PetrolTouchDecay:
				return new SettingLimits(1, Byte.MaxValue);

			case ExtendedOptionTypes.MaximumFlameletCount:
				return new SettingLimits(1, UInt16.MaxValue);

			case ExtendedOptionTypes.GameEngineSpeed:
				return new SettingLimits(0x1000, 0x800000);

			case ExtendedOptionTypes.RubberWormBounciness:
			case ExtendedOptionTypes.RubberWormWindInfluence:
				return new SettingLimits(0, 0x10000);

			case ExtendedOptionTypes.RubberWormAirViscosity:
				return new SettingLimits(0, 0x4000);

			case ExtendedOptionTypes.RubberWormGravityStrength:
				return new SettingLimits(-0x40000000, 0x40000000);

			case ExtendedOptionTypes.SheepHeavensGate:
				return new SettingLimits(1, 7);
			}

			Debug.Fail("InitialiseExtendedOptionLimit - Invalid extended option " + extendedOption.ToString());
			return null;
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

		public static SettingLimits GetExtendedOptionLimits(ExtendedOptionTypes extendedOption)
		{
			Debug.Assert(extendedOption < ExtendedOptionTypes.Count);
			return extendedOption < ExtendedOptionTypes.Count ? _extendedOptionLimits[(int)extendedOption] : null;
		}

		static SettingLimits[] _settingLimits = new SettingLimits[(int)SettingTypes.Count];
		static WeaponLimits[] _weaponLimits = new WeaponLimits[(int)WeaponTypes.Count];
		static SettingLimits[] _extendedOptionLimits = new SettingLimits[(int)ExtendedOptionTypes.Count];
	}
}
