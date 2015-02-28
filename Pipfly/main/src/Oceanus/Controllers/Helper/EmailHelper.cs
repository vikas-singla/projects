using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmazonSES;
using System.Web.Mvc;
using System.IO;

namespace Oceanus.Controllers
{
    public class EmailHelper
    {
        internal static string RenderViewToString(string viewName, string masterName, object model, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controllerContext.RouteData.GetRequiredString("action");

            viewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, masterName);
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        internal static string RenderViewToString(string viewName, string masterName, string key, object model, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controllerContext.RouteData.GetRequiredString("action");

            viewData.Model = model;
            viewBag.key = key;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, masterName);
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        internal static bool sendEmail(string senderFullName, string recipientEmail, string subject, string htmlBody, string textBody)
        {
            bool emailSuccessful = true;

            string senderEmail = (senderFullName != null && !senderFullName.Trim().Equals(string.Empty)) ? (senderFullName + " via Pipfly<noreply@pipfly.com>") : "Pipfly<noreply@pipfly.com>";

            try
            {
                //send email
                AmazonSESWrapper emailWrapper = new AmazonSESWrapper();
                AmazonSentEmailResult emailSendResult = emailWrapper.SendEmail(recipientEmail, senderEmail, senderEmail,
                    subject, htmlBody, textBody);

                if (!emailSendResult.HasError)
                {
                    emailSuccessful = true;
                }
            }
            catch (Exception e)
            {
                emailSuccessful = false;
            }

            return emailSuccessful;
        }

        internal static string GetMessageEchoEmailHtmlBody(bool echoSenderNameAndSubject, string messageFrom, string messageSubject,
            string messageBody, int messageId, ControllerContext controllerContext, dynamic viewBag, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            string result = RenderEmailEchoMessageToString(echoSenderNameAndSubject, "MessageEchoToEmail", "_EmailLayout", messageFrom, messageSubject, messageBody, messageId, controllerContext, viewBag,
                viewData, tempData);

            return result;
        }

        private static string RenderEmailEchoMessageToString(bool echoSenderNameAndSubject, string viewName, string masterName, string messageFrom, string messageSubject,
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
    }
}