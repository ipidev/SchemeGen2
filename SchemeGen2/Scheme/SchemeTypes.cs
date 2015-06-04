using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
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
        WeaponCrateProbabilty,
        DonorCards,
        HealthCrateProbability,
        HealthCrateEnergy,
        UtilityCrateProbabilty,
        HazardousObjectTypes,
        MineDelay,
        DudMines,
        WormPlacement,
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
        Invisiblity,
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
        NormalWeaponCount = Freeze
    }

    enum WeaponSettings
    {
        Ammo,
        Power,
        Delay,
        Crate,

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

    static class SchemeTypes
    {
        public const byte False = 0x00;
        public const byte True = 0x01;

        /// <summary>
        /// The number of game settings in the scheme that don't apply to weapons.
        /// </summary>
        public const int NumberOfNonWeaponSettings = (int)SettingTypes.Count;

        /// <summary>
        /// The number of non-super weapons in the scheme.
        /// </summary>
        public const int NumberOfNonSuperWeapons = (int)WeaponTypes.NormalWeaponCount;

        /// <summary>
        /// The number of weapons in the scheme, including super weapons.
        /// </summary>
        public const int NumberOfWeapons = (int)WeaponTypes.Count;

        /// <summary>
        /// The total number of individual weapon parameters, including super weapons.
        /// </summary>
        public const int NumberOfWeaponSettings = NumberOfWeapons * (int)WeaponSettings.Count;

        public static bool IsTeamWeapon(WeaponTypes weaponType)
        {
            switch (weaponType)
            {
            case WeaponTypes.FlameThrower:
            case WeaponTypes.HomingPigeon:
            case WeaponTypes.MadCow:
            case WeaponTypes.HolyHandGrenade:
            case WeaponTypes.OldWoman:
            case WeaponTypes.SheepLauncher:
            case WeaponTypes.SuperSheep:
            case WeaponTypes.MoleBomb:
                return true;

            default:
                return false;
            }
        }

        public static bool IsUtility(WeaponTypes weaponType)
        {
            switch (weaponType)
            {
            case WeaponTypes.JetPack:
            case WeaponTypes.LowGravity:
            case WeaponTypes.LaserSight:
            case WeaponTypes.FastWalk:
            case WeaponTypes.Invisiblity:
            case WeaponTypes.DoubleDamage:
                return true;

            default:
                return false;
            }
        }

        public static bool IsSuperWeapon(WeaponTypes weaponType)
        {
            return (weaponType >= WeaponTypes.NormalWeaponCount);
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
    }
}
