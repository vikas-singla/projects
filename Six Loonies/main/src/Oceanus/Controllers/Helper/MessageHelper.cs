using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Oceanus.ViewModels;
using Oceanus.Data;
using System.Text.RegularExpressions;
using AmazonSES;
using System.Web.Mvc;
using System.IO;
using Oceanus.Models;

namespace Oceanus.Controllers
{
    public class MessageHelper
    {
        #region Helpers

        internal static void CreateMessage(string messageTo, string messageSubject, string messageBody, System.Security.Principal.IPrincipal User, out string dbMessage, out bool successful,
            OceanusEntities database, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            dbMessage = string.Empty;
            successful = false;

            JavaScriptSerializer s = new JavaScriptSerializer();
            List<Oceanus.Controllers.MessageController.MessageToJson> messageToResult = s.Deserialize<List<Oceanus.Controllers.MessageController.MessageToJson>>(messageTo);

            Regex emailRegEx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    //create parent message
                    BaseMessageViewModel messageViewModel = new BaseMessageViewModel(null, messageSubject, DateTime.Now, DateTime.Now, false, userGuid);
                    Message messageModel = messageViewModel.ToMessageModel();

                    database.Messages.AddObject(messageModel);
                    database.SaveChanges();

                    //create response associated with message (i.e. initial user response)
                    MessageResponsesViewModel responseViewModel = new MessageResponsesViewModel(null, messageModel.MessageId, messageBody, DateTime.Now, userGuid);
                    MessageResponses responseModel = responseViewModel.ToMessageResponsesModel();

                    database.MessageResponses.AddObject(responseModel);
                    database.SaveChanges();

                    //add message visibility

                    //add message messibility for initiating user
                    MessageVisibilityViewModel initiatingUserVisibilityViewModel = new MessageVisibilityViewModel(null, messageModel.MessageId, userGuid, DateTime.Now, false);
                    MessageVisibility initiatingUserVisibilityModel = initiatingUserVisibilityViewModel.ToMessageVisibilityModel();

                    database.MessageVisibilities.AddObject(initiatingUserVisibilityModel);

                    //add message visibility for all other participants
                    foreach (Oceanus.Controllers.MessageController.MessageToJson recipient in messageToResult)
                    {
                        if (!emailRegEx.IsMatch(recipient.UserId))
                        {
                            Guid recipientGuid = Guid.Parse(recipient.UserId);

                            MessageVisibilityViewModel visibilityViewModel = new MessageVisibilityViewModel(null, messageModel.MessageId, recipientGuid, DateTime.Now, true);
                            MessageVisibility visibilityModel = visibilityViewModel.ToMessageVisibilityModel();

                            database.MessageVisibilities.AddObject(visibilityModel);
                        }
                    }

                    database.SaveChanges();

                    //send email echo
                    SendMessageEchoToEmail(userGuid, messageToResult, messageSubject, messageBody, messageModel.MessageId, database, controllerContext, viewBag, viewData, tempData);

                    successful = true;
                    dbMessage = "Message created successfully.";
                }
            }
            catch (Exception e)
            {
                successful = false;
                dbMessage = "Message could not be created.";
                throw e;
            }
        }

        protected static List<string> GetRecipientEmails(List<Oceanus.Controllers.MessageController.MessageToJson> messageTo, OceanusEntities database)
        {
            List<string> recipientEmails = new List<string>();
            List<Guid> recipientGuids = new List<Guid>();
            Regex emailRegEx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

            foreach (Oceanus.Controllers.MessageController.MessageToJson recipient in messageTo)
            {
                if (!emailRegEx.IsMatch(recipient.UserId))
                {
                    Guid recipientGuid = Guid.Parse(recipient.UserId);
                    recipientGuids.Add(recipientGuid);
                }
            }

            var userProfiles = database.aspnet_Membership.Where(u => recipientGuids.Contains(u.UserId)).AsQueryable();
            recipientEmails = new List<string>(userProfiles.Select(u => u.LoweredEmail).AsEnumerable());

            return recipientEmails;
        }

        protected static void SendMessageEchoToEmail(Guid senderId, List<Oceanus.Controllers.MessageController.MessageToJson> messageTo, string messageSubject, string messageBody,
            int messageId, OceanusEntities database, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            UserProfile senderUserProfile = database.UserProfiles.Where(u => u.UserId == senderId).FirstOrDefault();

            if (senderUserProfile != null)
            {
                List<string> recipientEmails = GetRecipientEmails(messageTo, database);
                string senderFullName = string.Empty;
                string textEmail = string.Empty;
                bool echoSubjectAndSenderName = !(Roles.IsUserInRole(senderUserProfile.aspnet_Users.UserName, SixLooniesMembership.UserRoles.Moderator.ToString()) || Roles.IsUserInRole(senderUserProfile.aspnet_Users.UserName, SixLooniesMembership.UserRoles.SuperAdmin.ToString()));

                if (echoSubjectAndSenderName)
                {
                    senderFullName = senderUserProfile.FirstName + " " + senderUserProfile.LastName;
                    textEmail = senderFullName + " sent you a message on www.sixloonies.com\nSubject: " + messageSubject + "\n\n" + messageBody + "\n\n" +
                        "To reply to this message, visit www.sixloonies.com/message/" + messageId + "\n\n-----------------------" +
                        "\n(c) Six Loonies Inc.";
                }
                else
                {
                    textEmail = messageBody + "\n\n" +
                        "To reply to this message, visit www.sixloonies.com/message/" + messageId + "\n\n-----------------------" +
                        "\n(c) Six Loonies Inc.";
                }

                if (recipientEmails.Count() > 50)
                {
                    while(recipientEmails.Count > 50)
                    {
                        List<string> subRecipientList = new List<string>(recipientEmails.Take(50));
                        recipientEmails.RemoveRange(0, 50);

                        //send email
                        AmazonSESWrapper emailWrapper = new AmazonSESWrapper();
                        AmazonSentEmailResult emailSendResult = emailWrapper.SendEmail(subRecipientList, new List<string>(), new List<string>(), "noreply@sixloonies.com", "noreply@sixloonies.com",
                            messageSubject, GetMessageEchoEmailHtmlBody(echoSubjectAndSenderName, senderFullName, messageSubject, messageBody, messageId,
                            controllerContext, viewBag, viewData, tempData), textEmail);

                        if (!emailSendResult.HasError)
                        {
                            // TO DO: Add logging
                        }
                        else
                        {
                            // TO DO: Add logging
                        }
                    }
                }
                else
                {
                    //send email
                    AmazonSESWrapper emailWrapper = new AmazonSESWrapper();
                    AmazonSentEmailResult emailSendResult = emailWrapper.SendEmail(recipientEmails, new List<string>(), new List<string>(), "noreply@sixloonies.com", "noreply@sixloonies.com",
                        messageSubject, GetMessageEchoEmailHtmlBody(echoSubjectAndSenderName, senderFullName, messageSubject, messageBody,
                        messageId, controllerContext, viewBag, viewData, tempData), textEmail);

                    if (!emailSendResult.HasError)
                    {
                        // TO DO: Add logging
                    }
                    else
                    {
                        // TO DO: Add logging
                    }
                }
            }
            else
            {
                // TO DO: Add logging
            }
        }

        protected static string GetMessageEchoEmailHtmlBody(bool echoSenderNameAndSubject, string messageFrom, string messageSubject,
            string messageBody, int messageId, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            string result = RenderEmailEchoMessageToString(echoSenderNameAndSubject, "MessageEchoToEmail", "_EmailLayout", messageFrom, messageSubject, messageBody, messageId, controllerContext, viewBag,
                viewData, tempData);

            return result;
        }

        protected static string RenderEmailEchoMessageToString(bool echoSenderNameAndSubject, string viewName, string masterName, string messageFrom, string messageSubject, 
            string messageBody, int messageId, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controllerContext.RouteData.GetRequiredString("action");

            viewData.Model = null;
            viewData.Add(new KeyValuePair<string, object>("echoSenderNameAndSubject", echoSenderNameAndSubject));
            viewData.Add(new KeyValuePair<string, object>("senderName", messageFrom));
            viewData.Add(new KeyValuePair<string, object>("subject", messageSubject));
            viewData.Add(new KeyValuePair<string, object>("messageBody", messageBody));
            viewData.Add(new KeyValuePair<string, object>("messageId", messageId));

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, masterName);
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        internal static void CreateMessageWithUserID(string messageTo, string messageSubject, string messageBody, System.Security.Principal.IPrincipal User, out string dbMessage, out bool successful, OceanusEntities database)
        {
            dbMessage = string.Empty;
            successful = false;

            JavaScriptSerializer s = new JavaScriptSerializer();

            try
            {
                if (User.Identity != null && User.Identity.Name != null && Membership.GetUser(User.Identity.Name) != null)
                {
                    MembershipUser user = Membership.GetUser(User.Identity.Name);
                    Guid userGuid = (Guid)user.ProviderUserKey;

                    //create parent message
                    BaseMessageViewModel messageViewModel = new BaseMessageViewModel(null, messageSubject, DateTime.Now, DateTime.Now, false, userGuid);
                    Message messageModel = messageViewModel.ToMessageModel();

                    database.Messages.AddObject(messageModel);
                    database.SaveChanges();

                    //create response associated with message (i.e. initial user response)
                    MessageResponsesViewModel responseViewModel = new MessageResponsesViewModel(null, messageModel.MessageId, messageBody, DateTime.Now, userGuid);
                    MessageResponses responseModel = responseViewModel.ToMessageResponsesModel();

                    database.MessageResponses.AddObject(responseModel);
                    database.SaveChanges();

                    //add message visibility

                    //add message messibility for initiating user
                    MessageVisibilityViewModel initiatingUserVisibilityViewModel = new MessageVisibilityViewModel(null, messageModel.MessageId, userGuid, DateTime.Now, false);
                    MessageVisibility initiatingUserVisibilityModel = initiatingUserVisibilityViewModel.ToMessageVisibilityModel();

                    database.MessageVisibilities.AddObject(initiatingUserVisibilityModel);

                    //add message visibility for all other participants
                    Guid recipientGuid = Guid.Parse(messageTo);

                    MessageVisibilityViewModel visibilityViewModel = new MessageVisibilityViewModel(null, messageModel.MessageId, recipientGuid, DateTime.Now, true);
                    MessageVisibility visibilityModel = visibilityViewModel.ToMessageVisibilityModel();

                    database.MessageVisibilities.AddObject(visibilityModel);

                    database.SaveChanges();

                    successful = true;
                    dbMessage = "Message created successfully.";
                }
            }
            catch (Exception e)
            {
                successful = false;
                dbMessage = "Message could not be created.";
                throw e;
            }
        }

        #endregion
    }
}