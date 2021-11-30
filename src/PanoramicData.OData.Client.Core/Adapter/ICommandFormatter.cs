using System.Collections.Generic;

namespace PanoramicData.OData.Client;

public interface ICommandFormatter
{
    string FormatCommand(ResolvedCommand command);
    string FormatNavigationPath(EntityCollection entityCollection, string path);
    string ConvertKeyValuesToUriLiteral(IDictionary<string, object> key, bool skipKeyNameForSingleValue);
    string ConvertValueToUriLiteral(object value, bool escapeString);
    FunctionFormat FunctionFormat { get; }
}
