using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace ResumeParserApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Word.ApplicationClass wordApp = new Word.ApplicationClass();

            string filePath = "C:\\Users\\VivekPandey\\Desktop\\Resumes\\Amit_Resume.docx";

            object file = filePath;

            object nullobj = System.Reflection.Missing.Value;

            // here on Document.Open there should be 9 arg.

            Word.Document doc = wordApp.Documents.Open(ref file,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj);

            Word.Document doc1 = wordApp.ActiveDocument;

            string m_Content = doc1.Content.Text;

            doc1.close();

        }
    }
}
