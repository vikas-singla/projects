﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Profile;
using Oceanus.ViewModels;
using Oceanus.Data;
using System.Linq;
using Oceanus.Controllers;

namespace Oceanus.Models
{

    #region Models

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    
    public class LogOnModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


    public class RegisterModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [EmailValidation(ErrorMessage = "Not a valid email address")]
        [Required]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string email, string password);
        MembershipCreateStatus CreateUser(string firstName, string lastName, string password, string email);
        MembershipCreateStatus CreateFacebookUser(string firstName, string lastName, long fbuid, string password, string email, string token, string userPhotoUrl);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        private OceanusEntities Database = new OceanusEntities();

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string email, string password)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(email, password);
        }

        public MembershipCreateStatus CreateUser(string firstName, string lastName, string password, string email)
        {
            if (String.IsNullOrEmpty(firstName)) throw new ArgumentException("Value cannot be null or empty.", "firstName");
            if (String.IsNullOrEmpty(lastName)) throw new ArgumentException("Value cannot be null or empty.", "lastName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            var userLevel = Database.UserLevels.Where(l => l.LevelName.ToLower().Trim().Equals("newbie")).FirstOrDefault();
            int userLevelId = 1;

            if (userLevel != null)
            {
                userLevelId = userLevel.UserLevelId;
            }
            else
            {
                throw new Exception("Unable to create user");
            }

            MembershipCreateStatus status;
            MembershipUser user = _provider.CreateUser(email, password, email, null, null, true, null, out status);

            //create user profile
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel(null, Guid.Parse(user.ProviderUserKey.ToString()), firstName, lastName, userLevelId, null, null, false, null, false, true);
            UserProfile profile = userProfileViewModel.ToUserProfile();            

            Database.UserProfiles.AddObject(profile);
            Database.SaveChanges();

            UserHelper.AddUserProfileFullName(profile.FirstName.Length > 13 ? (profile.FirstName.Substring(0, 13) + "...") : profile.FirstName);
            UserHelper.AddUserProfileImageUrlCookie(null);

            return status;
        }

        public MembershipCreateStatus CreateFacebookUser(string firstName, string lastName, long fbuid, string password, string email, string token, string userPhotoUrl)
        {
            if (String.IsNullOrEmpty(firstName)) throw new ArgumentException("Value cannot be null or empty.", "firstName");
            if (String.IsNullOrEmpty(lastName)) throw new ArgumentException("Value cannot be null or empty.", "lastName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            var userLevel = Database.UserLevels.Where(l => l.LevelName.ToLower().Trim().Equals("newbie")).FirstOrDefault();
            int userLevelId = 1;

            if (userLevel != null)
            {
                userLevelId = userLevel.UserLevelId;
            }
            else
            {
                throw new Exception("Unable to create user");
            }

            MembershipCreateStatus status;
            MembershipUser user = _provider.CreateUser(fbuid.ToString(), password, email, null, null, true, null, out status);

            //create user profile
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel(null, Guid.Parse(user.ProviderUserKey.ToString()), firstName, lastName, userLevelId, token, fbuid, true, userPhotoUrl, true, true);
            UserProfile profile = userProfileViewModel.ToUserProfile();

            Database.UserProfiles.AddObject(profile);
            Database.SaveChanges();

            UserHelper.AddUserProfileFullName(profile.FirstName.Length > 13 ? (profile.FirstName.Substring(0, 13) + "...") : profile.FirstName);
            UserHelper.AddUserProfileImageUrlCookie(null);

            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]{
                new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), _minCharacters, int.MaxValue)
            };
        }
    }
    #endregion

}
