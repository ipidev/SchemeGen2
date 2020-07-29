using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using SchemeGen2.Randomisation;
using SchemeGen2.Randomisation.Guarantees;
using SchemeGen2.Randomisation.ValueGenerators;

namespace SchemeGen2.XmlParser
{
	class XmlParser
	{
		public XmlParser(string filename)
		{
			_filename = filename;
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
				if (element.Name.LocalName == ElementTypes.Metascheme.ToString())
				{
					if (!foundElements.Contains(ElementTypes.Metascheme))
					{
						foundElements.Add(ElementTypes.Metascheme, element);
						ParseMetaschemeElement(element);
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
			if (!foundElements.Contains(ElementTypes.Metascheme))
			{
				_errorCollection.AddElementNotFound(ElementTypes.Metascheme.ToString());
			}

			//Set the out variables and lose our stored references to them.
			schemeGenerator = _schemeGenerator;
			_schemeGenerator = null;

			errorCollection = _errorCollection;
			_errorCollection = null;

			return errorCollection.Errors.Count == 0; 
		}

		void ParseMetaschemeElement(XElement metaschemeElement)
		{
			FoundElements foundElements = new FoundElements();

			//Iterate through all elements.
			IEnumerable<XElement> elements = metaschemeElement.Elements();

			foreach (XElement element in elements)
			{
				//TODO: Can I generalise these calls?
				//Handle settings element.
				if (element.Name.LocalName == ElementTypes.Information.ToString())
				{
					if (!foundElements.Contains(ElementTypes.Information))
					{
						foundElements.Add(ElementTypes.Information, element);
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.Information));
					}
				}
				//Handle settings element.
				else if (element.Name.LocalName == ElementTypes.Settings.ToString())
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
				//Handle guarantees element.
				else if (element.Name.LocalName == ElementTypes.Guarantees.ToString())
				{
					if (!foundElements.Contains(ElementTypes.Guarantees))
					{
						foundElements.Add(ElementTypes.Guarantees, element);
						ParseGuaranteesElement(element);
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.Guarantees));
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
					if (settingType != SettingTypes.Version && settingType != SettingTypes.BountyMode && settingType != SettingTypes.Count)
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
					else
					{
						_errorCollection.AddInvalidElement(element);
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
			ValueGenerator valueGenerator = CreateValueGenerator(settingElement, SchemeLimits.GetSettingLimits(settingType));

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
					if (weapon != WeaponTypes.Count)
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
					else
					{
						_errorCollection.AddInvalidElement(element);
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
					if (weaponSetting != WeaponSettings.Count)
					{
						if (!foundElements.Contains(weaponSetting))
						{
							foundElements.Add(weaponSetting, element);

							if (SchemeTypes.CanApplyWeaponSetting(weaponType, weaponSetting))
							{
								ParseWeaponSettingElement(element, weaponType, weaponSetting);
							}
							else
							{
								_errorCollection.AddSettingNotApplicableToWeapon(element, weaponType, weaponSetting);
							}
						}
						else
						{
							_errorCollection.AddRepeatedElement(element, foundElements.Get(weaponSetting));
						}
					}
					else
					{
						_errorCollection.AddInvalidElement(element);
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
			ValueGenerator valueGenerator = CreateValueGenerator(weaponSettingElement, SchemeLimits.GetWeaponSettingLimits(weaponType, weaponSetting));

			//Pass the new value generator to the scheme generator.
			Debug.Assert(_schemeGenerator != null);

			if (weaponType != WeaponTypes.Default)
			{
				_schemeGenerator.Set(weaponType, weaponSetting, valueGenerator);
			}
			else
			{
				_schemeGenerator.SetDefault(weaponSetting, valueGenerator);
			}
		}

		ValueGenerator CreateValueGenerator(XElement settingElement, SettingLimits settingLimits = null)
		{
			ValueGenerator outValueGenerator = null;
			bool hasCreatedValueGenerator = false;

			//Iterate through all elements.
			IEnumerable<XElement> elements = settingElement.Elements();

			foreach (XElement element in elements)
			{
				//Warn about more than one value element.
				if (hasCreatedValueGenerator)
				{
					_errorCollection.AddInvalidElement(element);
					continue;
				}

				//Handle value element.
				ValueGeneratorTypes valueGeneratorType;
				if (Enum.TryParse<ValueGeneratorTypes>(element.Name.LocalName, out valueGeneratorType))
				{
					switch (valueGeneratorType)
					{
					case ValueGeneratorTypes.Null:
						outValueGenerator = null;
						break;

					case ValueGeneratorTypes.Constant:
						outValueGenerator = CreateConstantValueGenerator(element);
						break;

					case ValueGeneratorTypes.Range:
						outValueGenerator = CreateRangeValueGenerator(element);
						break;

					case ValueGeneratorTypes.Boolean:
						outValueGenerator = CreateBooleanValueGenerator(element);
						break;

					case ValueGeneratorTypes.CoinFlip:
						outValueGenerator = CreateCoinFlipValueGenerator(element);
						break;

					case ValueGeneratorTypes.WeightedSelector:
						outValueGenerator = CreateWeightedSelectorValueGenerator(element, settingLimits);
						break;

					default:
						_errorCollection.AddInvalidElement(element);
						break;
					}

					hasCreatedValueGenerator = outValueGenerator != null || valueGeneratorType == ValueGeneratorTypes.Null;

					if (outValueGenerator != null && settingLimits != null && !outValueGenerator.IsValueRangeWithinLimits(settingLimits))
					{
						_errorCollection.AddSettingOutsideLimits(element, settingLimits);
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

		ConstantValueGenerator CreateConstantValueGenerator(XElement constantValueElement)
		{
			//Get value 
			XAttribute valueAttribute = constantValueElement.Attribute("value");
			if (valueAttribute != null)
			{
				int value;
				if (TryParseIntegerAttribute(constantValueElement, valueAttribute, out value))
				{
					return new ConstantValueGenerator(value);
				}
			}
			else
			{
				_errorCollection.AddAttributeNotFound("value", constantValueElement);
			}

			return null;
		}

		RangeValueGenerator CreateRangeValueGenerator(XElement rangeValueElement)
		{
			int? min = null;
			int? max = null;

			//Get min
			XAttribute minAttribute = rangeValueElement.Attribute("min");
			if (minAttribute != null)
			{
				int value;
				if (TryParseIntegerAttribute(rangeValueElement, minAttribute, out value))
				{
					min = value;
				}
			}
			else
			{
				_errorCollection.AddAttributeNotFound("min", rangeValueElement);
			}

			//Get max
			XAttribute maxAttribute = rangeValueElement.Attribute("max");
			if (maxAttribute != null)
			{
				int value;
				if (TryParseIntegerAttribute(rangeValueElement, maxAttribute, out value))
				{
					max = value;
				}
			}
			else
			{
				_errorCollection.AddAttributeNotFound("max", rangeValueElement);
			}

			if (!min.HasValue || !max.HasValue)
				return null;
			
			RangeValueGenerator rangeValueGenerator = new RangeValueGenerator(min.Value, max.Value);

			//Optionally get step
			XAttribute stepAttribute = rangeValueElement.Attribute("step");
			if (stepAttribute != null)
			{
				int value;
				if (TryParseIntegerAttribute(rangeValueElement, stepAttribute, out value))
				{
					rangeValueGenerator.Step = value;
				}
			}

			return rangeValueGenerator;
		}

		BooleanValueGenerator CreateBooleanValueGenerator(XElement booleanValueElement)
		{
			//Get chance 
			XAttribute chanceAttribute = booleanValueElement.Attribute("chance");

			if (chanceAttribute != null)
			{
				double chance;
				if (TryParseDoubleAttribute(booleanValueElement, chanceAttribute, 0.0, 1.0, out chance))
				{
					return new BooleanValueGenerator(chance);
				}
			}
			else
			{
				_errorCollection.AddAttributeNotFound("value", booleanValueElement);
			}

			return null;
		}

		CoinFlipValueGenerator CreateCoinFlipValueGenerator(XElement coinFlipValueElement)
		{
			double chance = 0.5;
			int? headsValue = null;
			int? tailsValue = null;

			//Get headsValue
			XAttribute headsValueAttribute = coinFlipValueElement.Attribute("headsValue");
			if (headsValueAttribute != null)
			{
				int value;
				if (TryParseIntegerAttribute(coinFlipValueElement, headsValueAttribute, out value))
				{
					headsValue = value;
				}
			}
			else
			{
				_errorCollection.AddAttributeNotFound("headsValue", coinFlipValueElement);
			}

			//Get tailsValue
			XAttribute tailsValueAttribute = coinFlipValueElement.Attribute("tailsValue");
			if (tailsValueAttribute != null)
			{
				int value;
				if (TryParseIntegerAttribute(coinFlipValueElement, tailsValueAttribute, out value))
				{
					tailsValue = value;
				}
			}
			else
			{
				_errorCollection.AddAttributeNotFound("tailsValue", coinFlipValueElement);
			}

			//Optionally get chance
			XAttribute chanceAttribute = coinFlipValueElement.Attribute("headsChance");
			if (chanceAttribute != null)
			{
				TryParseDoubleAttribute(coinFlipValueElement, chanceAttribute, out chance);
			}

			if (headsValue.HasValue && tailsValue.HasValue)
			{
				return new CoinFlipValueGenerator(chance, headsValue.Value, tailsValue.Value);
			}

			return null;
		}

		WeightedSelectorValueGenerator CreateWeightedSelectorValueGenerator(XElement weightedSelectorValueElement, SettingLimits settingLimits)
		{
			WeightedSelectorValueGenerator weightedSelectorValueGenerator = new WeightedSelectorValueGenerator();

			foreach (XElement element in weightedSelectorValueElement.Elements())
			{
				if (element.Name.LocalName == "Selection")
				{
					double weight = 1.0;

					//Optionally get weight
					XAttribute weightAttribute = element.Attribute("weight");
					if (weightAttribute != null)
					{
						TryParseDoubleAttribute(element, weightAttribute, out weight);
					}

					ValueGenerator valueGenerator = CreateValueGenerator(element, settingLimits);
					if (valueGenerator != null)
					{
						weightedSelectorValueGenerator.AddSelection(valueGenerator, weight);
					}
				}
				//Invalid element.
				else
				{
					_errorCollection.AddInvalidElement(element);
				}
			}

			return weightedSelectorValueGenerator;
		}

		void ParseGuaranteesElement(XElement guaranteesElement)
		{
			//Iterate through all elements.
			IEnumerable<XElement> elements = guaranteesElement.Elements();
			foreach (XElement element in elements)
			{
				Guarantee guarantee = null;

				//Handle guarantee element.
				GuaranteeTypes guranteeType;
				if (Enum.TryParse<GuaranteeTypes>(element.Name.LocalName, out guranteeType))
				{
					switch (guranteeType)
					{
					case GuaranteeTypes.WeaponSetting:
						guarantee = CreateWeaponSettingGuarantee(element);
						break;

					case GuaranteeTypes.GodWormsIndyTerrainExclusivity:
						guarantee = new GodWormsIndyTerrainExclusivityGuarantee();
						break;

					case GuaranteeTypes.WeaponsPerFunctionKey:
						guarantee = new WeaponsPerFunctionKeyGuarantee(CreateValueGenerator(element));
						break;

					default:
						_errorCollection.AddInvalidElement(element);
						break;
					}
				}
				//Invalid element.
				else
				{
					_errorCollection.AddInvalidElement(element);
				}

				if (guarantee != null)
				{
					_schemeGenerator.AddGuarantee(guarantee);
				}
			}
		}

		WeaponSettingGuarantee CreateWeaponSettingGuarantee(XElement weaponSettingGuaranteeElement)
		{
			//Get setting
			XAttribute settingAttribute = weaponSettingGuaranteeElement.Attribute("setting");
			if (settingAttribute == null)
			{
				_errorCollection.AddAttributeNotFound("setting", weaponSettingGuaranteeElement);
				return null;
			}

			WeaponSettings weaponSetting = WeaponSettings.Count;
			Enum.TryParse<WeaponSettings>(settingAttribute.Value, out weaponSetting);
			if (weaponSetting == WeaponSettings.Count)
			{
				_errorCollection.AddAttributeValueInvalidValue(weaponSettingGuaranteeElement, settingAttribute);
				return null;
			}

			WeaponSettingGuarantee weaponSettingGuarantee = new WeaponSettingGuarantee(weaponSetting);
			SettingLimits weaponCountSettingLimits = new SettingLimits(0, 64);
			SettingLimits settingValueSettingLimits = new SettingLimits(0, 255);

			//Iterate through all elements.
			FoundElements foundElements = new FoundElements();

			IEnumerable<XElement> elements = weaponSettingGuaranteeElement.Elements();
			foreach (XElement element in elements)
			{
				//Handle minimum weapons element.
				if (element.Name.LocalName == ElementTypes.MinimumWeaponCount.ToString())
				{
					if (!foundElements.Contains(ElementTypes.MinimumWeaponCount))
					{
						foundElements.Add(ElementTypes.MinimumWeaponCount, element);
						
						weaponSettingGuarantee.MinimumWeaponCount = CreateValueGenerator(element, weaponCountSettingLimits);
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.MinimumWeaponCount));
					}
				}
				//Handle maximum weapons element.
				else if (element.Name.LocalName == ElementTypes.MaximumWeaponCount.ToString())
				{
					if (!foundElements.Contains(ElementTypes.MaximumWeaponCount))
					{
						foundElements.Add(ElementTypes.MaximumWeaponCount, element);
						
						ParseMaximumWeaponCountElement(element, weaponSettingGuarantee, weaponCountSettingLimits);
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.MaximumWeaponCount));
					}
				}
				//Handle minimum value element.
				else if (element.Name.LocalName == ElementTypes.MinimumSettingValue.ToString())
				{
					if (!foundElements.Contains(ElementTypes.MinimumSettingValue))
					{
						foundElements.Add(ElementTypes.MinimumSettingValue, element);
						
						weaponSettingGuarantee.MinimumSettingValue = CreateValueGenerator(element, settingValueSettingLimits);
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.MinimumSettingValue));
					}
				}
				//Handle maximum weapons element.
				else if (element.Name.LocalName == ElementTypes.MaximumSettingValue.ToString())
				{
					if (!foundElements.Contains(ElementTypes.MaximumSettingValue))
					{
						foundElements.Add(ElementTypes.MaximumSettingValue, element);
						
						weaponSettingGuarantee.MaximumSettingValue = CreateValueGenerator(element, settingValueSettingLimits);
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.MaximumSettingValue));
					}
				}
				//Handle inclusion filter element.
				else if (element.Name.LocalName == ElementTypes.InclusionFilter.ToString())
				{
					if (!foundElements.Contains(ElementTypes.InclusionFilter))
					{
						foundElements.Add(ElementTypes.InclusionFilter, element);
						
						ParseWeaponFilterElement(element, ref weaponSettingGuarantee.GetCategoryInclusionFilterRef(), ref weaponSettingGuarantee.GetInclusionMatchTypeRef());
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.InclusionFilter));
					}
				}
				//Handle inclusion filter element.
				else if (element.Name.LocalName == ElementTypes.ExclusionFilter.ToString())
				{
					if (!foundElements.Contains(ElementTypes.ExclusionFilter))
					{
						foundElements.Add(ElementTypes.ExclusionFilter, element);
						
						ParseWeaponFilterElement(element, ref weaponSettingGuarantee.GetCategoryExclusionFilterRef(), ref weaponSettingGuarantee.GetExclusionMatchTypeRef());
					}
					else
					{
						_errorCollection.AddRepeatedElement(element, foundElements.Get(ElementTypes.ExclusionFilter));
					}
				}
				//Invalid element.
				else
				{
					_errorCollection.AddInvalidElement(element);
				}
			}

			return weaponSettingGuarantee;
		}

		void ParseWeaponFilterElement(XElement weaponFilterElement, ref WeaponCategoryFlags? flags, ref WeaponCategoryMatchType matchType)
		{
			//Optionally get match type
			XAttribute matchTypeAttribute = weaponFilterElement.Attribute("matchType");
			if (matchTypeAttribute != null)
			{
				if (!Enum.TryParse<WeaponCategoryMatchType>(matchTypeAttribute.Value, out matchType))
				{
					_errorCollection.AddAttributeValueInvalidValue(weaponFilterElement, matchTypeAttribute);
				}
			}

			//Iterate through all elements.
			IEnumerable<XElement> elements = weaponFilterElement.Elements();
			foreach (XElement element in elements)
			{
				if (element.Name.LocalName != "Category")
				{
					_errorCollection.AddInvalidElement(element);
					continue;
				}

				//Get name
				XAttribute nameAttribute = element.Attribute("name");
				if (nameAttribute != null)
				{
					WeaponCategoryFlags weaponCategoryFlag;
					if (Enum.TryParse<WeaponCategoryFlags>(nameAttribute.Value, out weaponCategoryFlag))
					{
						 if (!flags.HasValue)
							flags = weaponCategoryFlag;
						else
							flags |= weaponCategoryFlag;
					}
					else
					{
						_errorCollection.AddAttributeValueInvalidValue(element, nameAttribute);
					}
				}
				else
				{
					_errorCollection.AddAttributeNotFound("name", element);
				}
			}
		}

		void ParseMaximumWeaponCountElement(XElement maximumWeaponCountElement, WeaponSettingGuarantee weaponSettingGuarantee, SettingLimits weaponCountSettingLimits)
		{
			weaponSettingGuarantee.MaximumWeaponCount = CreateValueGenerator(maximumWeaponCountElement, weaponCountSettingLimits);

			//Parse optional attributes.
			IEnumerable<XAttribute> attributes = maximumWeaponCountElement.Attributes();
			foreach (XAttribute attribute in attributes)
			{
				if (attribute.Name == "forceExcessWeaponsToValue")
				{
					int intValue;
					if (TryParseIntegerAttribute(maximumWeaponCountElement, attribute, out intValue))
					{
						weaponSettingGuarantee.MaximumWeaponCountForcedValue = intValue;
					}
				}
				else
				{
					_errorCollection.AddInvalidAttribute(maximumWeaponCountElement, attribute);
				}
			}
		}

		bool TryParseIntegerAttribute(XElement element, XAttribute attribute, int minValue, int maxValue, out int intValue)
		{
			//Check the attribute's value is of the right type and in range.
			bool hasParsedValue = hasParsedValue = Int32.TryParse(attribute.Value, out intValue);
			if (!hasParsedValue)
			{
				//Allow boolean strings.
				if (attribute.Value.ToLower() == "true")
				{
					intValue = SchemeTypes.True;
					hasParsedValue = true;
				}
				else if (attribute.Value.ToLower() == "false")
				{
					intValue = SchemeTypes.False;
					hasParsedValue = true;
				}
			}

			if (hasParsedValue)
			{
				if (intValue >= (int)minValue && intValue <= (int)maxValue)
				{
					return true;
				}
				else
				{
					_errorCollection.AddAttributeValueOutOfRange(element, attribute, minValue, maxValue);
				}
			}
			else
			{
				_errorCollection.AddAttributeValueNonNumber(element, attribute);
			}

			//Failed to get value.
			return false;
		}

		bool TryParseIntegerAttribute(XElement element, XAttribute attribute, out int intValue)
		{
			return TryParseIntegerAttribute(element, attribute, Int32.MinValue, Int32.MaxValue, out intValue);
		}

		bool TryParseDoubleAttribute(XElement element, XAttribute attribute, double minValue, double maxValue, out double doubleValue)
		{
			//Check the attribute's value is of the right type and in range.
			if (Double.TryParse(attribute.Value, out doubleValue))
			{
				if (doubleValue >= minValue && doubleValue <= maxValue)
				{
					return true;
				}
				else
				{
					_errorCollection.AddAttributeValueOutOfRange(element, attribute, minValue, maxValue);
				}
			}
			else
			{
				_errorCollection.AddAttributeValueNonNumber(element, attribute);
			}

			//Failed to get value.
			doubleValue = Double.NaN;
			return false;
		}

		bool TryParseDoubleAttribute(XElement element, XAttribute attribute, out double doubleValue)
		{
			return TryParseDoubleAttribute(element, attribute, Double.NegativeInfinity, Double.PositiveInfinity, out doubleValue);
		}

		string _filename;
		XDocument _xDoc;

		SchemeGenerator _schemeGenerator;
		XmlErrorCollection _errorCollection;
	}
}
