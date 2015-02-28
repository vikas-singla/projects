using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using NUnit.Framework;
//using Selenium;
//using SeleniumClass;

namespace ResumeAnaylzerApplication
{
    public partial class Form1 : Form
    {

        //private StringBuilder verificationErrors;

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplicationClass wordApp = new ApplicationClass();

            string resumeFilePath = "C:\\Users\\VivekPandey\\Desktop\\Resumes\\Amit_Resume.doc";

            object file = resumeFilePath;

            object nullobj = System.Reflection.Missing.Value;

            // Heere on Document.Open, there should be 9 args
            Document doc = wordApp.Documents.Open(ref file,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj);

            Document resume = wordApp.ActiveDocument;

            string m_Content = resume.Content.Text;

            // Analyze the resume for certain key words and patterns

            string highlightedResume = Program.HighlightKeywords("c#", m_Content);

            resume.Close();
      }
  }
}
