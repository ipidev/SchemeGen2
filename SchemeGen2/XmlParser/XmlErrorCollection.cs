﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SchemeGen2.XmlParser
{
    struct XmlError
    {
        public bool HasLineNumber() { return lineNumber != -1; }

        public int lineNumber;
        public string errorString;
    };

    /// <summary>
    /// Represents a collection of errors found when parsing XML.
    /// </summary>
    class XmlErrorCollection
    {
        public XmlErrorCollection()
        {
            _errors = new List<XmlError>();
        }

        public void Add(string errorString)
        {
            XmlError xmlError;
            xmlError.lineNumber = -1;
            xmlError.errorString = errorString;

            _errors.Add(xmlError);
        }

        public void Add(string errorString, XElement element)
        {
            IXmlLineInfo lineInfo = element;

            if (lineInfo.HasLineInfo())
            {
                int lineNumber = lineInfo.LineNumber;

                XmlError xmlError;
                xmlError.lineNumber = lineNumber;
                xmlError.errorString = errorString;

                _errors.Add(xmlError);
            }
            else
            {
                Add(errorString);
            }
        }
        
        public void AddElementNotFound(string expectedElement)
        {
            string errorString = String.Format("Element '{0}' was expected but was not found.",
                expectedElement);

            Add(errorString);
        }

        public void AddElementNotFound(string expectedElement, XElement parentElement)
        {
            string errorString = String.Format("Element '{0}' was expected in parent element '{1}' but was not found.",
                expectedElement, parentElement.Name.LocalName);

            Add(errorString, parentElement);
        }

        public void AddInvalidElement(XElement element)
        {
            string errorString = String.Format("Element '{0}' is invalid.",
                element.Name.LocalName);

            Add(errorString, element);
        }

        public void AddRepeatedElement(XElement element)
        {
            string errorString = String.Format("Element '{0}' may only appear once within the current element.",
                element.Name.LocalName);

            Add(errorString, element);
        }

        public void AddRepeatedElement(XElement element, XElement originalElement)
        {
            IXmlLineInfo lineInfo = originalElement;

            if (lineInfo.HasLineInfo())
            {
                int lineNumber = lineInfo.LineNumber;

                string errorString = String.Format("Element '{0}' may only appear once within the current element. " +
                    "Original occurence is on line {1}.", element.Name.LocalName, lineNumber);

                Add(errorString, element);
            }
            else
            {
                AddRepeatedElement(element);
            }
        }

        public void AddAttributeNotFound(string expectedAttribute)
        {
            string errorString = String.Format("Attribute '{0}' was expected but was not found.",
                expectedAttribute);

            Add(errorString);
        }

        public void AddAttributeNotFound(string expectedAttribute, XElement element)
        {
            string errorString = String.Format("Attribute '{0}' was expected in element '{1}' but was not found.",
                expectedAttribute, element.Name.LocalName);

            Add(errorString, element);
        }

        public void AddInvalidAttribute(XElement element, XAttribute attribute)
        {
            string errorString = String.Format("Attribute '{0}' in element '{1}' is invalid.",
                attribute.Name.LocalName, element.Name.LocalName);

            Add(errorString, element);
        }

        public void AddAttributeValueNonInteger(XElement element, XAttribute attribute)
        {
            string errorString = String.Format("Attribute '{0}' in element '{1}' has non-integer value '{2}'.",
                attribute.Name.LocalName, element.Name.LocalName, attribute.Value);

            Add(errorString, element);
        }

        public void AddAttributeValueOutOfRange(XElement element, XAttribute attribute, Setting setting)
        {
            string errorString = String.Format("Attribute '{0}' in element '{1}' is out of range value '{2}'. Valid range is [{3} - {4}].",
                attribute.Name.LocalName, element.Name.LocalName, attribute.Value, setting.MinimumValue, setting.MaximumValue);

            Add(errorString, element);
        }

        public List<XmlError> Errors
        {
            get { return _errors; }
        }

        List<XmlError> _errors;
    }
}
