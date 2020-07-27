using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchemeGen2UI
{
	class InvalidMetaschemeFileException : Exception
	{
		public InvalidMetaschemeFileException(string message)
			: base(message)
		{
		}
	}

	class MetaschemeFileInfo
	{
		public MetaschemeFileInfo(string fullPath)
		{
			FullPath = fullPath;
			DisplayName = Path.GetFileNameWithoutExtension(fullPath);

			//Load and parse document.
			XDocument xDocument = XDocument.Load(fullPath, LoadOptions.SetLineInfo);
			if (xDocument == null)
			{
				throw new InvalidMetaschemeFileException("Could not parse XML file.");
			}
			
			if (xDocument.Root.Name != "Metascheme")
			{
				throw new InvalidMetaschemeFileException("XML file is not a metascheme.");
			}

			XElement informationElement = xDocument.Root.Element("Information");
			if (informationElement == null)
				return;

			foreach (XElement element in informationElement.Elements())
			{
				string decodedValue = System.Net.WebUtility.HtmlDecode(element.Value);
				switch (element.Name.LocalName)
				{
				case "DisplayName":
					DisplayName = decodedValue;
					break;

				case "AuthorName":
					AuthorName = decodedValue;
					break;

				case "Description":
					Description = decodedValue;
					break;
				}
			}
		}

		public string FullPath { get; private set; }
		public string DisplayName { get; private set; }
		public string AuthorName { get; private set; }
		public string Description { get; private set; }

		public override string ToString()
		{
			return DisplayName;
		}
	}
}
