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
        public XmlParser(string filename, Scheme scheme)
        {
            Debug.Assert(scheme != null);

            _filename = filename;
            _scheme = scheme;

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
                SettingTypes settingType;

                if (Enum.TryParse<SettingTypes>(element.Name.LocalName, out settingType))
                {
                    if (!foundElements.Contains(settingType))
                    {
                        foundElements.Add(settingType, element);

                        ParseSettingElement(element, settingType, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element, foundElements.Get(settingType));
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseSettingElement(XElement settingElement, SettingTypes settingType, ref XmlErrorCollection errorCollection)
        {
            //Iterate through all elements.
            IEnumerable<XElement> elements = settingElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle value element.
                ValueTypes value;

                if (Enum.TryParse<ValueTypes>(element.Name.LocalName, out value))
                {
                    XAttribute valueAttribute = element.Attribute("value");

                    if (valueAttribute != null)
                    {
                        Setting setting = _scheme[settingType];
                        Debug.Assert(setting != null);

                        SetValue(element, valueAttribute, setting, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddAttributeNotFound("value", element);
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseWeaponsElement(XElement weaponsElement, ref XmlErrorCollection errorCollection)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = weaponsElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle weapon element.
                WeaponTypes weapon;

                if (Enum.TryParse<WeaponTypes>(element.Name.LocalName, out weapon))
                {
                    if (!foundElements.Contains(weapon))
                    {
                        foundElements.Add(weapon, element);

                        ParseWeaponElement(element, weapon, ref errorCollection);
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

        void ParseWeaponElement(XElement weaponElement, WeaponTypes weaponType, ref XmlErrorCollection errorCollection)
        {
            FoundElements foundElements = new FoundElements();

            //Iterate through all elements.
            IEnumerable<XElement> elements = weaponElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle weapon setting element.
                WeaponSettings weaponSetting;

                if (Enum.TryParse<WeaponSettings>(element.Name.LocalName, out weaponSetting))
                {
                    if (!foundElements.Contains(weaponSetting))
                    {
                        foundElements.Add(weaponSetting, element);

                        ParseWeaponSettingElement(element, weaponType, weaponSetting, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddRepeatedElement(element, foundElements.Get(weaponSetting));
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseWeaponSettingElement(XElement weaponSettingElement, WeaponTypes weaponType, WeaponSettings weaponSetting, ref XmlErrorCollection errorCollection)
        {
            //Iterate through all elements.
            IEnumerable<XElement> elements = weaponSettingElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle value element.
                ValueTypes value;

                if (Enum.TryParse<ValueTypes>(element.Name.LocalName, out value))
                {
                    XAttribute valueAttribute = element.Attribute("value");

                    if (valueAttribute != null)
                    {
                        Weapon weapon = _scheme[weaponType];
                        Debug.Assert(weapon != null);

                        Setting setting = weapon[weaponSetting];
                        Debug.Assert(setting != null);

                        SetValue(element, valueAttribute, setting, ref errorCollection);
                    }
                    else
                    {
                        errorCollection.AddAttributeNotFound("value", element);
                    }
                }
                //Invalid element.
                else
                {
                    errorCollection.AddInvalidElement(element);
                }
            }
        }

        void SetValue(XElement element, XAttribute attribute, Setting setting, ref XmlErrorCollection errorCollection)
        {
            Debug.Assert(setting != null);

            //Check the attribute's value is of the right type and in range.
            int intValue;

            if (Int32.TryParse(attribute.Value, out intValue))
            {
                if (intValue >= (int)setting.MinimumValue && intValue <= (int)setting.MaximumValue)
                {
                    setting.Value = (byte)intValue;

                    Console.WriteLine("Set value: {0}", intValue);
                }
                else
                {
                    errorCollection.AddAttributeValueOutOfRange(element, attribute, setting);
                }
            }
            else
            {
                errorCollection.AddAttributeValueNonInteger(element, attribute);
            }
        }

        string _filename;
        Scheme _scheme;
        XDocument _xDoc;
    }
}
