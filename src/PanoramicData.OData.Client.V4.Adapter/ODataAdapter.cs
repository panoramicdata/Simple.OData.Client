using System;
using System.Collections.Generic;
using Microsoft.OData.Edm;
using Microsoft.Spatial;

using PanoramicData.OData.Client.Adapter;

namespace PanoramicData.OData.Client
{
	public static class V4Adapter
	{
		public static void Reference() { }
	}
}

namespace PanoramicData.OData.Client.V4.Adapter
{
	public class ODataAdapter : ODataAdapterBase
	{
		private readonly ISession _session;
		private IMetadata _metadata;

		public override AdapterVersion AdapterVersion => AdapterVersion.V4;

		public override ODataPayloadFormat DefaultPayloadFormat => ODataPayloadFormat.Json;

		public ODataAdapter(ISession session, IODataModelAdapter modelAdapter)
		{
			_session = session;
			ProtocolVersion = modelAdapter.ProtocolVersion;
			Model = modelAdapter.Model as IEdmModel;

			session.TypeCache.Converter.RegisterTypeConverter(typeof(GeographyPoint), TypeConverters.CreateGeographyPoint);
			session.TypeCache.Converter.RegisterTypeConverter(typeof(GeometryPoint), TypeConverters.CreateGeometryPoint);
		}

		public new IEdmModel Model
		{
			get => base.Model as IEdmModel;

			set
			{
				base.Model = value;
				_metadata = null;
			}
		}

		public override string GetODataVersionString() => ProtocolVersion switch
		{
			ODataProtocolVersion.V4 => "V4",
			_ => throw new InvalidOperationException($"Unsupported OData protocol version: \"{ProtocolVersion}\""),
		};

		public override IMetadata GetMetadata() =>
			// TODO: Should use a MetadataFactory here 
			_metadata ??= new MetadataCache(new Metadata(Model, _session.Settings.NameMatchResolver, _session.Settings.IgnoreUnmappedProperties, _session.Settings.UnqualifiedNameCall));

		public override ICommandFormatter GetCommandFormatter() => new CommandFormatter(_session);

		public override IResponseReader GetResponseReader() => new ResponseReader(_session, Model);

		public override IRequestWriter GetRequestWriter(Lazy<IBatchWriter> deferredBatchWriter) => new RequestWriter(_session, Model, deferredBatchWriter);

		public override IBatchWriter GetBatchWriter(IDictionary<object, IDictionary<string, object>> batchEntries) => new BatchWriter(_session, batchEntries);
	}
}