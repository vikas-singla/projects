using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Rainforest.BridgesAndLadders.Data
{
    public class UserDataAccess
    {
        public void AddGroup(string groupName)
        {
            Roles.CreateRole(groupName);
        }

        public MembershipUser AddUser(string login, string password)
        {
            // create a new user
            MembershipUser user = Membership.CreateUser(login, password);

            return user;
        }

        //public void ChangePassword(MembershipUser user
    }
}
