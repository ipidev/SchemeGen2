using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchemeGen2.XmlParser
{
    class XmlParser
    {
        public XmlParser(string filename)
        {
            _filename = filename;

            _xDoc = XDocument.Load(filename, LoadOptions.SetLineInfo);
        }

        public void Parse()
        {
            if (_xDoc == null)
            {
                throw new ArgumentNullException("XML document is not valid.");
            }

            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = _xDoc.Elements();

            foreach (XElement element in elements)
            {
                //Handle scheme element.
                if (element.Name.LocalName == ElementTypes.Scheme.ToString() && !foundElements.Contains(ElementTypes.Scheme))
                {
                    foundElements.Add(ElementTypes.Scheme, element);
                    ParseSchemeElement(element);
                }
            }
        }

        void ParseSchemeElement(XElement schemeElement)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = schemeElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle settings element.
                if (element.Name.LocalName == ElementTypes.Settings.ToString() && !foundElements.Contains(ElementTypes.Settings))
                {
                    foundElements.Add(ElementTypes.Settings, element);
                    ParseSettingsElement(element);
                }
                //Handle weapons element.
                else if (element.Name.LocalName == ElementTypes.Weapons.ToString() && !foundElements.Contains(ElementTypes.Weapons))
                {
                    foundElements.Add(ElementTypes.Weapons, element);
                    ParseWeaponsElement(element);
                }
            }
        }

        void ParseSettingsElement(XElement settingsElement)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = settingsElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle setting element.
                if (element.Name.LocalName == ElementTypes.Setting.ToString())
                {
                    XAttribute attribute = element.Attributes().First(a => a.Name == "name");

                    if (attribute != null)
                    {
                        SettingTypes setting;

                        if (Enum.TryParse<SettingTypes>(attribute.Value, out setting))
                        {
                            if (!foundElements.Contains(setting))
                            {
                                foundElements.Add(setting, element);

                                //TODO: Handle setting.
                                Console.WriteLine(attribute.Value);
                            }
                        }
                    }
                }
            }
        }

        void ParseWeaponsElement(XElement settingsElement)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = settingsElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle setting element.
                if (element.Name.LocalName == ElementTypes.Weapon.ToString())
                {
                    XAttribute attribute = element.Attributes().First(a => a.Name == "name");

                    if (attribute != null)
                    {
                        WeaponTypes weapon;

                        if (Enum.TryParse<WeaponTypes>(attribute.Value, out weapon))
                        {
                            if (!foundElements.Contains(weapon))
                            {
                                foundElements.Add(weapon, element);

                                //TODO: Handle weapon.
                                Console.WriteLine(attribute.Value);
                            }
                        }
                    }
                }
            }
        }

        string _filename;
        XDocument _xDoc;
    }
}
