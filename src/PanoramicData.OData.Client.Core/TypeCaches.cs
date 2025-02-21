﻿using System.Collections.Concurrent;

namespace PanoramicData.OData.Client;

internal static class TypeCaches
{
    private static readonly ConcurrentDictionary<string, ITypeCache> _typeCaches;

    static TypeCaches()
    {
        // TODO: Have a global switch whether we use the dictionary or not
        _typeCaches = new ConcurrentDictionary<string, ITypeCache>();
    }

	internal static ITypeCache TypeCache(string uri, INameMatchResolver nameMatchResolver) => _typeCaches.GetOrAdd(uri, new TypeCache(CustomConverters.Converter(uri), nameMatchResolver));
}
