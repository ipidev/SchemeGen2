using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
    class Scheme
    {
        //Scheme file-specific constants.
        const byte WormsSchemeVersion = 0x02;
        static readonly byte[] SchemeFileMagicNumber = { 0x53, 0x43, 0x48, 0x4D };
        const byte SchemeGeneratorMagicNumber = 0x17;

        public const int SchemeSettingsStartIndex = 4; //SchemeFileMagicNumber.Length;
        public const int SchemeWeaponsStartIndex = SchemeSettingsStartIndex + SchemeTypes.NumberOfNonWeaponSettings;
        public const int SchemeFileLength = SchemeWeaponsStartIndex + SchemeTypes.NumberOfWeaponSettings;

        public Scheme(bool useRubberWorm)
            : this(useRubberWorm, true)
        {
        }

        public Scheme(bool useRubberWorm, bool setUpDefaults)
        {
            _settings = new Setting[SchemeTypes.NumberOfNonWeaponSettings];
            _weapons = new Weapon[SchemeTypes.NumberOfWeapons];
            _useRubberWorm = useRubberWorm;

            Initialise();

            if (setUpDefaults)
            {
                SetUpDefaults();
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Initialisation

        public void Initialise()
        {
            for (int i = 0; i < _settings.Length; ++i)
            {
                InitialiseSetting((SettingTypes)i, ref _settings[i]);
            }

            for (int i = 0; i < _weapons.Length; ++i)
            {
                InitialiseWeapon((WeaponTypes)i, ref _weapons[i]);
            }
        }

        public void InitialiseSetting(SettingTypes settingType, ref Setting setting)
        {
            setting = new Setting();

            switch (settingType)
            {
            case SettingTypes.Version:
                setting.SetRange(WormsSchemeVersion, WormsSchemeVersion);
                setting.Value = WormsSchemeVersion;
                break;

            case SettingTypes.BountyMode:
                setting.SetRange(SchemeGeneratorMagicNumber, SchemeGeneratorMagicNumber);
                setting.Value = SchemeGeneratorMagicNumber;
                break;

            //Boolean properties.
            case SettingTypes.DisplayTotalRoundTime:
            case SettingTypes.AutomaticReplays:
            case SettingTypes.ArtilleryMode:
            case SettingTypes.DonorCards:
            case SettingTypes.DudMines:
            case SettingTypes.WormPlacement:
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
                setting.SetRangeToBoolean();
                break;

            //All ternary properties.
            case SettingTypes.StockpilingMode:
            case SettingTypes.WormSelect:
                setting.SetRange(0x00, 0x02);
                break;
            
            //All quartary properties.
            case SettingTypes.SuddenDeathEvent:
                setting.SetRange(0x00, 0x03);
                break;

            //Everything else has full byte range.
            default:
                //Nothing.
                break;
            }
        }
    
        public void InitialiseWeapon(WeaponTypes weaponType, ref Weapon weapon)
        {
            weapon = new Weapon(SchemeTypes.IsSuperWeapon(weaponType));

            InitialiseWeaponPowerSettings(weaponType, ref weapon);
            InitialiseWeaponCrateSettings(weaponType, ref weapon);
        }

        public void InitialiseWeaponPowerSettings(WeaponTypes weaponType, ref Weapon weapon)
        {
            if (SchemeTypes.IsSuperWeapon(weaponType) || SchemeTypes.IsUtility(weaponType))
            {
                //TODO: Jetpack with RubberWorm active.
                weapon.Power.SetRange(0x00, 0x00);
            }
            else
            {
                //TODO: Account for more than just 1-5 settings.
                weapon.Power.SetRange(0x00, 0x04);
                weapon.Power.Value = 0x02;
            }
        }

        public void InitialiseWeaponCrateSettings(WeaponTypes weaponType, ref Weapon weapon)
        {
            if (SchemeTypes.IsSuperWeapon(weaponType))
            {
                RubberWormSettings rubberWormSetting;

                if (_useRubberWorm && SchemeTypes.WeaponTypeToRubberWormSetting(weaponType, out rubberWormSetting))
                {
                    switch (rubberWormSetting)
                    {
                    case RubberWormSettings.AntiWormSink:
                    case RubberWormSettings.SelectWormAnyTime:
                        weapon.Crate.SetRangeToBoolean();
                        break;

                    //Everything else has full byte range.
                    default:
                        //Nothing.
                        break;
                    }
                }
                //If not using RubberWorm, the only valid crate setting is 0 for super weapons.
                else
                {
                    weapon.Crate.SetRange(0x00, 0x00);
                }
            }
            else if (SchemeTypes.IsUtility(weaponType))
            {
                weapon.Crate.SetRange(0x00, 0x00);
            }
            
            //TODO: What are the valid ranges for other weapons?
        }

        public void SetUpDefaults()
        {
            //Most of these are taken from Intermediate.
            Get(SettingTypes.HotSeatDelay).Value            = 5;
            Get(SettingTypes.RetreatTime).Value             = 3;
            Get(SettingTypes.RopeRetreatTime).Value         = 5;
            Get(SettingTypes.DisplayTotalRoundTime).Value   = SchemeTypes.False;
            Get(SettingTypes.AutomaticReplays).Value        = SchemeTypes.False;
            Get(SettingTypes.FallDamage).Value              = SchemeTypes.True;
            Get(SettingTypes.ArtilleryMode).Value           = SchemeTypes.False;
            Get(SettingTypes.StockpilingMode).Value         = (byte)StockpilingModes.Off;
            Get(SettingTypes.WormSelect).Value              = (byte)WormSelectModes.Off;
            Get(SettingTypes.SuddenDeathEvent).Value        = (byte)SuddenDeathEvents.Nothing;
            Get(SettingTypes.WaterRiseRate).Value           = 0;
            Get(SettingTypes.MineDelay).Value               = 3;
            Get(SettingTypes.InitialWormEnergy).Value       = 100;
            Get(SettingTypes.TurnTime).Value                = 45;
            Get(SettingTypes.RoundTime).Value               = 15;
            Get(SettingTypes.NumberOfRounds).Value          = 1;
            Get(SettingTypes.TeamWeapons).Value             = SchemeTypes.False;
            Get(SettingTypes.SuperWeapons).Value            = SchemeTypes.True;
        }

        ///////////////////////////////////////////////////////////////////////
        // Accessors

        public Setting Get(SettingTypes setting)
        {
            Debug.Assert(setting < SettingTypes.Count);
            return _settings[(int)setting];
        }

        public Setting this[SettingTypes setting]
        {
            get { return Get(setting); }
        }

        public Weapon Get(WeaponTypes weapon)
        {
            Debug.Assert(weapon < WeaponTypes.Count);
            return _weapons[(int)weapon];
        }

        public Weapon this[WeaponTypes weapon]
        {
            get { return Get(weapon); }
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[SchemeFileLength];
            SchemeFileMagicNumber.CopyTo(bytes, 0);

            GetSettingsBytes().CopyTo(bytes, SchemeSettingsStartIndex);
            GetWeaponSettingsBytes().CopyTo(bytes, SchemeWeaponsStartIndex);

            return bytes;
        }

        byte[] GetSettingsBytes()
        {
            byte[] bytes = new byte[SchemeTypes.NumberOfNonWeaponSettings];

            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = _settings[i].Value;
            }

            return bytes;
        }

        byte[] GetWeaponSettingsBytes()
        {
            byte[] bytes = new byte[SchemeTypes.NumberOfWeaponSettings];

            for (int i = 0; i < SchemeTypes.NumberOfWeapons; ++i)
            {
                int baseByteIndex = i * (int)WeaponSettings.Count;

               _weapons[i].GetBytes().CopyTo(bytes, baseByteIndex);
            }

            return bytes;
        }

        Setting[] _settings;
        Weapon[] _weapons;
        bool _useRubberWorm;
    }
}
