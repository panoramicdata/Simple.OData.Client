using System.Linq;

namespace PanoramicData.OData.Client.Extensions;

internal static class StringExtensions
{
	public static bool IsAllUpperCase(this string str) => !str.Cast<char>().Any(char.IsLower);

	public static string NullIfWhitespace(this string str) => string.IsNullOrWhiteSpace(str) ? null : str;

	public static string OrDefault(this string str, string defaultValue) => str ?? defaultValue;

	public static string EnsureStartsWith(this string source, string value) => (source == null || source.StartsWith(value)) ? source : value + source;
}
