using DocumentFormat.OpenXml.Packaging;
using System;
using System.IO;
using System.Text;
using System.Xml;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class FileReader
    {
        public static Form ReadForm(string text)
        {
            FormBuilder formBuilder = new FormBuilder();
            Form file = formBuilder.BuildForm(text);
            return file;
        }

        public static Form TestReader()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\user\source\repos\WebApplication2\testText.txt");
            return ReadForm(text);
        }

        public static Form TextFromWord(Stream file)
        {
            string result;
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(file, false))
            {
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                xmlNamespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                string wordprocessingDocumentText;
                using (StreamReader streamReader = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream()))
                {
                    wordprocessingDocumentText = streamReader.ReadToEnd();
                }

                StringBuilder stringBuilder = new StringBuilder(wordprocessingDocumentText.Length);

                XmlDocument xmlDocument = new XmlDocument(nameTable);
                xmlDocument.LoadXml(wordprocessingDocumentText);

                XmlNodeList paragraphNodes = xmlDocument.SelectNodes("//w:p", xmlNamespaceManager);
                foreach (XmlNode paragraphNode in paragraphNodes)
                {
                    XmlNodeList textNodes = paragraphNode.SelectNodes(".//w:t | .//w:tab | .//w:br", xmlNamespaceManager);
                    foreach (XmlNode textNode in textNodes)
                    {
                        switch (textNode.Name)
                        {
                            case "w:t":
                                stringBuilder.Append(textNode.InnerText);
                                break;

                            case "w:tab":
                                stringBuilder.Append("\t");
                                break;

                            case "w:br":
                                stringBuilder.Append("\v");
                                break;
                        }
                    }

                    stringBuilder.Append(Environment.NewLine);
                }
                result = stringBuilder.ToString();
            }
            return ReadForm(result);
        }
    }
}