using System;
using System.Linq;
using Oceanus.Data;

namespace Oceanus.Utilities
{
    public static class CityStateLinker
    {
        public static void LinkCityToReferenceTable()
        {
            using (OceanusEntitiesLive entities = new OceanusEntitiesLive())
            {
                
                foreach (var vcs in entities.VirtualCitiesServeds)
                {
                    var refCity =
                        entities.ReferenceCities.Where(c => 
                            c.City.Equals(vcs.City.ToLower()) && 
                            c.Country.Equals(vcs.Reference_LocationStates.Country) &&
                            c.Region.Equals(vcs.Reference_LocationStates.State))
                        .FirstOrDefault();
                    if (refCity != null)
                    {
                        vcs.ReferenceCityId = refCity.Id;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Could not map '{0}'", vcs.City));
                    }
                }

                entities.SaveChanges();
            }

            Console.WriteLine("Mapping finished, reference to VirtualCitiesServed for details");
        }
    }
}