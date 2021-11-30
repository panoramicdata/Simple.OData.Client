using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace PanoramicData.OData.Client.Extensions;

internal static class HomogenizeEx
{
    private static readonly ConcurrentDictionary<string, string> Cache
        = new(StringComparer.OrdinalIgnoreCase);
    private static Regex _homogenizeRegex = new(@"[\s\p{P}]");

	/// <summary>
	/// Downshift a string and remove all non-alphanumeric characters.
	/// </summary>
	/// <param name="source">The original string.</param>
	/// <returns>The modified string.</returns>
	public static string Homogenize(this string source) => source == null ? null : Cache.GetOrAdd(source, HomogenizeImpl);

	private static string HomogenizeImpl(string source) => _homogenizeRegex.Replace(source.ToLowerInvariant(), string.Empty);

	/// <summary>
	/// Sets the regular expression to be used for homogenizing object names.
	/// </summary>
	/// <param name="regex">A regular expression matching all non-comparing characters. The default is &quot;[^a-z0-9]&quot;.</param>
	/// <remarks>Homogenized strings are always forced to lower-case.</remarks>
	public static void SetRegularExpression(Regex regex) => _homogenizeRegex = regex;
}
