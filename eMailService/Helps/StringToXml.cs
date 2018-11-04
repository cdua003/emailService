using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using eMailService.Exceptions;

namespace eMailService.Helps
{
    public class StringToXml
    {
        private static readonly string _expression = @"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}";

        public static string ToXml(string input)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml($"<root>{XmlSanitizer(input)}</root>");
            }
            catch (Exception)
            {
                throw new InvalidXmlException(GlobalConstant.INVALID_XML);
            }

            var xmlFragments = from XmlNode node in doc.FirstChild.ChildNodes
                               where node.NodeType == XmlNodeType.Element
                               select node;

            var parsedXML = new StringBuilder();
            foreach (var fragment in xmlFragments)
            {
                parsedXML.Append(fragment.OuterXml);
            }

            parsedXML.Insert(0, "<root>");
            parsedXML.Append("</root>");

            return parsedXML.ToString();
        }

        private static string XmlSanitizer(string input)
        {
            var reg = new Regex(_expression, RegexOptions.IgnoreCase);
            var results = new List<string>();
            Match match;

            for (match = reg.Match(input); match.Success; match = match.NextMatch())
            {
                input = input.Replace($"<{match.Value}>", "");
            }

            return input;
        }
    }
}