using System.Collections.Concurrent;

namespace PanoramicData.OData.Client;

public class CachedPluralizer : IPluralizer
{
    private readonly IPluralizer pluralizer;
    private readonly ConcurrentDictionary<string, string> singles;
    private readonly ConcurrentDictionary<string, string> plurals;

    public CachedPluralizer(IPluralizer pluralizer)
    {
        this.pluralizer = pluralizer;
        singles = new ConcurrentDictionary<string, string>();
        plurals = new ConcurrentDictionary<string, string>();
    }

	public string Pluralize(string word) => plurals.GetOrAdd(word, x => pluralizer.Pluralize(x));

	public string Singularize(string word) => singles.GetOrAdd(word, x => pluralizer.Singularize(x));
}
