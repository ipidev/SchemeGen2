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

		public Scheme(SchemeVersion version, int extendedOptionsDataVersion = 0, bool setUpDefaults = true)
		{
			Version = version;
			
			Settings = new Setting[SchemeTypes.NumberOfNonWeaponSettings];
			for (int i = 0; i < Settings.Length; ++i)
			{
				SettingTypes settingType = (SettingTypes)i;
				Settings[i] = new Setting(settingType.ToString(), SchemeLimits.GetSettingLimits(settingType));
			}

			int weaponsCount = version >= SchemeVersion.Armageddon2 ? SchemeTypes.NumberOfWeapons : SchemeTypes.NumberOfNonSuperWeapons;
			Weapons = new Weapon[weaponsCount];
			for (int i = 0; i < Weapons.Length; ++i)
			{
				Weapons[i] = new Weapon((WeaponTypes)i);
			}

			if (version >= SchemeVersion.Armageddon3)
			{
				int optionsCount = SchemeTypes.GetExtendedOptionsSettingsCount(extendedOptionsDataVersion);
				ExtendedOptions = new Setting[optionsCount];
				for (int i = 0; i < ExtendedOptions.Length; ++i)
				{
					ExtendedOptionTypes extendedOption = (ExtendedOptionTypes)i;
					ExtendedOptions[i] = new Setting(extendedOption.ToString(), SchemeLimits.GetExtendedOptionLimits(extendedOption), SchemeTypes.GetExtendedOptionSettingSize(extendedOption));
				}

				Access(ExtendedOptionTypes.DataVersion).SetValue(extendedOptionsDataVersion);
			}

			Access(SettingTypes.Version).SetValue(SchemeTypes.GetSchemeVersionNumber(version));
			Access(SettingTypes.BountyMode).SetValue(SchemeGeneratorMagicNumber);

			if (setUpDefaults)
			{
				SetUpDefaults();
			}
		}

		public SchemeVersion Version { get; private set; }
		public Setting[] Settings { get; private set; }
		public Weapon[] Weapons { get; private set; }
		public Setting[] ExtendedOptions { get; private set; }

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

			if (Version < SchemeVersion.Armageddon3)
				return;

			//Set extended option non-zero defaults.
			Access(ExtendedOptionTypes.Wind).SetValue							(100);
			Access(ExtendedOptionTypes.WindBias).SetValue						(15);
			Access(ExtendedOptionTypes.Gravity).SetValue						(0x3D70);
			Access(ExtendedOptionTypes.Friction).SetValue						(0xF5C2);
			Access(ExtendedOptionTypes.RopeKnocking).SetValue					(255);
			Access(ExtendedOptionTypes.BloodLevel).SetValue						(255);
			Access(ExtendedOptionTypes.NoCrateProbability).SetValue				(255);
			Access(ExtendedOptionTypes.MaximumCrateCount).SetValue				(5);
			Access(ExtendedOptionTypes.SuddenDeathDisablesWormSelect).SetValue	(SchemeTypes.True);
			Access(ExtendedOptionTypes.SuddenDeathWormDamagePerTurn).SetValue	(5);
			Access(ExtendedOptionTypes.ExplosionsPushAllObjects).SetValue		((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.UndeterminedCrates).SetValue				((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.UndeterminedFuses).SetValue				((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.PauseTimerWhileFiring).SetValue			(SchemeTypes.True);
			Access(ExtendedOptionTypes.PnuematicDrillImpartsVelocity).SetValue	((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.PetrolTurnDecay).SetValue				(0x3332);
			Access(ExtendedOptionTypes.PetrolTouchDecay).SetValue				(30);
			Access(ExtendedOptionTypes.MaximumFlameletCount).SetValue			(200);
			Access(ExtendedOptionTypes.MaximumProjectileSpeed).SetValue			(0x200000);
			Access(ExtendedOptionTypes.MaximumRopeSpeed).SetValue				(0x200000);
			Access(ExtendedOptionTypes.MaximumJetPackSpeed).SetValue			(0x200000);
			Access(ExtendedOptionTypes.GameEngineSpeed).SetValue				(0x10000);
			Access(ExtendedOptionTypes.IndianRopeGlitch).SetValue				((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.HerdDoublingGlitch).SetValue				((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.JetPackBungeeGlitch).SetValue			(SchemeTypes.True);
			Access(ExtendedOptionTypes.HerdDoublingGlitch).SetValue				(SchemeTypes.True);
			Access(ExtendedOptionTypes.AngleCheatGlitch).SetValue				(SchemeTypes.True);
			Access(ExtendedOptionTypes.GlideGlitch).SetValue					(SchemeTypes.True);
			Access(ExtendedOptionTypes.FloatingWeaponGlitch).SetValue			(SchemeTypes.True);
			Access(ExtendedOptionTypes.RubberWormGravityStrength).SetValue		(0x10000);
			Access(ExtendedOptionTypes.TerrainOverlapPhasingGlitch).SetValue	((int)ExtendedOptionsTriState.Default);
			Access(ExtendedOptionTypes.HealthCratesCurePoison).SetValue			((int)HealthCratesCurePoisonModes.Team);
			Access(ExtendedOptionTypes.SheepHeavensGate).SetValue				(7);
			Access(ExtendedOptionTypes.DoubleTimeStackLimit).SetValue			(1);
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
		/// Gets the given extended option by reference.
		/// </summary>
		public Setting Access(ExtendedOptionTypes extendedOption)
		{
			Debug.Assert(extendedOption < ExtendedOptionTypes.Count);
			return ExtendedOptions[(int)extendedOption];
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

			if (ExtendedOptions != null)
			{
				foreach (Setting extendedOption in ExtendedOptions)
				{
					extendedOption.Serialise(stream);
				}
			}

			if (Version == SchemeVersion.WorldParty)
			{
				stream.Write(new byte[3], 0, 3);
				stream.Write(SchemeFileMagicNumber, 0, SchemeFileMagicNumber.Length);
				stream.WriteByte(1);
			}
		}
	}
}
