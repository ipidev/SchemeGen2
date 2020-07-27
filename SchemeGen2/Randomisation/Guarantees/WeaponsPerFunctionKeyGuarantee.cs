using SchemeGen2.Randomisation.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.Guarantees
{
	class WeaponsPerFunctionKeyGuarantee : Guarantee
	{
		public WeaponsPerFunctionKeyGuarantee(ValueGenerator valueGenerator)
		{
			_valueGenerator = valueGenerator;
		}

		public override void ApplyGuarantee(Scheme scheme, Random rng)
		{
			if (_valueGenerator == null)
				return;

			int weaponCount = _valueGenerator.GenerateByte(rng);

			for (int i = 0; i < (int)WeaponFunctionKeys.Count; ++i)
			{
				List<Weapon> availableWeapons = new List<Weapon>();
				foreach (WeaponTypes weaponType in SchemeTypes.GetWeaponsForFunctionKey((WeaponFunctionKeys)i))
				{
					Weapon weapon = scheme.Access(weaponType);
					if (weapon.CanAppearAtAll())
					{
						availableWeapons.Add(weapon);
					}
				}

				if (availableWeapons.Count > weaponCount)
				{
					for (int j = availableWeapons.Count - 1; j >= 1; --j)
					{
						int randomIndex = rng.Next(j);
						Weapon temp = availableWeapons[j];
						availableWeapons[j] = availableWeapons[randomIndex];
						availableWeapons[randomIndex] = temp;
					}

					for (int j = weaponCount; j < availableWeapons.Count; ++j)
					{
						Weapon weapon = availableWeapons[j];
						weapon.Ammo.SetValue(0);

						if (SchemeTypes.CanApplyWeaponSetting(weapon.WeaponType, WeaponSettings.Crate))
						{
							weapon.Crate.SetValue(0);
						}
					}
				}
			}
		}

		ValueGenerator _valueGenerator;
	}
}
