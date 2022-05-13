using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2.Randomisation.Guarantees
{
	class GodWormsIndyTerrainExclusivityGuarantee : Guarantee
	{
		public GodWormsIndyTerrainExclusivityGuarantee()
		{
		}

		public override void ApplyGuarantee(Scheme scheme, Random rng)
		{
			Setting godWormsSetting = scheme.Access(SettingTypes.GodWorms);
			Setting indyTerrainSetting = scheme.Access(SettingTypes.IndestructibleTerrain);

			if (godWormsSetting.Value == 0 || indyTerrainSetting.Value == 0)
				return;

			//If one of the value generators could have produced a zero, force that.
			bool godWormsCouldBeFalse = godWormsSetting.ValueGenerator != null && godWormsSetting.ValueGenerator.DoesValueRangeOverlap(0, 0);
			bool indyTerrainCouldBeFalse = indyTerrainSetting.ValueGenerator != null && godWormsSetting.ValueGenerator.DoesValueRangeOverlap(0, 0);

			if (godWormsCouldBeFalse && !indyTerrainCouldBeFalse)
			{
				godWormsSetting.SetValue(0);
			}
			else if (!godWormsCouldBeFalse && indyTerrainCouldBeFalse)
			{
				indyTerrainSetting.SetValue(0);
			}
			//Select one at random to become zero.
			else
			{
				if (rng.NextDouble() < 0.5)
				{
					godWormsSetting.SetValue(0);
				}
				else
				{
					indyTerrainSetting.SetValue(0);
				}
			}
		}
	}
}
