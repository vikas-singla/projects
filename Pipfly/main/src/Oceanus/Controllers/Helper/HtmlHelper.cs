using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Oceanus.Controllers
{
    public static class HtmlHelper
    {
        public static string formatArticleMarkupForEmail(string markup)
        {
            string result = markup;

            try
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(markup);

                if (document.DocumentNode.SelectNodes("//p") != null)
                {
                    foreach (HtmlNode paragraph in document.DocumentNode.SelectNodes("//p"))
                    {
                        if (paragraph.Attributes.Contains("style"))
                        {
                            paragraph.Attributes.Remove("style");
                        }

                        paragraph.Attributes.Add("style", "font-size:14px;line-height:20px;font-weight:normal;text-shadow:none;" +
                            "font-style:normal;font-variant:normal;margin:14px 0;word-wrap: break-word;" +
                            "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;");
                    }
                }

                if (document.DocumentNode.SelectNodes("//div") != null)
                {
                    foreach (HtmlNode div in document.DocumentNode.SelectNodes("//div"))
                    {
                        if (div.Attributes.Contains("style"))
                        {
                            div.Attributes.Remove("style");
                        }

                        div.Attributes.Add("style", "font-size:14px;line-height:20px;font-weight:normal;text-shadow:none;" +
                            "font-style:normal;font-variant:normal;margin:0;word-wrap: break-word;" +
                            "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;overflow:auto;");
                    }
                }

                if (document.DocumentNode.SelectNodes("//img") != null)
                {
                    foreach (HtmlNode image in document.DocumentNode.SelectNodes("//img"))
                    {
                        string prevValue = string.Empty;

                        if (image.Attributes.Contains("style"))
                        {
                            prevValue = image.Attributes["style"].Value;
                            image.Attributes["style"].Value = prevValue + ";min-height:auto;min-width:auto;max-height: 234px;max-width: 234px;";
                        }
                        else
                        {
                            image.Attributes.Add("style", prevValue + ";min-height:auto;min-width:auto;max-height: 234px;max-width: 234px;");
                        }
                    }
                }

                if (document.DocumentNode.SelectNodes("//a") != null)
                {
                    foreach (HtmlNode links in document.DocumentNode.SelectNodes("//a"))
                    {
                        if (links.Attributes.Contains("style"))
                        {
                            links.Attributes.Remove("style");
                        }

                        links.Attributes.Add("style", "text-decoration:none;");
                    }
                }

                if (document.DocumentNode.SelectNodes("//h2") != null)
                {
                    foreach (HtmlNode h2 in document.DocumentNode.SelectNodes("//h2"))
                    {
                        if (h2.Attributes.Contains("style"))
                        {
                            h2.Attributes.Remove("style");
                        }

                        h2.Attributes.Add("style", "text-align:left;font-size:18px;line-height:23px;padding-bottom:8px;margin:0;" +
                        "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;");
                    }
                }

                if (document.DocumentNode.SelectNodes("//h3") != null)
                {
                    foreach (HtmlNode h3 in document.DocumentNode.SelectNodes("//h3"))
                    {
                        if (h3.Attributes.Contains("style"))
                        {
                            h3.Attributes.Remove("style");
                        }

                        h3.Attributes.Add("style", "text-align:left;font-size:14px;line-height:16px;padding:0;margin:0;" +
                        "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;");
                    }
                }


                if (document.DocumentNode.SelectNodes("//h4") != null)
                {
                    foreach (HtmlNode h4 in document.DocumentNode.SelectNodes("//h4"))
                    {
                        if (h4.Attributes.Contains("style"))
                        {
                            h4.Attributes.Remove("style");
                        }

                        h4.Attributes.Add("style", "text-align:left;font-size:14px;line-height:16px;padding:0;margin:0;" +
                        "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;");
                    }
                }


                if (document.DocumentNode.SelectNodes("//h5") != null)
                {
                    foreach (HtmlNode h5 in document.DocumentNode.SelectNodes("//h5"))
                    {
                        if (h5.Attributes.Contains("style"))
                        {
                            h5.Attributes.Remove("style");
                        }

                        h5.Attributes.Add("style", "text-align:left;font-size:14px;line-height:16px;padding:0;margin:0;" +
                        "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;");
                    }
                }


                if (document.DocumentNode.SelectNodes("//h6") != null)
                {
                    foreach (HtmlNode h6 in document.DocumentNode.SelectNodes("//h6"))
                    {
                        if (h6.Attributes.Contains("style"))
                        {
                            h6.Attributes.Remove("style");
                        }

                        h6.Attributes.Add("style", "text-align:left;font-size:14px;line-height:16px;padding:0;margin:0;" +
                        "font-family: Helvetica, Arial, Verdana, sans-serif;color: #393939;");
                    }
                }

                result =  document.DocumentNode.OuterHtml;
            }
            catch (Exception e)
            {
            }

            return result;
        }
    }
}