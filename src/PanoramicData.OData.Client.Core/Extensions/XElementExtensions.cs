﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PanoramicData.OData.Client.Extensions;

/// <summary>
/// Extension methods for <see cref="XElement"/>.
/// </summary>
internal static class XElementExtensions
{
	public static XElement Element(this XElement element, string prefix, string name) => Elements(element, prefix, name).FirstOrDefault();

	public static IEnumerable<XElement> Elements(this XElement element, string prefix, string name)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            return element.Elements().Where(
                x => x.Name.LocalName == name &&
                    string.IsNullOrEmpty(element.GetPrefixOfNamespace(x.Name.Namespace)));
        }

        return element.Elements(ResolvePrefix(element, prefix) + name);
    }

    public static IEnumerable<XElement> Descendants(this XElement element, string prefix, string name)
    {
        var result = element.Descendants(ResolvePrefix(element, prefix) + name);

        if (result.Any())
		{
			return result;
		}

		if (string.IsNullOrEmpty(prefix))
        {
            return element.Descendants().Where(
                x => x.Name.LocalName == name &&
                    string.IsNullOrEmpty(element.GetPrefixOfNamespace(x.Name.Namespace)));
        }

        return XElement.EmptySequence;
    }

	public static XAttribute Attribute(this XElement element, string prefix, string name) => element.Attribute(ResolvePrefix(element, prefix) + name);

	private static XNamespace ResolvePrefix(XElement element, string prefix) => string.IsNullOrEmpty(prefix) ? element.GetDefaultNamespace() : element.GetNamespaceOfPrefix(prefix);

	public static string ValueOrDefault(this XElement element) => element == null ? string.Empty : element.Value;
}
