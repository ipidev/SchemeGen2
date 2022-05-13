using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.XmlParser
{
	enum ElementTypes
	{
		Metascheme,
		Information,
		Settings,
		Setting,
		Weapons,
		Weapon,
		Guarantees,
		Guarantee,
		MinimumWeaponCount,
		MaximumWeaponCount,
		MinimumSettingValue,
		MaximumSettingValue,
		InclusionFilter,
		ExclusionFilter,
		ExtendedOptions,

		Count
	}
}
