using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PanoramicData.OData.Client;

public class MediaClient : FluentClientBase<IDictionary<string, object>, MediaClient>, IMediaClient
{
    internal MediaClient(ODataClient client, Session session, FluentCommand command = null, bool dynamicResults = false)
        : base(client, session, null, command, dynamicResults)
    {
    }

	public Task<Stream> GetStreamAsync() => GetStreamAsync(CancellationToken.None);

	public Task<Stream> GetStreamAsync(CancellationToken cancellationToken) => _client.GetMediaStreamAsync(_command, cancellationToken);

	public Task<byte[]> GetStreamAsArrayAsync() => GetStreamAsArrayAsync(CancellationToken.None);

	public async Task<byte[]> GetStreamAsArrayAsync(CancellationToken cancellationToken)
    {
        using var stream = await _client.GetMediaStreamAsync(_command, cancellationToken).ConfigureAwait(false);
        return Utils.StreamToByteArray(stream);
    }

	public Task<string> GetStreamAsStringAsync() => GetStreamAsStringAsync(CancellationToken.None);

	public async Task<string> GetStreamAsStringAsync(CancellationToken cancellationToken)
    {
        using var stream = await _client.GetMediaStreamAsync(_command, cancellationToken).ConfigureAwait(false);
        return Utils.StreamToString(stream);
    }

	public Task SetStreamAsync(Stream stream, string contentType, bool optimisticConcurrency) => SetStreamAsync(stream, contentType, optimisticConcurrency, CancellationToken.None);

	public Task SetStreamAsync(Stream stream, string contentType, bool optimisticConcurrency, CancellationToken cancellationToken) => _client.SetMediaStreamAsync(_command, stream, contentType, optimisticConcurrency, cancellationToken);

	public Task SetStreamAsync(byte[] streamContent, string contentType, bool optimisticConcurrency) => SetStreamAsync(streamContent, contentType, optimisticConcurrency, CancellationToken.None);

	public Task SetStreamAsync(byte[] streamContent, string contentType, bool optimisticConcurrency, CancellationToken cancellationToken) => _client.SetMediaStreamAsync(_command, Utils.ByteArrayToStream(streamContent), contentType, optimisticConcurrency, cancellationToken);

	public Task SetStreamAsync(string streamContent, bool optimisticConcurrency) => SetStreamAsync(streamContent, optimisticConcurrency, CancellationToken.None);

	public Task SetStreamAsync(string streamContent, bool optimisticConcurrency, CancellationToken cancellationToken) => _client.SetMediaStreamAsync(_command, Utils.StringToStream(streamContent), "text/plain", optimisticConcurrency, cancellationToken);
}
