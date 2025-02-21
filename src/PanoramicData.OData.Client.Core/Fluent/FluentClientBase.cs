﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PanoramicData.OData.Client.Extensions;

namespace PanoramicData.OData.Client;

/// <summary>
/// Provides access to OData operations in a fluent style.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="FT"></typeparam>
public abstract class FluentClientBase<T, FT> : IFluentClient<T, FT>
    where T : class
    where FT : class
{
    protected readonly ODataClient _client;
    internal readonly Session _session;
    protected readonly FluentCommand _parentCommand;
    protected FluentCommand _command;
    protected readonly bool _dynamicResults;

    internal FluentClientBase(ODataClient client, Session session,
        FluentCommand parentCommand = null, FluentCommand command = null, bool dynamicResults = false)
    {
        _client = client;
        _session = session;
        _parentCommand = parentCommand;
        _command = command;
        _dynamicResults = dynamicResults;
    }

    internal FluentCommand Command
    {
        get
        {
            if (_command != null)
			{
				return _command;
			}

			lock (this)
            {
                return _command ??= CreateCommand();
            }
        }
    }

	protected FluentCommand CreateCommand() => new(_parentCommand, _client.BatchEntries);

	internal Session Session => _session;

    protected ITypeCache TypeCache => _session.TypeCache;

    public FT WithProperties(Expression<Func<T, IDictionary<string, object>>> expression)
    {
        Command.WithProperties(expression.ExtractColumnName(_session.TypeCache));
        return this as FT;
    }

    public FT WithMedia(IEnumerable<string> properties)
    {
        Command.WithMedia(properties);
        return this as FT;
    }

    public FT WithMedia(params string[] properties)
    {
        Command.WithMedia(properties);
        return this as FT;
    }

    public FT WithMedia(params ODataExpression[] properties)
    {
        Command.WithMedia(properties);
        return this as FT;
    }

    public FT WithMedia(Expression<Func<T, object>> expression)
    {
        Command.WithMedia(expression.ExtractColumnNames(_session.TypeCache));
        return this as FT;
    }

    public FT WithHeader(string name, string value)
    {
        Command.WithHeader(name, value);
        return this as FT;
    }

    public FT WithHeaders(IEnumerable<KeyValuePair<string, string>> headers)
    {
        Command.WithHeaders(headers);
        return this as FT;
    }

    public FT Key(params object[] entryKey)
    {
        Command.Key(entryKey);
        return this as FT;
    }

    public FT Key(IEnumerable<object> entryKey)
    {
        Command.Key(entryKey);
        return this as FT;
    }

    public FT Key(IDictionary<string, object> entryKey)
    {
        Command.Key(entryKey);
        return this as FT;
    }

    public FT Key(T entryKey)
    {
        Command.Key(entryKey.ToDictionary(_session.TypeCache));
        return this as FT;
    }

    public FT Filter(string filter)
    {
        Command.Filter(filter);
        return this as FT;
    }

    public FT Filter(ODataExpression expression)
    {
        Command.Filter(expression);
        return this as FT;
    }

    public FT Filter(Expression<Func<T, bool>> expression)
    {
        Command.Filter(ODataExpression.FromLinqExpression(expression.Body));
        return this as FT;
    }

    public FT Search(string search)
    {
        Command.Search(search);
        return this as FT;
    }

    public FT Function(string functionName)
    {
        Command.Function(functionName);
        return this as FT;
    }

    public IBoundClient<U> Function<U>(string functionName) where U : class
    {
        Command.Function(functionName);
        return new BoundClient<U>(_client, _session, _parentCommand, Command, _dynamicResults);
    }

    public FT Action(string actionName)
    {
        Command.Action(actionName);
        return this as FT;
    }

    public FT Skip(long count)
    {
        Command.Skip(count);
        return this as FT;
    }

    public FT Top(long count)
    {
        Command.Top(count);
        return this as FT;
    }

    public FT Expand(ODataExpandOptions expandOptions)
    {
        Command.Expand(expandOptions);
        return this as FT;
    }

    public FT Expand(IEnumerable<string> associations)
    {
        Command.Expand(associations.Select(ODataExpandAssociation.From));
        return this as FT;
    }

    public FT Expand(ODataExpandOptions expandOptions, IEnumerable<string> associations)
    {
        Command.Expand(expandOptions, associations.Select(ODataExpandAssociation.From));
        return this as FT;
    }

    public FT Expand(params string[] associations)
    {
        Command.Expand(associations.Select(ODataExpandAssociation.From));
        return this as FT;
    }

    public FT Expand(ODataExpandOptions expandOptions, params string[] associations)
    {
        Command.Expand(expandOptions, associations.Select(ODataExpandAssociation.From));
        return this as FT;
    }

    public FT Expand(params ODataExpression[] associations)
    {
        Command.Expand(associations.Select(a => ODataExpandAssociation.From(a.Reference)));
        return this as FT;
    }

    public FT Expand(ODataExpandOptions expandOptions, params ODataExpression[] associations)
    {
        Command.Expand(expandOptions, associations.Select(a => ODataExpandAssociation.From(a.Reference)));
        return this as FT;
    }

    public FT Expand(Expression<Func<T, object>> expression)
    {
        Command.Expand(expression.ExtractExpandAssociations(_session.TypeCache));
        return this as FT;
    }

    public FT Expand(ODataExpandOptions expandOptions, Expression<Func<T, object>> expression)
    {
        Command.Expand(expandOptions, expression.ExtractExpandAssociations(_session.TypeCache));
        return this as FT;
    }

    public FT Select(IEnumerable<string> columns)
    {
        Command.Select(columns);
        return this as FT;
    }

    public FT Select(params string[] columns)
    {
        Command.Select(columns);
        return this as FT;
    }

    public FT Select(params ODataExpression[] columns)
    {
        Command.Select(columns);
        return this as FT;
    }

    public FT Select(Expression<Func<T, object>> expression)
    {
        Command.Select(expression.ExtractColumnNames(_session.TypeCache));
        return this as FT;
    }

    public FT OrderBy(IEnumerable<KeyValuePair<string, bool>> columns)
    {
        Command.OrderBy(columns);
        return this as FT;
    }

    public FT OrderBy(params string[] columns)
    {
        Command.OrderBy(columns);
        return this as FT;
    }

    public FT OrderBy(params ODataExpression[] columns)
    {
        Command.OrderBy(columns);
        return this as FT;
    }

    public FT OrderBy(Expression<Func<T, object>> expression)
    {
        Command.OrderBy(expression.ExtractColumnNames(_session.TypeCache).Select(x => new KeyValuePair<string, bool>(x, false)));
        return this as FT;
    }

    public FT ThenBy(params ODataExpression[] columns)
    {
        Command.ThenBy(columns);
        return this as FT;
    }

    public FT ThenBy(Expression<Func<T, object>> expression)
    {
        Command.ThenBy(expression.ExtractColumnNames(TypeCache).ToArray());
        return this as FT;
    }

    public FT OrderByDescending(params string[] columns)
    {
        Command.OrderByDescending(columns);
        return this as FT;
    }

    public FT OrderByDescending(params ODataExpression[] columns)
    {
        Command.OrderByDescending(columns);
        return this as FT;
    }

    public FT OrderByDescending(Expression<Func<T, object>> expression)
    {
        Command.OrderBy(expression.ExtractColumnNames(_session.TypeCache).Select(x => new KeyValuePair<string, bool>(x, true)));
        return this as FT;
    }

    public FT ThenByDescending(params ODataExpression[] columns)
    {
        Command.ThenByDescending(columns);
        return this as FT;
    }

    public FT ThenByDescending(Expression<Func<T, object>> expression)
    {
        Command.ThenByDescending(expression.ExtractColumnNames(_session.TypeCache).ToArray());
        return this as FT;
    }

    public FT QueryOptions(string queryOptions)
    {
        Command.QueryOptions(queryOptions);
        return this as FT;
    }

    public FT QueryOptions(IDictionary<string, object> queryOptions)
    {
        Command.QueryOptions(queryOptions);
        return this as FT;
    }

    public FT QueryOptions(ODataExpression expression)
    {
        Command.QueryOptions(expression);
        return this as FT;
    }

    public FT QueryOptions<U>(Expression<Func<U, bool>> expression)
    {
        Command.QueryOptions(ODataExpression.FromLinqExpression(expression.Body));
        return this as FT;
    }

    public IMediaClient Media()
    {
        Command.Media();
        return new MediaClient(_client, _session, Command, _dynamicResults);
    }

    public IMediaClient Media(string streamName)
    {
        Command.Media(streamName);
        return new MediaClient(_client, _session, Command, _dynamicResults);
    }

    public IMediaClient Media(ODataExpression expression)
    {
        Command.Media(expression);
        return new MediaClient(_client, _session, Command, _dynamicResults);
    }

    public IMediaClient Media(Expression<Func<T, object>> expression)
    {
        Command.Media(expression.ExtractColumnName(_session.TypeCache));
        return new MediaClient(_client, _session, Command, _dynamicResults);
    }

    public FT Count()
    {
        Command.Count();
        return this as FT;
    }

    protected BoundClient<U> Link<U>(FluentCommand command, string linkName = null)
    where U : class
    {
        linkName ??= typeof(U).Name;
        var links = linkName.Split('/');
        var linkCommand = command;
        BoundClient<U> linkedClient = null;
        foreach (var link in links)
        {
            linkedClient = new BoundClient<U>(_client, _session, linkCommand, null, _dynamicResults);
            linkedClient.Command.Link(link);
            linkCommand = linkedClient.Command;
        }
        return linkedClient;
    }

	protected BoundClient<U> Link<U>(FluentCommand command, ODataExpression expression)
	where U : class => Link<U>(command, expression.Reference);


	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="linkName">Name of the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(string linkName = null)
		where U : class => Link<U>(Command, linkName);
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(Expression<Func<T, U>> expression)
		where U : class => Link<U>(Command, expression.ExtractColumnName(_session.TypeCache));
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(Expression<Func<T, IEnumerable<U>>> expression) where U : class => Link<U>(Command, expression.ExtractColumnName(_session.TypeCache));
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(Expression<Func<T, IList<U>>> expression) where U : class => Link<U>(Command, expression.ExtractColumnName(_session.TypeCache));
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(Expression<Func<T, ISet<U>>> expression) where U : class => Link<U>(Command, expression.ExtractColumnName(_session.TypeCache));
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(Expression<Func<T, HashSet<U>>> expression) where U : class => Link<U>(Command, expression.ExtractColumnName(_session.TypeCache));
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <typeparam name="U">The type of the linked entity.</typeparam>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<U> NavigateTo<U>(Expression<Func<T, U[]>> expression) where U : class => Link<U>(Command, expression.ExtractColumnName(_session.TypeCache));
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <param name="linkName">Name of the link.</param>
	/// <returns>Self.</returns>		
	public IBoundClient<IDictionary<string, object>> NavigateTo(string linkName) => Link<IDictionary<string, object>>(Command, linkName);
	/// <summary>
	/// Navigates to the linked entity.
	/// </summary>
	/// <param name="expression">The expression for the link.</param>
	/// <returns>Self.</returns>
	public IBoundClient<T> NavigateTo(ODataExpression expression) => Link<T>(Command, expression);

	/// <summary>
	/// Executes the OData function or action.
	/// </summary>
	/// <returns>Execution result task.</returns>
	public Task ExecuteAsync() => _client.ExecuteAsync(_command, CancellationToken.None);
	/// <summary>
	/// Executes the OData function or action.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result task.</returns>
	public Task ExecuteAsync(CancellationToken cancellationToken) => _client.ExecuteAsync(_command, cancellationToken);

	/// <summary>
	/// Executes the OData function or action and returns a single item.
	/// </summary>
	/// <returns>Execution result.</returns>
	public Task<T> ExecuteAsSingleAsync() => ExecuteAsSingleAsync(CancellationToken.None);
	/// <summary>
	/// Executes the OData function or action and returns a single item.
	/// </summary>
	/// <returns>Execution result.</returns>
	public Task<U> ExecuteAsSingleAsync<U>() => ExecuteAsSingleAsync<U>(CancellationToken.None);
	/// <summary>
	/// Executes the OData function or action and returns a single item.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result.</returns>
	public Task<T> ExecuteAsSingleAsync(CancellationToken cancellationToken) => FilterAndTypeColumnsAsync(
			_client.ExecuteAsSingleAsync(_command, cancellationToken),
			_command.SelectedColumns, _command.DynamicPropertiesContainerName);
	/// <summary>
	/// Executes the OData function or action and returns a single item.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result.</returns>
	public async Task<U> ExecuteAsSingleAsync<U>(CancellationToken cancellationToken) => (await ExecuteAsArrayAsync<U>(cancellationToken)).Single();

	/// <summary>
	/// Executes the OData function or action and returns enumerable result.
	/// </summary>
	/// <returns>Execution result.</returns>
	public Task<IEnumerable<T>> ExecuteAsEnumerableAsync() => FilterAndTypeColumnsAsync(
			_client.ExecuteAsEnumerableAsync(_command, CancellationToken.None),
			_command.SelectedColumns, _command.DynamicPropertiesContainerName);
	/// <summary>
	/// Executes the OData function or action and returns enumerable result.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result.</returns>
	public Task<IEnumerable<T>> ExecuteAsEnumerableAsync(CancellationToken cancellationToken) => FilterAndTypeColumnsAsync(
			_client.ExecuteAsEnumerableAsync(_command, cancellationToken),
			_command.SelectedColumns, _command.DynamicPropertiesContainerName);

	/// <summary>
	/// Executes the OData function or action and returns scalar result.
	/// </summary>
	/// <returns>Execution result.</returns>
	public Task<U> ExecuteAsScalarAsync<U>() => _client.ExecuteAsScalarAsync<U>(_command, CancellationToken.None);
	/// <summary>
	/// Executes the OData function or action and returns scalar result.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result.</returns>
	public Task<U> ExecuteAsScalarAsync<U>(CancellationToken cancellationToken) => _client.ExecuteAsScalarAsync<U>(_command, cancellationToken);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <returns>Execution result.</returns>
	public Task<U[]> ExecuteAsArrayAsync<U>() => ExecuteAsArrayAsync<U>(CancellationToken.None);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <returns>Execution result.</returns>
	public Task<T[]> ExecuteAsArrayAsync() => ExecuteAsArrayAsync<T>(CancellationToken.None);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result.</returns>
	public Task<U[]> ExecuteAsArrayAsync<U>(CancellationToken cancellationToken) => _client.ExecuteAsArrayAsync<U>(_command, cancellationToken);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Execution result.</returns>
	public Task<T[]> ExecuteAsArrayAsync(CancellationToken cancellationToken) => ExecuteAsArrayAsync<T>(cancellationToken);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <param name="annotations">The OData feed annotations.</param>
	/// <returns>Execution result.</returns>
	public Task<U[]> ExecuteAsArrayAsync<U>(ODataFeedAnnotations annotations) => _client.ExecuteAsArrayAsync<U>(_command, annotations, CancellationToken.None);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <param name="annotations">The OData feed annotations.</param>
	/// <returns>Execution result.</returns>
	public Task<T[]> ExecuteAsArrayAsync(ODataFeedAnnotations annotations) => ExecuteAsArrayAsync<T>(annotations, CancellationToken.None);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <param name="annotations">The OData feed annotations.</param>
	/// <returns>Execution result.</returns>
	public Task<U[]> ExecuteAsArrayAsync<U>(ODataFeedAnnotations annotations, CancellationToken cancellationToken) => _client.ExecuteAsArrayAsync<U>(_command, annotations, cancellationToken);

	/// <summary>
	/// Executes the OData function and returns an array.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <param name="annotations">The OData feed annotations.</param>
	/// <returns>Execution result.</returns>
	public Task<T[]> ExecuteAsArrayAsync(ODataFeedAnnotations annotations, CancellationToken cancellationToken) => ExecuteAsArrayAsync<T>(annotations, cancellationToken);

	/// <summary>
	/// Gets the OData command text.
	/// </summary>
	/// <returns>The command text.</returns>
	public Task<string> GetCommandTextAsync() => GetCommandTextAsync(CancellationToken.None);
	/// <summary>
	/// Gets the OData command text.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The command text.</returns>
	public async Task<string> GetCommandTextAsync(CancellationToken cancellationToken)
    {
        await _session.ResolveAdapterAsync(cancellationToken).ConfigureAwait(false);
        if (cancellationToken.IsCancellationRequested)
		{
			cancellationToken.ThrowIfCancellationRequested();
		}

		return Command.Resolve(_session).Format();
    }

    protected async Task<IEnumerable<T>> FilterAndTypeColumnsAsync(
        Task<IEnumerable<IDictionary<string, object>>> entries, IList<string> selectedColumns, string dynamicPropertiesContainerName)
    {
        var result = FilterColumns(await entries.ConfigureAwait(false), selectedColumns);
        return result?.Select(z => ConvertResult(z, dynamicPropertiesContainerName));
    }

	protected async Task<T> FilterAndTypeColumnsAsync(
		Task<IDictionary<string, object>> entry, IList<string> selectedColumns, string dynamicPropertiesContainerName) => ConvertResult(FilterColumns(await entry.ConfigureAwait(false), selectedColumns), dynamicPropertiesContainerName);

	protected async Task<Tuple<IEnumerable<T>, int>> FilterAndTypeColumnsAsync(
        Task<Tuple<IEnumerable<IDictionary<string, object>>, int>> entries, IList<string> selectedColumns, string dynamicPropertiesContainerName)
    {
        var result = await entries.ConfigureAwait(false);
        return new Tuple<IEnumerable<T>, int>(
            FilterColumns(result.Item1, selectedColumns).Select(y => ConvertResult(y, dynamicPropertiesContainerName)),
            result.Item2);
    }

	protected IEnumerable<IDictionary<string, object>> FilterColumns(IEnumerable<IDictionary<string, object>> entries, IList<string> selectedColumns) => entries?.Select(x => FilterColumns(x, selectedColumns));

	protected IDictionary<string, object> FilterColumns(IDictionary<string, object> entry, IList<string> selectedColumns)
    {
        if (entry == null || selectedColumns == null || !selectedColumns.Any())
        {
            return entry;
        }
        else
        {
            return entry.Where(x => selectedColumns.Any(y => IsSelectedColumn(x, y))).ToIDictionary();
        }
    }

    private T ConvertResult(IDictionary<string, object> result, string dynamicPropertiesContainerName)
    {
        if (!string.IsNullOrEmpty(dynamicPropertiesContainerName))
        {
            TypeCache.Register<T>(dynamicPropertiesContainerName);
        }

        if (result != null && result.Keys.Count == 1 && result.ContainsKey(FluentCommand.ResultLiteral) &&
            TypeCache.IsValue(typeof(T)) || typeof(T) == typeof(string) || typeof(T) == typeof(object))
		{
			return TypeCache.Convert<T>(result.Values.First());
		}
		else
		{
			return result.ToObject<T>(TypeCache, _dynamicResults);
		}
	}

    private bool IsSelectedColumn(KeyValuePair<string, object> kv, string columnName)
    {
        var items = columnName.Split('/');
        if (items.Count() == 1)
        {
            return _session.Settings.NameMatchResolver.IsMatch(kv.Key, columnName);
        }
        else
        {
            var item = items.First();
            return _session.Settings.NameMatchResolver.IsMatch(kv.Key, item) &&
                   (kv.Value is IDictionary<string, object> && (kv.Value as IDictionary<string, object>)
                        .Any(x => IsSelectedColumn(x, string.Join("/", items.Skip(1)))) ||
                    kv.Value is IEnumerable<object> && (kv.Value as IEnumerable<object>)
                        .Any(x => x is IDictionary<string, object> && (x as IDictionary<string, object>)
                            .Any(y => IsSelectedColumn(y, string.Join("/", items.Skip(1))))));
        }
    }
}
