using System.Linq;

namespace Rainforest.BridgesAndLadders.Data
{
    /// <summary>
    /// The data access layer for site content.
    /// http://msdn.microsoft.com/en-us/library/bb386870.aspx
    /// </summary>
    public class EntityDataAccess
    {
        /// <summary>
        /// Creates an instance of the data access class.
        /// </summary>
        public EntityDataAccess()
        {

        }

        /// <summary>
        /// Gets a company from the database.
        /// NOTE: need to set appropriate joins for company metadata.
        /// </summary>
        /// <param name="companyId">The id of the company</param>
        /// <returns>The company.</returns>
        public Company GetCompany(int companyId)
        {
            using (var db = new BridgesAndLaddersEntities())
            {
                Company c = db.Companies.Where(company => company.id == companyId).FirstOrDefault();
                return c;
            }
        }

        /// <summary>
        /// Adds a company to the database.
        /// </summary>
        /// <param name="company">The company to add.</param>
        public void AddCompany(Company company)
        {
            using (var db = new BridgesAndLaddersEntities())
            {
                // NOTE: should perform server side error checking
                db.Companies.AddObject(company);

                // NOTE: should look at return value to see the number of affected records.
                db.SaveChanges();
            }
        }
    }
}
