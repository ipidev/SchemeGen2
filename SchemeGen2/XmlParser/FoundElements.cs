using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchemeGen2.XmlParser
{
	/// <summary>
	/// Represents a collection of XML elements that have been found in the current context.
	/// </summary>
	class FoundElements
	{
		public FoundElements()
		{
			_settingTypes = new Dictionary<SettingTypes, XElement>();
			_weaponTypes = new Dictionary<WeaponTypes, XElement>();
			_weaponSettings = new Dictionary<WeaponSettings, XElement>();
			_elementTypes = new Dictionary<ElementTypes, XElement>();
			_extendedOptionTypes = new Dictionary<ExtendedOptionTypes, XElement>();
		}

		public bool Contains(SettingTypes setting)
		{
			return _settingTypes.ContainsKey(setting);
		}

		public bool Contains(WeaponTypes weapon)
		{
			return _weaponTypes.ContainsKey(weapon);
		}

		public bool Contains(WeaponSettings weaponSetting)
		{
			return _weaponSettings.ContainsKey(weaponSetting);
		}

		public bool Contains(ElementTypes element)
		{
			return _elementTypes.ContainsKey(element);
		}

		public bool Contains(ExtendedOptionTypes extendedOptionType)
		{
			return _extendedOptionTypes.ContainsKey(extendedOptionType);
		}

		public void Add(SettingTypes setting, XElement element)
		{
			_settingTypes.Add(setting, element);
		}

		public void Add(WeaponTypes weapon, XElement element)
		{
			_weaponTypes.Add(weapon, element);
		}

		public void Add(WeaponSettings weaponSetting, XElement element)
		{
			_weaponSettings.Add(weaponSetting, element);
		}

		public void Add(ElementTypes elementType, XElement element)
		{
			_elementTypes.Add(elementType, element);
		}

		public void Add(ExtendedOptionTypes extendedOptionType, XElement element)
		{
			_extendedOptionTypes.Add(extendedOptionType, element);
		}

		public XElement Get(SettingTypes setting)
		{
			return _settingTypes[setting];
		}

		public XElement Get(WeaponTypes weapon)
		{
			return _weaponTypes[weapon];
		}

		public XElement Get(WeaponSettings weaponSetting)
		{
			return _weaponSettings[weaponSetting];
		}

		public XElement Get(ElementTypes element)
		{
			return _elementTypes[element];
		}

		public XElement Get(ExtendedOptionTypes extendedOptionType)
		{
			return _extendedOptionTypes[extendedOptionType];
		}

		Dictionary<SettingTypes, XElement> _settingTypes;
		Dictionary<WeaponTypes, XElement> _weaponTypes;
		Dictionary<WeaponSettings, XElement> _weaponSettings;
		Dictionary<ElementTypes, XElement> _elementTypes;
		Dictionary<ExtendedOptionTypes, XElement> _extendedOptionTypes;
	}
}
