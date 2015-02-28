using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class UserStatsViewModel
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Number of clips the user has made
        /// </summary>
        public int NumClips { get; set; }
        /// <summary>
        /// Number of other users that are following this user
        /// </summary>
        public int NumFollowers { get; set; }
        /// <summary>
        /// Number of other users that the user is following
        /// </summary>
        public int NumFollowed { get; set; }
        /// <summary>
        /// Number of likes the user has received
        /// </summary>
        public int NumLikes { get; set; }
    }
}