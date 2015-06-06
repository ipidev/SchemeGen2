using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using SchemeGen2.Randomisation;

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

            _schemeGenerator = null;
            _errorCollection = null;
        }

        public bool Parse(out XmlErrorCollection errorCollection, out SchemeGenerator schemeGenerator)
        {
            if (_xDoc == null)
            {
                throw new ArgumentNullException("XML document is not valid.");
            }

            //Store a reference to a new scheme generator and error collector for ease of use later on.
            //This object is not responsible for their lifetime once Parse() has finished!
            _schemeGenerator = new SchemeGenerator();
            _errorCollection = new XmlErrorCollection();

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
                        ParseSchemeElement(element);
                    }
                    else
                    {
                        _errorCollection.AddRepeatedElement(element);
                    }
                }
                //Invalid element.
                else
                {
                    _errorCollection.AddInvalidElement(element);
                }
            }

            //If we didn't get the scheme element, we didn't do anything just now!
            if (!foundElements.Contains(ElementTypes.Scheme))
            {
                _errorCollection.AddElementNotFound(ElementTypes.Scheme.ToString());
            }

            //Set the out variables and lose our stored references to them.
            schemeGenerator = _schemeGenerator;
            _schemeGenerator = null;

            errorCollection = _errorCollection;
            _errorCollection = null;

            return errorCollection.Errors.Count == 0; 
        }

        void ParseSchemeElement(XElement schemeElement)
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
                        ParseSettingsElement(element);
                    }
                    else
                    {
                        _errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.Settings));
                    }
                }
                //Handle weapons element.
                else if (element.Name.LocalName == ElementTypes.Weapons.ToString())
                {
                    if (!foundElements.Contains(ElementTypes.Weapons))
                    {
                        foundElements.Add(ElementTypes.Weapons, element);
                        ParseWeaponsElement(element);
                    }
                    else
                    {
                        _errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.Weapons));
                    }
                }
                //Invalid element.
                else
                {
                    _errorCollection.AddInvalidElement(element);
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
                SettingTypes settingType;

                if (Enum.TryParse<SettingTypes>(element.Name.LocalName, out settingType))
                {
                    if (!foundElements.Contains(settingType))
                    {
                        foundElements.Add(settingType, element);

                        ParseSettingElement(element, settingType);
                    }
                    else
                    {
                        _errorCollection.AddRepeatedElement(element, foundElements.Get(settingType));
                    }
                }
                //Invalid element.
                else
                {
                    _errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseSettingElement(XElement settingElement, SettingTypes settingType)
        {
            //Create the value generator.
            ValueGenerator valueGenerator = CreateValueGenerator(settingElement);

            //Pass the new value generator to the scheme generator.
            Debug.Assert(_schemeGenerator != null);

            _schemeGenerator.Set(settingType, valueGenerator);
        }

        void ParseWeaponsElement(XElement weaponsElement)
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

                        ParseWeaponElement(element, weapon);
                    }
                    else
                    {
                        _errorCollection.AddRepeatedElement(element, foundElements.Get(weapon));
                    }
                }
                //Invalid element.
                else
                {
                    _errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseWeaponElement(XElement weaponElement, WeaponTypes weaponType)
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

                        ParseWeaponSettingElement(element, weaponType, weaponSetting);
                    }
                    else
                    {
                        _errorCollection.AddRepeatedElement(element, foundElements.Get(weaponSetting));
                    }
                }
                //Invalid element.
                else
                {
                    _errorCollection.AddInvalidElement(element);
                }
            }
        }

        void ParseWeaponSettingElement(XElement weaponSettingElement, WeaponTypes weaponType, WeaponSettings weaponSetting)
        {
            //Create the value generator.
            ValueGenerator valueGenerator = CreateValueGenerator(weaponSettingElement);

            //Pass the new value generator to the scheme generator.
            Debug.Assert(_schemeGenerator != null);

            _schemeGenerator.Set(weaponType, weaponSetting, valueGenerator);
        }

        void SetValue(XElement element, XAttribute attribute, Setting setting)
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
                    _errorCollection.AddAttributeValueOutOfRange(element, attribute, setting);
                }
            }
            else
            {
                _errorCollection.AddAttributeValueNonInteger(element, attribute);
            }
        }

        ValueGenerator CreateValueGenerator(XElement settingElement)
        {
            ValueGenerator outValueGenerator = null;

            //Iterate through all elements.
            IEnumerable<XElement> elements = settingElement.Elements();

            foreach (XElement element in elements)
            {
                //Handle value element.
                ValueGeneratorTypes valueGeneratorType;

                if (Enum.TryParse<ValueGeneratorTypes>(element.Name.LocalName, out valueGeneratorType))
                {
                    switch (valueGeneratorType)
                    {
                    case ValueGeneratorTypes.Set:
                        outValueGenerator = CreateSetValueGenerator(element);
                        break;

                    default:
                        Debug.Assert(false, "Invalid enum.");
                        break;
                    }
                }
                //Invalid element.
                else
                {
                    _errorCollection.AddInvalidElement(element);
                }
            }

            return outValueGenerator;
        }

        SetValueGenerator CreateSetValueGenerator(XElement setValueElement)
        {
            //Get value 
            XAttribute valueAttribute = setValueElement.Attribute("value");

            if (valueAttribute != null)
            {
                byte value;

                if (TryParseValueAttribute(setValueElement, valueAttribute, out value))
                {
                    return new SetValueGenerator(value);
                }
            }
            else
            {
                _errorCollection.AddAttributeNotFound("value", setValueElement);
            }

            return null;
        }

        bool TryParseValueAttribute(XElement element, XAttribute attribute, byte minValue, byte maxValue, out byte byteValue)
        {
            //Check the attribute's value is of the right type and in range.
            int intValue;

            if (Int32.TryParse(attribute.Value, out intValue))
            {
                if (intValue >= (int)minValue && intValue <= (int)maxValue)
                {
                    byteValue = (byte)intValue;
                    return true;
                }
                else
                {
                    _errorCollection.AddAttributeValueOutOfRange(element, attribute, minValue, maxValue);
                }
            }
            else
            {
                _errorCollection.AddAttributeValueNonInteger(element, attribute);
            }

            //Failed to get byte value.
            byteValue = 0xFF;
            return false;
        }

        bool TryParseValueAttribute(XElement element, XAttribute attribute, out byte byteValue)
        {
            return TryParseValueAttribute(element, attribute, Byte.MinValue, Byte.MaxValue, out byteValue);
        }

        string _filename;
        Scheme _scheme;
        XDocument _xDoc;

        SchemeGenerator _schemeGenerator;
        XmlErrorCollection _errorCollection;
    }
}
