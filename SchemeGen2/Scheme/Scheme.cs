using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
	class Scheme
	{
		//Scheme file-specific constants.
		static readonly byte[] SchemeFileMagicNumber = { 0x53, 0x43, 0x48, 0x4D };
		const byte SchemeGeneratorMagicNumber = 0x17;

		public const int SchemeSettingsStartIndex = 4;
		public const int SchemeWeaponsStartIndex = SchemeSettingsStartIndex + SchemeTypes.NumberOfNonWeaponSettings;
		public const int SchemeFileLength = SchemeWeaponsStartIndex + SchemeTypes.NumberOfWeaponSettings;

		public Scheme(bool setUpDefaults = true)
		{
			Settings = new Setting[SchemeTypes.NumberOfNonWeaponSettings];
			for (int i = 0; i < Settings.Length; ++i)
			{
				SettingTypes settingType = (SettingTypes)i;
				Settings[i] = new Setting(settingType.ToString(), SchemeLimits.GetSettingLimits(settingType));
			}

			Weapons = new Weapon[SchemeTypes.NumberOfWeapons];
			for (int i = 0; i < Weapons.Length; ++i)
			{
				Weapons[i] = new Weapon((WeaponTypes)i);
			}

			Access(SettingTypes.Version).SetValue(2);
			Access(SettingTypes.BountyMode).SetValue(SchemeGeneratorMagicNumber);

			if (setUpDefaults)
			{
				SetUpDefaults();
			}
		}

		public Setting[] Settings { get; private set; }
		public Weapon[] Weapons { get; private set; }

		///////////////////////////////////////////////////////////////////////
		// Initialisation

		public void SetUpDefaults()
		{
			for (int i = 0; i < (int)WeaponTypes.Count; ++i)
			{
				WeaponTypes weaponType = (WeaponTypes)i;
				if (SchemeTypes.CanApplyWeaponSetting(weaponType, WeaponSettings.Power) &&
					weaponType != WeaponTypes.JetPack)
				{
					Access(weaponType).Power.SetValue(2);
				}
			}

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

		/// <summary>
		/// Outputs the scheme in the Worms Armageddon scheme format.
		/// </summary>
		/// <param name="stream"></param>
		public void Serialise(System.IO.Stream stream)
		{
			stream.Write(SchemeFileMagicNumber, 0, SchemeFileMagicNumber.Length);
			
			foreach (Setting setting in Settings)
			{
				setting.Serialise(stream);
			}

			foreach (Weapon weapon in Weapons)
			{
				weapon.Serialise(stream);
			}
		}
	}
}
