using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
	public static class SchemeGen2
	{
		public static bool Generate(string inputPath, string outputPath = null, int? seed = null, TextWriter errorTextWriter = null)
		{
			if (!File.Exists(inputPath))
			{
				if (errorTextWriter != null)
					errorTextWriter.WriteLine("File {0} does not exist.", inputPath);
				return false;
			}

			if (outputPath == null)
			{
				outputPath = Path.ChangeExtension(inputPath, "wsc");
			}

			Randomisation.SchemeGenerator schemeGenerator = null;
			XmlParser.XmlErrorCollection xmlErrorCollection = null;

			bool parseSucceeded = false;

			try
			{
				XmlParser.XmlParser schemeXmlParse = new XmlParser.XmlParser(inputPath);
				parseSucceeded = schemeXmlParse.Parse(out xmlErrorCollection, out schemeGenerator);
			}
			catch (Exception e)
			{
				if (errorTextWriter != null)
					errorTextWriter.WriteLine(String.Format("Error while parsing XML file: {0}\r\nStack:\r\n{1}", e.Message, e.StackTrace));
				return false;
			}

			if (xmlErrorCollection != null)
			{
				if (xmlErrorCollection.Errors.Count > 0)
				{
					if (errorTextWriter != null)
						errorTextWriter.WriteLine("Found {0} XML error(s):", xmlErrorCollection.Errors.Count);

					foreach (XmlParser.XmlError xmlError in xmlErrorCollection.Errors)
					{
						if (xmlError.HasLineNumber())
						{
							if (errorTextWriter != null)
								errorTextWriter.WriteLine(" - Line {0}: {1}", xmlError.lineNumber, xmlError.errorString);
						}
						else
						{
							if (errorTextWriter != null)
								errorTextWriter.WriteLine(" - Unknown line: {0}", xmlError.errorString);
						}
					}
				}
			}

			if (!parseSucceeded)
			{
				return false;
			}

			try
			{
				Random rng = seed.HasValue ? new Random(seed.Value) : new Random();
				Scheme testScheme = schemeGenerator.GenerateScheme(rng);
				
				using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
				{
					testScheme.Serialise(fs);
					fs.Close();
				}
			}
			catch (Exception e)
			{
				if (errorTextWriter != null)
					errorTextWriter.WriteLine(String.Format("Error while writing scheme file: {0}\r\nStack:\r\n{1}", e.Message, e.StackTrace));
				return false;
			}

			return true;
		}
	}
}
