using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2.Randomisation.Guarantees
{
	abstract class Guarantee
	{
		public Guarantee()
		{
		}

		public abstract void ApplyGuarantee(Scheme scheme, Random rng);
	}

	enum GuaranteeTypes
	{
		WeaponSetting,
		GodWormsIndyTerrainExclusivity,
		WeaponsPerFunctionKey
	}
}
