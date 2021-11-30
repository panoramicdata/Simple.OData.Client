using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PanoramicData.OData.Client.Extensions;

namespace PanoramicData.OData.Client;

public partial class BoundClient<T>
{
	public Task<IEnumerable<T>> FindEntriesAsync() => FindEntriesAsync(CancellationToken.None);

	public Task<IEnumerable<T>> FindEntriesAsync(CancellationToken cancellationToken) => FilterAndTypeColumnsAsync(
			_client.FindEntriesAsync(_command, false, null, cancellationToken),
			_command.SelectedColumns, _command.DynamicPropertiesContainerName);

	public Task<IEnumerable<T>> FindEntriesAsync(bool scalarResult) => FindEntriesAsync(scalarResult, CancellationToken.None);

	public Task<IEnumerable<T>> FindEntriesAsync(bool scalarResult, CancellationToken cancellationToken) => FilterAndTypeColumnsAsync(
			_client.FindEntriesAsync(_command, scalarResult, null, cancellationToken),
			_command.SelectedColumns, _command.DynamicPropertiesContainerName);

	public Task<IEnumerable<T>> FindEntriesAsync(ODataFeedAnnotations annotations) => FindEntriesAsync(annotations, CancellationToken.None);

	public async Task<IEnumerable<T>> FindEntriesAsync(ODataFeedAnnotations annotations, CancellationToken cancellationToken)
    {
        await _session.ResolveAdapterAsync(cancellationToken);
        var command = _command.WithCount().Resolve(_session);
        if (cancellationToken.IsCancellationRequested)
		{
			cancellationToken.ThrowIfCancellationRequested();
		}

		var result = _client.FindEntriesAsync(command.Format(), annotations, cancellationToken);
        return await FilterAndTypeColumnsAsync(
            result, _command.SelectedColumns, _command.DynamicPropertiesContainerName).ConfigureAwait(false);
    }

	public Task<IEnumerable<T>> FindEntriesAsync(Uri annotatedUri, ODataFeedAnnotations annotations) => FindEntriesAsync(annotatedUri, annotations, CancellationToken.None);

	public async Task<IEnumerable<T>> FindEntriesAsync(Uri annotatedUri, ODataFeedAnnotations annotations, CancellationToken cancellationToken)
    {
        var commandText = annotatedUri.AbsoluteUri;
        if (cancellationToken.IsCancellationRequested)
		{
			cancellationToken.ThrowIfCancellationRequested();
		}

		var result = _client.FindEntriesAsync(commandText, annotations, cancellationToken);
        return await FilterAndTypeColumnsAsync(
            result, _command.SelectedColumns, _command.DynamicPropertiesContainerName).ConfigureAwait(false);
    }

	public Task<T> FindEntryAsync() => FindEntryAsync(CancellationToken.None);

	public Task<T> FindEntryAsync(CancellationToken cancellationToken) => FilterAndTypeColumnsAsync(
			_client.FindEntryAsync(_command, cancellationToken),
			_command.SelectedColumns, _command.DynamicPropertiesContainerName);

	public Task<U> FindScalarAsync<U>() => FindScalarAsync<U>(CancellationToken.None);

	public async Task<U> FindScalarAsync<U>(CancellationToken cancellationToken)
    {
        var result = await _client.FindScalarAsync(_command, cancellationToken).ConfigureAwait(false);
        return _client.IsBatchRequest
            ? default(U)
            : _session.TypeCache.Convert<U>(result);
    }

	public Task<T> InsertEntryAsync() => InsertEntryAsync(true, CancellationToken.None);

	public Task<T> InsertEntryAsync(CancellationToken cancellationToken) => InsertEntryAsync(true, cancellationToken);

	public Task<T> InsertEntryAsync(bool resultRequired) => InsertEntryAsync(resultRequired, CancellationToken.None);

	public async Task<T> InsertEntryAsync(bool resultRequired, CancellationToken cancellationToken)
    {
        var result = await _client.InsertEntryAsync(_command, resultRequired, cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(_command.DynamicPropertiesContainerName))
        {
            TypeCache.Register<T>(_command.DynamicPropertiesContainerName);
        }
        return result.ToObject<T>(TypeCache, _dynamicResults);
    }

	public Task<T> UpdateEntryAsync() => UpdateEntryAsync(true, CancellationToken.None);

	public Task<T> UpdateEntryAsync(CancellationToken cancellationToken) => UpdateEntryAsync(true, cancellationToken);

	public Task<T> UpdateEntryAsync(bool resultRequired) => UpdateEntryAsync(resultRequired, CancellationToken.None);

	public async Task<T> UpdateEntryAsync(bool resultRequired, CancellationToken cancellationToken)
    {
        if (_command.Details.HasFilter)
        {
            var result = await UpdateEntriesAsync(resultRequired, cancellationToken).ConfigureAwait(false);
            return resultRequired
                ? result?.First()
                : null;
        }
        else
        {
            var result = await _client.UpdateEntryAsync(_command, resultRequired, cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(_command.DynamicPropertiesContainerName))
            {
                TypeCache.Register<T>(_command.DynamicPropertiesContainerName);
            }
            return resultRequired
                ? result?.ToObject<T>(TypeCache, _dynamicResults)
                : null;
        }
    }

	public Task<IEnumerable<T>> UpdateEntriesAsync() => UpdateEntriesAsync(true, CancellationToken.None);

	public Task<IEnumerable<T>> UpdateEntriesAsync(CancellationToken cancellationToken) => UpdateEntriesAsync(true, cancellationToken);

	public Task<IEnumerable<T>> UpdateEntriesAsync(bool resultRequired) => UpdateEntriesAsync(resultRequired, CancellationToken.None);

	public async Task<IEnumerable<T>> UpdateEntriesAsync(bool resultRequired, CancellationToken cancellationToken)
    {
        var result = await _client.UpdateEntriesAsync(_command, resultRequired, cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(_command.DynamicPropertiesContainerName))
        {
            TypeCache.Register<T>(_command.DynamicPropertiesContainerName);
        }
        return result.Select(y => y.ToObject<T>(TypeCache, _dynamicResults));
    }

	public Task DeleteEntryAsync() => DeleteEntryAsync(CancellationToken.None);

	public Task DeleteEntryAsync(CancellationToken cancellationToken)
    {
        if (_command.Details.HasFilter)
		{
			return DeleteEntriesAsync(cancellationToken);
		}
		else
		{
			return _client.DeleteEntryAsync(_command, cancellationToken);
		}
	}

	public Task<int> DeleteEntriesAsync() => DeleteEntriesAsync(CancellationToken.None);

	public Task<int> DeleteEntriesAsync(CancellationToken cancellationToken) => _client.DeleteEntriesAsync(_command, cancellationToken);

	public Task LinkEntryAsync<U>(U linkedEntryKey) => LinkEntryAsync(linkedEntryKey, null, CancellationToken.None);

	public Task LinkEntryAsync<U>(U linkedEntryKey, CancellationToken cancellationToken) => LinkEntryAsync(linkedEntryKey, null, cancellationToken);

	public Task LinkEntryAsync<U>(U linkedEntryKey, string linkName) => LinkEntryAsync(linkedEntryKey, linkName, CancellationToken.None);

	public Task LinkEntryAsync<U>(U linkedEntryKey, string linkName, CancellationToken cancellationToken) => _client.LinkEntryAsync(_command, linkName ?? typeof(U).Name, linkedEntryKey.ToDictionary(_session.TypeCache), cancellationToken);

	public Task LinkEntryAsync<U>(Expression<Func<T, U>> expression, U linkedEntryKey) => LinkEntryAsync(expression, linkedEntryKey, CancellationToken.None);

	public Task LinkEntryAsync<U>(Expression<Func<T, U>> expression, U linkedEntryKey, CancellationToken cancellationToken) => _client.LinkEntryAsync(_command, ColumnExpression.ExtractColumnName(expression, _session.TypeCache), linkedEntryKey.ToDictionary(_session.TypeCache), cancellationToken);

	public Task LinkEntryAsync(ODataExpression expression, IDictionary<string, object> linkedEntryKey) => _client.LinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey.ToDictionary(), CancellationToken.None);

	public Task LinkEntryAsync(ODataExpression expression, IDictionary<string, object> linkedEntryKey, CancellationToken cancellationToken) => _client.LinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey.ToDictionary(), cancellationToken);

	public Task LinkEntryAsync(ODataExpression expression, ODataEntry linkedEntryKey) => _client.LinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey.ToDictionary(_session.TypeCache), CancellationToken.None);

	public Task LinkEntryAsync(ODataExpression expression, ODataEntry linkedEntryKey, CancellationToken cancellationToken) => _client.LinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey.ToDictionary(_session.TypeCache), cancellationToken);

	public Task UnlinkEntryAsync<U>() => _client.UnlinkEntryAsync(_command, typeof(U).Name, null, CancellationToken.None);

	public Task UnlinkEntryAsync<U>(CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, typeof(U).Name, null, cancellationToken);

	public Task UnlinkEntryAsync(string linkName) => _client.UnlinkEntryAsync(_command, linkName, null, CancellationToken.None);

	public Task UnlinkEntryAsync(string linkName, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, linkName, null, cancellationToken);

	public Task UnlinkEntryAsync<U>(Expression<Func<T, U>> expression) => _client.UnlinkEntryAsync(_command, expression.ExtractColumnName(_session.TypeCache), null, CancellationToken.None);

	public Task UnlinkEntryAsync<U>(Expression<Func<T, U>> expression, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, expression.ExtractColumnName(_session.TypeCache), null, cancellationToken);

	public Task UnlinkEntryAsync(ODataExpression expression) => _client.UnlinkEntryAsync(_command, expression.AsString(_session), null, CancellationToken.None);

	public Task UnlinkEntryAsync(ODataExpression expression, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, expression.AsString(_session), null, cancellationToken);

	public Task UnlinkEntryAsync<U>(U linkedEntryKey) => UnlinkEntryAsync(linkedEntryKey, CancellationToken.None);

	public Task UnlinkEntryAsync<U>(U linkedEntryKey, CancellationToken cancellationToken)
    {
        if (linkedEntryKey.GetType() == typeof(string))
		{
			return UnlinkEntryAsync(linkedEntryKey.ToString(), cancellationToken);
		}
		else if (linkedEntryKey is ODataExpression && (linkedEntryKey as ODataExpression).Reference != null)
		{
			return UnlinkEntryAsync((linkedEntryKey as ODataExpression).Reference, cancellationToken);
		}
		else
		{
			return UnlinkEntryAsync(linkedEntryKey, null, cancellationToken);
		}
	}

	public Task UnlinkEntryAsync<U>(U linkedEntryKey, string linkName) => UnlinkEntryAsync(linkedEntryKey, linkName, CancellationToken.None);

	public Task UnlinkEntryAsync<U>(U linkedEntryKey, string linkName, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, linkName ?? typeof(U).Name, linkedEntryKey?.ToDictionary(_session.TypeCache), cancellationToken);

	public Task UnlinkEntryAsync<U>(Expression<Func<T, U>> expression, U linkedEntryKey) => UnlinkEntryAsync(expression, linkedEntryKey, CancellationToken.None);

	public Task UnlinkEntryAsync<U>(Expression<Func<T, U>> expression, U linkedEntryKey, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, expression.ExtractColumnName(_session.TypeCache), linkedEntryKey?.ToDictionary(_session.TypeCache), cancellationToken);

	public Task UnlinkEntryAsync(ODataExpression expression, IDictionary<string, object> linkedEntryKey) => _client.UnlinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey?.ToDictionary(), CancellationToken.None);

	public Task UnlinkEntryAsync(ODataExpression expression, IDictionary<string, object> linkedEntryKey, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey?.ToDictionary(), cancellationToken);

	public Task UnlinkEntryAsync(ODataExpression expression, ODataEntry linkedEntryKey) => _client.UnlinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey?.ToDictionary(_session.TypeCache), CancellationToken.None);

	public Task UnlinkEntryAsync(ODataExpression expression, ODataEntry linkedEntryKey, CancellationToken cancellationToken) => _client.UnlinkEntryAsync(_command, expression.AsString(_session), linkedEntryKey?.ToDictionary(_session.TypeCache), cancellationToken);
}
