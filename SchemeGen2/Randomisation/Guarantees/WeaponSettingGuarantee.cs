using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2.Randomisation.Guarantees
{
	class WeaponSettingGuarantee : Guarantee
	{
		public WeaponSettingGuarantee(WeaponSettings weaponSetting)
		{
			WeaponSetting = weaponSetting;
		}

		public WeaponSettings WeaponSetting { get; private set; }
		public ValueGenerator MinimumWeaponCount { get; set; }
		public ValueGenerator MaximumWeaponCount { get; set; }
		public byte? MaximumWeaponCountForcedValue { get; set; }
		public ValueGenerator MinimumSettingValue { get; set; }
		public ValueGenerator MaximumSettingValue { get; set; }
		public WeaponCategoryFlags? CategoryInclusionFilter { get { return _categoryInclusionFilter; } }
		public WeaponCategoryMatchType InclusionMatchType { get { return _inclusionMatchType; }  }
		public WeaponCategoryFlags? CategoryExclusionFilter { get { return _categoryExclusionFilter; } }
		public WeaponCategoryMatchType ExclusionMatchType { get { return _exclusionMatchType; }  }

		public override void ApplyGuarantee(Scheme scheme, Random rng)
		{
			if (MinimumWeaponCount != null)
				_minimumWeaponCount = MinimumWeaponCount.GenerateByte(rng);

			if (MaximumWeaponCount != null)
				_maximumWeaponCount = MaximumWeaponCount.GenerateByte(rng);

			if (MinimumSettingValue != null)
				_minimumSettingValue = MinimumSettingValue.GenerateByte(rng);

			if (MaximumSettingValue != null)
				_maximumSettingValue = MaximumSettingValue.GenerateByte(rng);

			if ((!_minimumWeaponCount.HasValue && !_maximumWeaponCount.HasValue) ||
				(!_minimumSettingValue.HasValue && !_maximumSettingValue.HasValue))
				return;

			List<Weapon> weaponsMatchingGuarantee = new List<Weapon>();
			List<Weapon> weaponsNotMatchingGuarantee = new List<Weapon>();
			foreach (Weapon weapon in scheme.Weapons)
			{
				if (CategoryInclusionFilter.HasValue && !SchemeTypes.HasWeaponCategoryFlags(weapon.WeaponType, CategoryInclusionFilter.Value, InclusionMatchType))
					continue;

				if (CategoryExclusionFilter.HasValue && SchemeTypes.HasWeaponCategoryFlags(weapon.WeaponType, CategoryExclusionFilter.Value, ExclusionMatchType))
					continue;

				Setting setting = weapon.Access(WeaponSetting);
				if ((!_minimumSettingValue.HasValue || setting.Value >= _minimumSettingValue.Value) &&
					(!_maximumSettingValue.HasValue || setting.Value <= _maximumSettingValue.Value))
				{
					weaponsMatchingGuarantee.Add(weapon);
				}
				else
				{
					weaponsNotMatchingGuarantee.Add(weapon);
				}
			}

			//In general, prefer changing weapons whose value generators could have generated the desired value.
			if (_minimumWeaponCount.HasValue && weaponsMatchingGuarantee.Count < _minimumWeaponCount.Value)
			{
				HandleNotEnoughWeaponsMatchingGuarantee(weaponsNotMatchingGuarantee, _minimumWeaponCount.Value - weaponsMatchingGuarantee.Count, rng);
			}
			else if (_maximumWeaponCount.HasValue && weaponsMatchingGuarantee.Count > _maximumWeaponCount.Value)
			{
				int weaponCountExcess = weaponsMatchingGuarantee.Count - _maximumWeaponCount.Value;
				if (!MaximumWeaponCountForcedValue.HasValue)
				{
					HandleTooManyWeaponsMeetingGuarantee(weaponsMatchingGuarantee, weaponCountExcess, rng);
				}
				else
				{
					HandleTooManyWeaponsMeetingGuaranteeWithForcedValue(weaponsMatchingGuarantee, weaponCountExcess, rng);
				}
			}
		}

		void HandleNotEnoughWeaponsMatchingGuarantee(List<Weapon> weaponsNotMatchingGuarantee, int weaponCountDeficit, Random rng)
		{
			List<Weapon> weaponsWithinRange = null;
			List<Weapon> weaponsNotWithinRange = null;
			FilterByValueRangeOverlap(weaponsNotMatchingGuarantee, _minimumSettingValue, _maximumSettingValue, out weaponsWithinRange, out weaponsNotWithinRange);

			Shuffle(weaponsWithinRange, rng);
			Shuffle(weaponsNotWithinRange, rng);

			List<Weapon> weaponsToAlter = weaponsWithinRange;
			weaponsToAlter.AddRange(weaponsNotWithinRange);

			int numberOfWeaponsToAlter = Math.Min(weaponCountDeficit, weaponsToAlter.Count);
			for (int i = 0; i < numberOfWeaponsToAlter; ++i)
			{
				Setting settingToAlter = weaponsToAlter[i].Access(WeaponSetting);
				settingToAlter.ValueGenerator.GuaranteeValueRange(_minimumSettingValue, _maximumSettingValue);
				byte newValue = settingToAlter.ValueGenerator.GenerateByte(rng);
				settingToAlter.SetValue(newValue, settingToAlter.ValueGenerator);
			}
		}

		void HandleTooManyWeaponsMeetingGuarantee(List<Weapon> weaponsMatchingGuarantee, int weaponCountExcess, Random rng)
		{
			List<Weapon> weaponsBelowRange = null;
			byte? belowMinimumSettingValue = null;
			if (_minimumSettingValue.HasValue)
			{
				int minimumValue = (int)_minimumSettingValue.Value;
				belowMinimumSettingValue = (byte)(minimumValue > 0 ? minimumValue - 1 : 0);

				List<Weapon> dummy;
				FilterByValueRangeOverlap(weaponsMatchingGuarantee, 0, belowMinimumSettingValue, out weaponsBelowRange, out dummy);
			}

			List<Weapon> weaponsAboveRange = null;
			byte? aboveMaximumSettingValue = null;
			if (_maximumSettingValue.HasValue)
			{
				int maximumValue = (int)_maximumSettingValue.Value;
				aboveMaximumSettingValue = (byte)(maximumValue < 255 ? maximumValue + 1 : 255);

				List<Weapon> dummy;
				FilterByValueRangeOverlap(weaponsMatchingGuarantee, aboveMaximumSettingValue, 255, out weaponsAboveRange, out dummy);
			}

			//Ensure each weapon only appears in one list of the lists.
			if (weaponsBelowRange != null && weaponsAboveRange != null)
			{
				Debug.Assert(_minimumSettingValue.HasValue && _maximumSettingValue.HasValue);

				foreach (Weapon weapon in weaponsMatchingGuarantee)
				{
					if (!weaponsBelowRange.Contains(weapon) || !weaponsAboveRange.Contains(weapon))
						continue;

					if (IsCurrentSettingCloserToMinimumValue(weapon))
					{
						weaponsAboveRange.Remove(weapon);
					}
					else
					{
						weaponsBelowRange.Remove(weapon);
					}
				}
			}

			//Combine the separate lists into one, remembering whether to generate their new value below or above the given range.
			List<Tuple<Weapon, int>> allWeapons = new List<Tuple<Weapon, int>>();

			if (weaponsBelowRange != null)
			{
				foreach (Weapon weapon in weaponsBelowRange)
				{
					allWeapons.Add(new Tuple<Weapon, int>(weapon, -1));
				}
			}

			if (weaponsAboveRange != null)
			{
				foreach (Weapon weapon in weaponsAboveRange)
				{
					allWeapons.Add(new Tuple<Weapon, int>(weapon, 1));
				}
			}

			Shuffle(allWeapons, rng);

			//Append weapons whose only generateable values lie within the guarantee's range.
			{
				List<Weapon> weaponsOutsideRange = new List<Weapon>();
				if (weaponsBelowRange != null)
					weaponsOutsideRange.AddRange(weaponsBelowRange);

				if (weaponsAboveRange != null)
					weaponsOutsideRange.AddRange(weaponsAboveRange);

				List<Weapon> weaponsNotOutsideRange = weaponsMatchingGuarantee.Except(weaponsOutsideRange).ToList();
				Shuffle(weaponsNotOutsideRange, rng);

				foreach (Weapon weapon in weaponsNotOutsideRange)
				{
					int newSortingValue = 0;
					if (_minimumSettingValue.HasValue && _maximumSettingValue.HasValue)
					{
						newSortingValue = IsCurrentSettingCloserToMinimumValue(weapon) ? -1 : 1;
					}
					else
					{
						newSortingValue = _minimumSettingValue.HasValue ? -1 : 1;
					}

					allWeapons.Add(new Tuple<Weapon, int>(weapon, newSortingValue));
				}
			}

			//Finally generate new values for the weapons.
			int numberOfWeaponsToAlter = Math.Min(weaponCountExcess, allWeapons.Count);
			for (int i = 0; i < numberOfWeaponsToAlter; ++i)
			{
				Tuple<Weapon, int> weaponSortingTuple = allWeapons[i];

				Setting settingToAlter = weaponSortingTuple.Item1.Access(WeaponSetting);
				if (weaponSortingTuple.Item2 == -1)
				{
					Debug.Assert(belowMinimumSettingValue.HasValue);
					settingToAlter.ValueGenerator.GuaranteeValueRange(0, belowMinimumSettingValue);
				}
				else
				{
					Debug.Assert(aboveMaximumSettingValue.HasValue);
					settingToAlter.ValueGenerator.GuaranteeValueRange(aboveMaximumSettingValue, 255);
				}

				byte newValue = settingToAlter.ValueGenerator.GenerateByte(rng);
				settingToAlter.SetValue(newValue, settingToAlter.ValueGenerator);
			}
		}

		void HandleTooManyWeaponsMeetingGuaranteeWithForcedValue(List<Weapon> weaponsMatchingGuarantee, int weaponCountExcess, Random rng)
		{
			List<Weapon> weaponsWithinRange = null;
			List<Weapon> weaponsNotWithinRange = null;
			FilterByValueRangeOverlap(weaponsMatchingGuarantee, _minimumSettingValue, _maximumSettingValue, out weaponsWithinRange, out weaponsNotWithinRange);

			Shuffle(weaponsWithinRange, rng);
			Shuffle(weaponsNotWithinRange, rng);

			List<Weapon> weaponsToAlter = weaponsWithinRange;
			weaponsToAlter.AddRange(weaponsNotWithinRange);

			int numberOfWeaponsToAlter = Math.Min(weaponCountExcess, weaponsToAlter.Count);
			for (int i = 0; i < numberOfWeaponsToAlter; ++i)
			{
				Setting settingToAlter = weaponsToAlter[i].Access(WeaponSetting);

				//Nullify the value generator references so upcoming guarantees cannot undo the forced value.
				settingToAlter.SetValue(MaximumWeaponCountForcedValue.Value);
			}
		}

		void FilterByValueRangeOverlap(List<Weapon> allWeapons, byte? min, byte? max, out List<Weapon> weaponsWithinRange, out List<Weapon> weaponsNotWithinRange)
		{
			weaponsWithinRange = new List<Weapon>();
			weaponsNotWithinRange = new List<Weapon>();

			foreach (Weapon weapon in allWeapons)
			{
				ValueGenerator valueGenerator = weapon.Access(WeaponSetting).ValueGenerator;
				if (valueGenerator == null)
					continue;

				if (valueGenerator.DoesValueRangeOverlap(min, max))
				{
					weaponsWithinRange.Add(weapon);
				}
				else
				{
					weaponsNotWithinRange.Add(weapon);
				}
			}
		}

		void Shuffle<T>(List<T> list, Random rng)
		{
			for (int i = list.Count - 1; i >= 1; --i)
			{
				int randomIndex = rng.Next(i);
				T temp = list[i];
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		bool IsCurrentSettingCloserToMinimumValue(Weapon weapon)
		{
			Setting setting = weapon.Access(WeaponSetting);
			int differenceBetweenMin = (int)setting.Value - (int)_minimumSettingValue.Value;
			int differenceBetweenMax = (int)_maximumSettingValue.Value - (int)setting.Value;

			Debug.Assert(differenceBetweenMin >= 0 && differenceBetweenMax >= 0);
			return differenceBetweenMax > differenceBetweenMin;
		}

		public ref WeaponCategoryFlags? GetCategoryInclusionFilterRef() { return ref _categoryInclusionFilter; }
		public ref WeaponCategoryMatchType GetInclusionMatchTypeRef() { return ref _inclusionMatchType; }
		public ref WeaponCategoryFlags? GetCategoryExclusionFilterRef() { return ref _categoryExclusionFilter; }
		public ref WeaponCategoryMatchType GetExclusionMatchTypeRef() { return ref _exclusionMatchType; }

		WeaponCategoryFlags? _categoryInclusionFilter;
		WeaponCategoryMatchType _inclusionMatchType;
		WeaponCategoryFlags? _categoryExclusionFilter;
		WeaponCategoryMatchType _exclusionMatchType;

		byte? _minimumWeaponCount;
		byte? _maximumWeaponCount;
		byte? _minimumSettingValue;
		byte? _maximumSettingValue;
	}
}
