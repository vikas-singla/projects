using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.ViewModels
{
    public class NetworkConnectionPathViewModel
    {
        /// <summary>
        /// Source friend user ID
        /// </summary>
        public Guid FromUID { get; set; }
        /// <summary>
        /// Reference to the 'from' user profile
        /// </summary>
        public UserProfileViewModel FromUserProfile { get; set; }
        /// <summary>
        /// Target friend user ID
        /// </summary>
        public Guid TargetUID { get; set; }
        /// <summary>
        /// Reference to the target friend's user profile
        /// </summary>
        public UserProfileViewModel TargetUserProfile { get; set; }
        /// <summary>
        /// User ID of the friend that connects the two people (FromUID, TOUID) through his network
        /// </summary>
        public Guid? ConnectingFriendID { get; set; }
        /// <summary>
        /// Reference to the connecting friend's user profile
        /// </summary>
        public UserProfileViewModel ConnectingFriendUserProfile { get; set; }
        /// <summary>
        /// Name of the friend that connects the two people (FromUID, TOUID) through his network
        /// </summary>
        public string ConnectingFriendName { get; set; }
        /// <summary>
        /// Distance between the friends (i.e. degrees of seperation)
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// Network path (showing how friends are connected to each other)
        /// </summary>
        public string NetworkPath { get; set; }

        public NetworkConnectionPathViewModel(Guid fromUID_, Guid targetUID_, UserProfileViewModel targetUserProfile_, int distance_, string networkPath_)
        {
            FromUID = fromUID_;
            TargetUID = targetUID_;
            Distance = distance_;
            NetworkPath = networkPath_;
            TargetUserProfile = targetUserProfile_;

            if (distance_ == 2)
            { //2 degrees of seperation
                string strConnectingFriendID = (networkPath_.Contains('.') ? networkPath_.Split('.')[1] : null);
                Guid trialGuid = new Guid();
                bool guidParseResult = Guid.TryParse(strConnectingFriendID, out trialGuid);

                if(guidParseResult)
                {
                    ConnectingFriendID = trialGuid;
                }
            }
        }
    }
}