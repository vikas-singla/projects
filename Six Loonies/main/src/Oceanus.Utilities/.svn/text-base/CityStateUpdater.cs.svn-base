using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oceanus.Utilities
{
    public static class CityStateUpdater
    {
        public static void UpdateStateName()
        {
            using (OceanusEntitiesLive entities = new OceanusEntitiesLive())
            {
                var regionData = entities.ReferenceRegions.ToDictionary(r => r.SubdivisionCode, r => r.Name);
                foreach (var city in entities.ReferenceCities)
                {
                    if (regionData.ContainsKey(city.Region))
                    {
                        city.Region = regionData[city.Region].Trim('\"');
                    }
                }

                entities.SaveChanges();
            }
        }
    }
}
