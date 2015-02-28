using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ResumeAnaylzerApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string HighlightKeywords(string keywords, string text)
        {
            // Swap out the ,<space> for pipes and add the braces
            Regex r = new Regex(@", ?");
            keywords = "(" + r.Replace(keywords, @"|") + ")";

            // Get ready to replace the keywords
            r = new Regex(keywords, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Do the replace
            return r.Replace(text, new MatchEvaluator(MatchEval));
        }

        public static string MatchEval(Match match)
        {
            if (match.Groups[1].Success)
            {
                return "<b>" + match.ToString() + "</b>";

            }

            return ""; //no match
        }

    }
}
