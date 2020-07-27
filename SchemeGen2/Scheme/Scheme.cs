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

		public const int SchemeSettingsStartIndex = 4;
		public const int SchemeWeaponsStartIndex = SchemeSettingsStartIndex + SchemeTypes.NumberOfNonWeaponSettings;
		public const int SchemeFileLength = SchemeWeaponsStartIndex + SchemeTypes.NumberOfWeaponSettings;

		public Scheme(bool useRubberWorm)
			: this(useRubberWorm, true)
		{
		}

		public Scheme(bool useRubberWorm, bool setUpDefaults)
		{
			Settings = new Setting[SchemeTypes.NumberOfNonWeaponSettings];
			Weapons = new Weapon[SchemeTypes.NumberOfWeapons];
			_useRubberWorm = useRubberWorm;

			Initialise();

			if (setUpDefaults)
			{
				SetUpDefaults();
			}
		}

		public Setting[] Settings { get; private set; }
		public Weapon[] Weapons { get; private set; }

		///////////////////////////////////////////////////////////////////////
		// Initialisation

		public void Initialise()
		{
			for (int i = 0; i < Settings.Length; ++i)
			{
				InitialiseSetting((SettingTypes)i, ref Settings[i]);
			}

			for (int i = 0; i < Weapons.Length; ++i)
			{
				InitialiseWeapon((WeaponTypes)i, ref Weapons[i]);
			}
		}

		public void InitialiseSetting(SettingTypes settingType, ref Setting setting)
		{
			setting = new Setting();

			switch (settingType)
			{
			case SettingTypes.Version:
				setting.SetRange(WormsSchemeVersion, WormsSchemeVersion);
				setting.SetValue(WormsSchemeVersion);
				break;

			case SettingTypes.BountyMode:
				setting.SetRange(SchemeGeneratorMagicNumber, SchemeGeneratorMagicNumber);
				setting.SetValue(SchemeGeneratorMagicNumber);
				break;

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
				setting.SetRange(0x00, 0xFF);
				break;
			}
		}
	
		public void InitialiseWeapon(WeaponTypes weaponType, ref Weapon weapon)
		{
			weapon = new Weapon(weaponType);

			InitialiseWeaponPowerSettings(weaponType, ref weapon);
			InitialiseWeaponCrateSettings(weaponType, ref weapon);
		}

		public void InitialiseWeaponPowerSettings(WeaponTypes weaponType, ref Weapon weapon)
		{
			if (SchemeTypes.IsSuperWeapon(weaponType) || (weaponType != WeaponTypes.JetPack && SchemeTypes.IsUtility(weaponType)) ||
				weaponType == WeaponTypes.Parachute || weapon.WeaponType == WeaponTypes.Bungee || weapon.WeaponType == WeaponTypes.Teleport)
			{
				//TODO: Jetpack with RubberWorm active.
				weapon.Power.SetRange(0x00, 0x00);
			}
			else if (weaponType != WeaponTypes.JetPack)
			{
				weapon.Power.SetValue(2);
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
			Access(SettingTypes.HotSeatDelay).SetValue          (5);
			Access(SettingTypes.RetreatTime).SetValue           (3);
			Access(SettingTypes.RopeRetreatTime).SetValue       (5);
			Access(SettingTypes.DisplayTotalRoundTime).SetValue (SchemeTypes.False);
			Access(SettingTypes.AutomaticReplays).SetValue      (SchemeTypes.True);
			Access(SettingTypes.FallDamage).SetValue            (SchemeTypes.True);
			Access(SettingTypes.ArtilleryMode).SetValue         (SchemeTypes.False);
			Access(SettingTypes.StockpilingMode).SetValue       ((byte)StockpilingModes.Off);
			Access(SettingTypes.WormSelect).SetValue            ((byte)WormSelectModes.Off);
			Access(SettingTypes.SuddenDeathEvent).SetValue      ((byte)SuddenDeathEvents.OneHitPoint);
			Access(SettingTypes.WaterRiseRate).SetValue         (0x02);
			Access(SettingTypes.DonorCards).SetValue            (SchemeTypes.False);
			Access(SettingTypes.HealthCrateEnergy).SetValue     (25);
			Access(SettingTypes.HazardousObjectTypes).SetValue  (0x05);
			Access(SettingTypes.MineDelay).SetValue             (3);
			Access(SettingTypes.DudMines).SetValue              (SchemeTypes.True);
			Access(SettingTypes.InitialWormPlacement).SetValue  (SchemeTypes.False);
			Access(SettingTypes.InitialWormEnergy).SetValue     (100);
			Access(SettingTypes.TurnTime).SetValue              (45);
			Access(SettingTypes.RoundTime).SetValue             (15);
			Access(SettingTypes.NumberOfRounds).SetValue        (1);
			Access(SettingTypes.TeamWeapons).SetValue           (SchemeTypes.True);
			Access(SettingTypes.SuperWeapons).SetValue          (SchemeTypes.True);
		}

		///////////////////////////////////////////////////////////////////////
		// Accessors

		/// <summary>
		/// Gets the given setting by reference.
		/// </summary>
		public Setting Access(SettingTypes setting)
		{
			Debug.Assert(setting < SettingTypes.Count);
			return Settings[(int)setting];
		}

		/// <summary>
		/// Gets the given weapon by reference.
		/// </summary>
		public Weapon Access(WeaponTypes weapon)
		{
			Debug.Assert(weapon < WeaponTypes.Count);
			return Weapons[(int)weapon];
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
				bytes[i] = Settings[i].Value;
			}

			return bytes;
		}

		byte[] GetWeaponSettingsBytes()
		{
			byte[] bytes = new byte[SchemeTypes.NumberOfWeaponSettings];

			for (int i = 0; i < SchemeTypes.NumberOfWeapons; ++i)
			{
				int baseByteIndex = i * (int)WeaponSettings.Count;

			   Weapons[i].GetBytes().CopyTo(bytes, baseByteIndex);
			}

			return bytes;
		}

		bool _useRubberWorm;
	}
}
