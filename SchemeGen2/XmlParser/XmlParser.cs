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

        public bool Parse(out XmlErrorCollection errorCollection)
        {
            if (_xDoc == null)
            {
                throw new ArgumentNullException("XML document is not valid.");
            }

            //Error collecting object, handy for flooding the output with errors at the end :)
            errorCollection = new XmlErrorCollection();

            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = _xDoc.Elements();

            foreach (XElement element in elements)
            {
                //Handle scheme element.
                if (element.Name.LocalName == ElementTypes.Scheme.ToString())
                {
                    if (!foundElements.Contains(ElementTypes.Scheme))
                    {
                        foundElements.Add(ElementTypes.Scheme, element);
                        ParseSchemeElement(element, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element);
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }

            //If we didn't get the scheme element, we didn't do anything just now!
            if (!foundElements.Contains(ElementTypes.Scheme))
            {
                errorCollection.AddElementNotFound(ElementTypes.Scheme.ToString());
            }

            return errorCollection.Errors.Count == 0; 
        }

        void ParseSchemeElement(XElement schemeElement, ref XmlErrorCollection errorCollection)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = schemeElement.Elements();

            foreach (XElement element in elements)
            {
                //TODO: Can I generalise these calls?
                //Handle settings element.
                if (element.Name.LocalName == ElementTypes.Settings.ToString())
                {
                    if (!foundElements.Contains(ElementTypes.Settings))
                    {
                        foundElements.Add(ElementTypes.Settings, element);
                        ParseSettingsElement(element, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.Settings));
                    }
                }
                //Handle weapons element.
                else if (element.Name.LocalName == ElementTypes.Weapons.ToString())
                {
                    if (!foundElements.Contains(ElementTypes.Weapons))
                    {
                        foundElements.Add(ElementTypes.Weapons, element);
                        ParseWeaponsElement(element, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.Weapons));
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseSettingsElement(XElement settingsElement, ref XmlErrorCollection errorCollection)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = settingsElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle setting element.
                SettingTypes setting;

                if (Enum.TryParse<SettingTypes>(element.Name.LocalName, out setting))
                {
                    if (!foundElements.Contains(setting))
                    {
                        foundElements.Add(setting, element);

                        //TODO: Handle setting.
                        Console.WriteLine(element.Name.LocalName);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element, foundElements.Get(setting));
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseWeaponsElement(XElement settingsElement, ref XmlErrorCollection errorCollection)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = settingsElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle weapon element.
                WeaponTypes weapon;

                if (Enum.TryParse<WeaponTypes>(element.Name.LocalName, out weapon))
                {
                    if (!foundElements.Contains(weapon))
                    {
                        foundElements.Add(weapon, element);

                        //TODO: Handle weapon.
                        Console.WriteLine(element.Name.LocalName);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element, foundElements.Get(weapon));
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        string _filename;
        XDocument _xDoc;
    }
}
