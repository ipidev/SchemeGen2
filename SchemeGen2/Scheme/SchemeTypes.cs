using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
	enum SchemeVersion
	{
		Armageddon1,
		WorldParty,
		Armageddon2,
		Armageddon3
	}

	enum SettingTypes
	{
		Version,
		HotSeatDelay,
		RetreatTime,
		RopeRetreatTime,
		DisplayTotalRoundTime,
		AutomaticReplays,
		FallDamage,
		ArtilleryMode,
		BountyMode,
		StockpilingMode,
		WormSelect,
		SuddenDeathEvent,
		WaterRiseRate,
		WeaponCrateProbability,
		DonorCards,
		HealthCrateProbability,
		HealthCrateEnergy,
		UtilityCrateProbability,
		HazardousObjectTypes,
		MineDelay,
		DudMines,
		InitialWormPlacement,
		InitialWormEnergy,
		TurnTime,
		RoundTime,
		NumberOfRounds,
		Blood,
		AquaSheep,
		SheepHeaven,
		GodWorms,
		IndestructibleTerrain,
		UpgradedGrenade,
		UpgradedShotgun,
		UpgradedCluster,
		UpgradedLongbow,
		TeamWeapons,
		SuperWeapons,

		Count
	}

	enum WeaponTypes
	{
		Bazooka,
		HomingMissile,
		Mortar,
		Grenade,
		ClusterBomb,
		Skunk,
		PetrolBomb,
		BananaBomb,
		Handgun,
		Shotgun,
		Uzi,
		Minigun,
		Longbow,
		AirStrike,
		NapalmStrike,
		Mine,
		FirePunch,
		DragonBall,
		Kamikaze,
		Prod,
		BattleAxe,
		BlowTorch,
		PneumaticDrill,
		Girder,
		NinjaRope,
		Parachute,
		Bungee,
		Teleport,
		Dynamite,
		Sheep,
		BaseballBat,
		FlameThrower,
		HomingPigeon,
		MadCow,
		HolyHandGrenade,
		OldWoman,
		SheepLauncher,
		SuperSheep,
		MoleBomb,
		JetPack,
		LowGravity,
		LaserSight,
		FastWalk,
		Invisibility,
		DoubleDamage,

		Freeze,
		SuperBananaBomb,
		MineStrike,
		GirderStarterPack,
		Earthquake,
		ScalesOfJustice,
		MingVase,
		CarpetBomb,
		MagicBullet,
		NuclearTest,
		SelectWorm,
		SalvationArmy,
		MoleSquadron,
		MBBomb,
		ConcreteDonkey,
		SuicideBomber,
		SheepStrike,
		MailStrike,
		Armageddon,

		Count,
		Default
	}

	enum WeaponSettings
	{
		Ammo,
		Power,
		Delay,
		Crate,

		Count
	}

	[Flags]
	enum WeaponCategoryFlags
	{
		/// <summary>
		/// Set if the weapon is affected by the Super Weapons setting.
		/// </summary>
		SuperWeapon		= 1 << 0,
		/// <summary>
		/// Set if the weapon is affected by the Team Weapons setting.
		/// </summary>
		TeamWeapon		= 1 << 1,
		/// <summary>
		/// Set if the weapon can deal lethal damage to other worms.
		/// </summary>
		Damaging		= 1 << 2,
		/// <summary>
		/// Set if the weapon is obtainable from utility crates.
		/// </summary>
		Utility			= 1 << 3,
		/// <summary>
		/// Set if the weapon fires a projectile upon use.
		/// </summary>
		Launched		= 1 << 4,
		/// <summary>
		/// Set if the weapon appears in front of the player upon use.
		/// </summary>
		Dropped			= 1 << 5,
		/// <summary>
		/// Set if the weapon is unusable in cavern maps.
		/// </summary>
		Strike			= 1 << 6,
		/// <summary>
		/// Set if the weapon appears as a (relatively) convential firearm.
		/// </summary>
		Firearm			= 1 << 7,
		/// <summary>
		/// Set if the weapon requires direct contact to be effective.
		/// </summary>
		Melee			= 1 << 8,
		/// <summary>
		/// Set if using the weapon applies an effect to the entire map.
		/// </summary>
		Global			= 1 << 9,
		/// <summary>
		/// Set if the weapon attempts to move towards a pre-determined target.
		/// </summary>
		Homing			= 1 << 10,
		/// <summary>
		/// Set if the weapon splits into smaller projectiles upon detonation.
		/// </summary>
		Cluster			= 1 << 11,
		/// <summary>
		/// Set if the weapon detonates when a player-visible fuse reaches 0.
		/// </summary>
		Fused			= 1 << 12,
		/// <summary>
		/// Set if the player can manually affect the weapon after it has been used.
		/// </summary>
		RemoteControl	= 1 << 13,
		/// <summary>
		/// Set if the weapon appears as something fluffy (or fleshy).
		/// </summary>
		Animal			= 1 << 14,
		/// <summary>
		/// Set if the weapon is capable of poisoning players.
		/// </summary>
		Poisonous		= 1 << 15,
		/// <summary>
		/// Set if the weapon creates fire particles.
		/// </summary>
		Flammable		= 1 << 16,
		/// <summary>
		/// Set if the weapon helps the player navigate the map.
		/// </summary>
		Traversal		= 1 << 17,
		/// <summary>
		/// Set if the weapon can be used more than once before expending the ammunition.
		/// </summary>
		MultiUse		= 1 << 18,

		None			= 0,
		All				= MultiUse - 1
	}

	enum WeaponCategoryMatchType
	{
		Any,
		All,
		Exact
	}

	enum WeaponFunctionKeys
	{
		Utilities,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		
		Count
	}

	enum StockpilingModes : byte
	{
		Off,
		On,
		Anti,

		Count
	}

	enum WormSelectModes : byte
	{
		Off,
		On,
		Random,

		Count
	}


	enum SuddenDeathEvents : byte
	{
		RoundEnds,
		NuclearStrike,
		OneHitPoint,
		Nothing,

		Count
	}

	enum RubberWormSettings
	{
		Bounciness          = WeaponTypes.Armageddon,
		Gravity             = WeaponTypes.MailStrike,
		AirViscosity        = WeaponTypes.ConcreteDonkey,
		WindInfluence       = WeaponTypes.SuicideBomber,
		AntiWormSink        = WeaponTypes.SheepStrike,
		Friction            = WeaponTypes.SalvationArmy,
		MoleSquadronFlags   = WeaponTypes.MoleSquadron,
		VersionOverrideLow  = WeaponTypes.SelectWorm,
		VersionOverrideHigh = WeaponTypes.Freeze,
		CrateRate           = WeaponTypes.NuclearTest,
		CrateLimit          = WeaponTypes.MagicBullet,
		FlameLimit          = WeaponTypes.ScalesOfJustice,
		EarthquakeFlags     = WeaponTypes.Earthquake,
		SelectWormAnyTime   = WeaponTypes.MBBomb,
		KnockForce          = WeaponTypes.SuperBananaBomb,
		RopeSpeedLimit      = WeaponTypes.MineStrike,

		Count
	}

	enum MoleSquadronFlags
	{
		ShotDoesntEndTurn               = 1,
		LossOfControlDoesntEndTurn      = 1 << 1,
		FireDoesntPauseTimer            = 1 << 2,
		ImprovedRope                    = 1 << 3,
		ContinousCrateShower            = 1 << 4,
		AllObjectsPushedByExplosions    = 1 << 5,
		WeaponsDontChangeAutomatically  = 1 << 6,
		FuseExtension                   = 1 << 7,

		Count
	}

	enum EarthquakeFlags
	{
		ReAimOnTurnStart                = 1,
		CircularAiming                  = 1 << 1,
		AntiLockPower                   = 1 << 2,
		UnlockShotDoesntEndTurnWeapons  = 1 << 3,
		Kaosmod1                        = 1 << 4,
		Kaosmod2                        = 2 << 4,
		Kaosmod3                        = 5 << 4,
		Kaosmod4                        = 7 << 4,
		Kaosmod5                        = 9 << 4,

		Count
	}

	enum ExtendedOptionTypes
	{
		DataVersion,
		ConstantWind,
		Wind,
		WindBias,
		Gravity,
		Friction,
		RopeKnocking,
		BloodLevel,
		UnrestrictRope,
		AutoPlaceWormsByAlly,
		NoCrateProbability,
		MaximumCrateCount,
		SuddenDeathDisablesWormSelect,
		SuddenDeathWormDamagePerTurn,
		AlliedPhasedWorms,
		EnemyPhasedWorms,
		CircularAim,
		AntiLockAim,
		AntiLockPower,
		WormSelectionDoesntEndHotSeat,
		WormSelectionNeverCancelled,
		BattyRope,
		RopeRollDrops,
		XImpactLossOfControl,
		KeepControlAfterBumpingHead,
		KeepControlAfterSkimming,
		FallDamageTriggeredByExplosions,
		ExplosionsPushAllObjects,
		UndeterminedCrates,
		UndeterminedFuses,
		PauseTimerWhileFiring,
		LossOfControlDoesntEndTurn,
		WeaponUseDoesntEndTurn,
		WeaponUseDoesntEndTurnDoesntBlockWeapons,
		PneumaticDrillImpartsVelocity,
		GirderRadiusAssist,
		PetrolTurnDecay,
		PetrolTouchDecay,
		MaximumFlameletCount,
		MaximumProjectileSpeed,
		MaximumRopeSpeed,
		MaximumJetPackSpeed,
		GameEngineSpeed,
		IndianRopeGlitch,
		HerdDoublingGlitch,
		JetPackBungeeGlitch,
		AngleCheatGlitch,
		GlideGlitch,
		Skipwalking,
		BlockRoofing,
		FloatingWeaponGlitch,
		RubberWormBounciness,
		RubberWormAirViscosity,
		RubberWormAirViscosityAppliesToWorms,
		RubberWormWindInfluence,
		RubberWormWindInfluenceAppliesToWorms,
		RubberWormGravityType,
		RubberWormGravityStrength,
		RubberWormCrateRate,
		RubberWormCrateShower,
		RubberWormAntiSink,
		RubberWormRememberWeapons,
		RubberWormExtendedFuses,
		RubberWormAntiLockAim,
		TerrainOverlapPhasingGlitch,
		FractionalRoundTimer,
		AutomaticEndOfTurnRetreat,
		HealthCratesCurePoison,
		RubberWormKaosMod,
		SheepHeavensGate,
		ConserveInstantUtilities,
		ExpediteInstantUtilities,
		DoubleTimeStackLimit,

		Count
	}

	enum ExtendedOptionsTriState : byte
	{
		False,
		True,
		Default = 0x80
	}

	enum PhasedWormsModes : byte
	{
		Off,
		Worms,
		WormsWeapons,
		WormsWeaponsDamage,

		Count
	}

	enum RopeRollDropModes : byte
	{
		Disabled,
		AsFromRope,
		AsFromRopeOrJump,

		Count
	}

	enum XImpactLossOfControlModes : byte
	{
		NoLossOfControl = 0xFF,
		LossOfControl = 0x00
	}

	enum KeepControlAfterSkimmingModes : byte
	{
		LoseControl,
		KeepControl,
		KeepControlAndRope,
		
		Count
	}

	enum SkipwalkingModes : byte
	{
		Disabled = 0xFF,
		Possible = 0,
		Faciliated
	}

	enum BlockRoofingModes : byte
	{
		Allowed,
		BlockAbove,
		BlockEverywhere,
		
		Count
	}

	enum RubberWormGravityTypes : byte
	{
		Unmodified,
		Standard,
		ConstantBlackHole,
		LinearBlackHole,
		
		Count
	}

	enum HealthCratesCurePoisonModes : byte
	{
		None = 0xFF,
		Collector = 0,
		Team,
		Allies
	}

	enum RubberWormKaosMods : byte
	{
		Standard,
		KaosMod1,
		KaosMod2,
		KaosMod3,
		KaosMod4,
		KaosMod5,
		
		Count
	}

	[Flags]
	enum SheepHeavensGate : byte
	{
		SheepFromCrateExplosion = 0x01,
		ExtendedFuseTime = 0x02,
		IncreasedCrateProbabilities = 0x04
	}

	static class SchemeTypes
	{
		public const int False = 0;
		public const int True = 1;

		/// <summary>
		/// The number of game settings in the scheme that don't apply to weapons.
		/// </summary>
		public const int NumberOfNonWeaponSettings = (int)SettingTypes.Count;

		/// <summary>
		/// The number of non-super weapons in the scheme.
		/// </summary>
		public const int NumberOfNonSuperWeapons = (int)WeaponTypes.Freeze;

		/// <summary>
		/// The number of weapons in the scheme, including super weapons.
		/// </summary>
		public const int NumberOfWeapons = (int)WeaponTypes.Count;

		/// <summary>
		/// The total number of individual weapon parameters, including super weapons.
		/// </summary>
		public const int NumberOfWeaponSettings = NumberOfWeapons * (int)WeaponSettings.Count;

		/// <summary>
		/// The number of settings for most recent version of the extended scheme options.
		/// </summary>
		public const int NumberOfExtendedOptions = (int)ExtendedOptionTypes.Count;

		private static readonly WeaponCategoryFlags[] WeaponCategoryFlagsArray = 
		{
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Homing,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Cluster,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Fused,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Fused | WeaponCategoryFlags.Cluster,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Animal | WeaponCategoryFlags.Poisonous,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Flammable,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Fused | WeaponCategoryFlags.Cluster,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Firearm,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Firearm | WeaponCategoryFlags.MultiUse,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Firearm,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Firearm,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Firearm | WeaponCategoryFlags.MultiUse,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike | WeaponCategoryFlags.Flammable,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Fused,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee,
			WeaponCategoryFlags.Melee,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee | WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee | WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Traversal | WeaponCategoryFlags.MultiUse,
			WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Fused,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Melee,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Firearm | WeaponCategoryFlags.Flammable,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging |  WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Homing | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Fused,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Animal | WeaponCategoryFlags.RemoteControl,
			WeaponCategoryFlags.TeamWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Animal | WeaponCategoryFlags.RemoteControl,
			WeaponCategoryFlags.Utility | WeaponCategoryFlags.Traversal | WeaponCategoryFlags.RemoteControl,
			WeaponCategoryFlags.Utility | WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Utility | WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Utility | WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Utility | WeaponCategoryFlags.Traversal,
			WeaponCategoryFlags.Utility,
			WeaponCategoryFlags.SuperWeapon,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Cluster | WeaponCategoryFlags.RemoteControl,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike | WeaponCategoryFlags.Fused,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Traversal | WeaponCategoryFlags.MultiUse,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Global,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Global,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Cluster | WeaponCategoryFlags.Fused,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Launched | WeaponCategoryFlags.Homing,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Global | WeaponCategoryFlags.Poisonous,
			WeaponCategoryFlags.SuperWeapon,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Dropped | WeaponCategoryFlags.Cluster | WeaponCategoryFlags.RemoteControl,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Poisonous,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike | WeaponCategoryFlags.Animal,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Strike,
			WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Damaging | WeaponCategoryFlags.Global
		};

		private static readonly WeaponTypes[][] WeaponsPerFunctionKey =
		{
			new WeaponTypes[] { WeaponTypes.JetPack,		WeaponTypes.LowGravity,		WeaponTypes.FastWalk,		WeaponTypes.LaserSight,		WeaponTypes.Invisibility },
			new WeaponTypes[] { WeaponTypes.Bazooka,		WeaponTypes.HomingMissile,	WeaponTypes.Mortar,			WeaponTypes.HomingPigeon,	WeaponTypes.SheepLauncher },
			new WeaponTypes[] { WeaponTypes.Grenade,		WeaponTypes.ClusterBomb,	WeaponTypes.BananaBomb,		WeaponTypes.BattleAxe,		WeaponTypes.Earthquake },
			new WeaponTypes[] { WeaponTypes.Shotgun,		WeaponTypes.Handgun,		WeaponTypes.Uzi,			WeaponTypes.Minigun,		WeaponTypes.Longbow },
			new WeaponTypes[] { WeaponTypes.FirePunch,		WeaponTypes.DragonBall,		WeaponTypes.Kamikaze,		WeaponTypes.SuicideBomber,	WeaponTypes.Prod },
			new WeaponTypes[] { WeaponTypes.Dynamite,		WeaponTypes.Mine,			WeaponTypes.Sheep,			WeaponTypes.SuperSheep,		WeaponTypes.MoleBomb },
			new WeaponTypes[] { WeaponTypes.AirStrike,		WeaponTypes.NapalmStrike,	WeaponTypes.MailStrike,		WeaponTypes.MineStrike,		WeaponTypes.MoleSquadron },
			new WeaponTypes[] { WeaponTypes.BlowTorch,		WeaponTypes.PneumaticDrill, WeaponTypes.Girder,			WeaponTypes.BaseballBat,	WeaponTypes.GirderStarterPack },
			new WeaponTypes[] { WeaponTypes.NinjaRope,		WeaponTypes.Bungee,			WeaponTypes.Parachute,		WeaponTypes.Teleport,		WeaponTypes.ScalesOfJustice },
			new WeaponTypes[] { WeaponTypes.SuperBananaBomb,WeaponTypes.HolyHandGrenade,WeaponTypes.FlameThrower,	WeaponTypes.SalvationArmy,	WeaponTypes.MBBomb },
			new WeaponTypes[] { WeaponTypes.PetrolBomb,		WeaponTypes.Skunk,			WeaponTypes.MingVase,		WeaponTypes.SheepStrike,	WeaponTypes.CarpetBomb },
			new WeaponTypes[] { WeaponTypes.MadCow,			WeaponTypes.OldWoman,		WeaponTypes.ConcreteDonkey,	WeaponTypes.NuclearTest,	WeaponTypes.Armageddon },
			new WeaponTypes[] { /* Skip Go */				/* Surrender */				WeaponTypes.SelectWorm,		WeaponTypes.Freeze,			WeaponTypes.MagicBullet },
		};

		private static readonly int[] ExtendedOptionSettingsCounts =
		{
			(int)ExtendedOptionTypes.DoubleTimeStackLimit + 1
		};

		/// <summary>
		/// The maximum extended option data version that is supported.
		/// </summary>
		public static readonly int MaximumExtendedOptionDataVersion = ExtendedOptionSettingsCounts.Length - 1;

		public static bool IsTeamWeapon(WeaponTypes weaponType)
		{
			return HasWeaponCategoryFlags(weaponType, WeaponCategoryFlags.TeamWeapon);
		}

		public static bool IsUtility(WeaponTypes weaponType)
		{
			return HasWeaponCategoryFlags(weaponType, WeaponCategoryFlags.Utility);
		}

		public static bool IsSuperWeapon(WeaponTypes weaponType)
		{
			return HasWeaponCategoryFlags(weaponType, WeaponCategoryFlags.SuperWeapon);
		}

		public static bool HasWeaponCategoryFlags(WeaponTypes weaponType, WeaponCategoryFlags flags, WeaponCategoryMatchType matchType = WeaponCategoryMatchType.Any)
		{
			WeaponCategoryFlags weaponFlags = WeaponCategoryFlagsArray[(int)weaponType];
			switch (matchType)
			{
			case WeaponCategoryMatchType.Any:
				return (weaponFlags & flags) != 0;

			case WeaponCategoryMatchType.All:
				return (weaponFlags & flags) == flags;

			case WeaponCategoryMatchType.Exact:
				return (weaponFlags & ~flags) == 0;
			}

			return false;
		}

		public static WeaponTypes[] GetWeaponsForFunctionKey(WeaponFunctionKeys weaponFunctionKey)
		{
			return weaponFunctionKey < WeaponFunctionKeys.Count ? WeaponsPerFunctionKey[(int)weaponFunctionKey] : null;
		}

		public static WeaponFunctionKeys GetFunctionKeyForWeapon(WeaponTypes weaponType)
		{
			for (int i = 0; i < (int)WeaponFunctionKeys.Count; ++i)
			{
				if (WeaponsPerFunctionKey[i].Contains(weaponType))
					return (WeaponFunctionKeys)i;
			}

			return WeaponFunctionKeys.Count;
		}

		public static bool HasRubberWormSetting(WeaponTypes weaponType)
		{
			return (Enum.IsDefined(typeof(RubberWormSettings), (int)weaponType));
		}

		public static bool WeaponTypeToRubberWormSetting(WeaponTypes weaponType, out RubberWormSettings rubberWormSetting)
		{
			if (HasRubberWormSetting(weaponType))
			{
				rubberWormSetting = (RubberWormSettings)weaponType;
				return true;
			}
			else
			{
				rubberWormSetting = RubberWormSettings.Count;
				return false;
			}
		}

		public static bool CanApplyWeaponSetting(WeaponTypes weaponType, WeaponSettings weaponSetting)
		{
			if (weaponType == WeaponTypes.Default)
				return true;

			switch (weaponSetting)
			{
			case WeaponSettings.Ammo:
			case WeaponSettings.Delay:
				return true;

			case WeaponSettings.Power:
				return (!HasWeaponCategoryFlags(weaponType, WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Utility)
					&& weaponType != WeaponTypes.Parachute && weaponType != WeaponTypes.Bungee  && weaponType != WeaponTypes.Teleport)
					|| weaponType == WeaponTypes.JetPack;

			case WeaponSettings.Crate:
				return !HasWeaponCategoryFlags(weaponType, WeaponCategoryFlags.SuperWeapon | WeaponCategoryFlags.Utility);
			}

			return false;
		}

		public static int GetSchemeVersionNumber(SchemeVersion version)
		{
			switch (version)
			{
			case SchemeVersion.Armageddon1:
			case SchemeVersion.WorldParty:
				return 1;

			case SchemeVersion.Armageddon2:
				return 2;

			case SchemeVersion.Armageddon3:
				return 3;
			}

			return -1;
		}

		public static int GetExtendedOptionsSettingsCount(int version)
		{
			if (version < 0 || version >= ExtendedOptionSettingsCounts.Length)
			{
				throw new ArgumentOutOfRangeException("version", version, "Version is invalid or not supported.");
			}

			return ExtendedOptionSettingsCounts[version];
		}

		public static SettingSize GetExtendedOptionSettingSize(ExtendedOptionTypes extendedOption)
		{
			switch(extendedOption)
			{
			case ExtendedOptionTypes.Wind:
			case ExtendedOptionTypes.MaximumCrateCount:
			case ExtendedOptionTypes.PetrolTurnDecay:
			case ExtendedOptionTypes.MaximumFlameletCount:
				return SettingSize.TwoBytes;

			case ExtendedOptionTypes.DataVersion:
			case ExtendedOptionTypes.Gravity:
			case ExtendedOptionTypes.Friction:
			case ExtendedOptionTypes.MaximumProjectileSpeed:
			case ExtendedOptionTypes.MaximumRopeSpeed:
			case ExtendedOptionTypes.MaximumJetPackSpeed:
			case ExtendedOptionTypes.GameEngineSpeed:
			case ExtendedOptionTypes.RubberWormBounciness:
			case ExtendedOptionTypes.RubberWormAirViscosity:
			case ExtendedOptionTypes.RubberWormWindInfluence:
			case ExtendedOptionTypes.RubberWormGravityStrength:
				return SettingSize.FourBytes;
			}

			return SettingSize.Byte;
		}
	}
}
