using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Scheme testing
            Scheme testScheme = new Scheme(true);

            XmlParser.XmlErrorCollection xmlErrorCollection = null;

            //XML testing
            try
            {
                XmlParser.XmlParser schemeXmlParse = new XmlParser.XmlParser("testXml.xml", testScheme);
                schemeXmlParse.Parse(out xmlErrorCollection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (xmlErrorCollection != null)
            {
                if (xmlErrorCollection.Errors.Count > 0)
                {
                    Console.WriteLine("Found {0} XML error(s)! :)", xmlErrorCollection.Errors.Count);

                    foreach (XmlParser.XmlError xmlError in xmlErrorCollection.Errors)
                    {
                        if (xmlError.HasLineNumber())
                        {
                            Console.WriteLine(" - Line {0}: {1}", xmlError.lineNumber, xmlError.errorString);
                        }
                        else
                        {
                            Console.WriteLine(" - Unknown line: {0}", xmlError.errorString);
                        }
                    }
                }
            }

            byte[] schemeBytes = testScheme.GetBytes();

            try
            {
                FileStream fs = new FileStream("testScheme.wsc", FileMode.Create, FileAccess.Write);

                fs.Write(schemeBytes, 0, schemeBytes.Length);

                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
