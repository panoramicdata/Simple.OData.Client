using System;
using System.Collections.Generic;
using System.Spatial;

using Microsoft.Data.Edm;

using PanoramicData.OData.Client.Adapter;

namespace PanoramicData.OData.Client
{
	public static class V3Adapter
	{
		public static void Reference() { }
	}
}

namespace PanoramicData.OData.Client.V3.Adapter
{
	public class ODataAdapter : ODataAdapterBase
	{
		private readonly ISession _session;
		private IMetadata _metadata;

		public override AdapterVersion AdapterVersion => AdapterVersion.V3;

		public override ODataPayloadFormat DefaultPayloadFormat => ODataPayloadFormat.Atom;

		public ODataAdapter(ISession session, IODataModelAdapter modelAdapter)
		{
			_session = session;
			ProtocolVersion = modelAdapter.ProtocolVersion;
			Model = modelAdapter.Model as IEdmModel;

			session.TypeCache.Converter.RegisterTypeConverter(typeof(GeographyPoint), TypeConverters.CreateGeographyPoint);
			session.TypeCache.Converter.RegisterTypeConverter(typeof(GeometryPoint), TypeConverters.CreateGeometryPoint);
			session.TypeCache.Converter.RegisterTypeConverter(typeof(DateTime), TypeConverters.ConvertToEdmDate);
			session.TypeCache.Converter.RegisterTypeConverter(typeof(DateTimeOffset), TypeConverters.ConvertToEdmDate);
		}

		public new IEdmModel Model
		{
			get => base.Model as IEdmModel;

			set
			{
				base.Model = value;
				// Ensure we replace the cache on change of model
				_metadata = null;
			}
		}

		public override string GetODataVersionString() => ProtocolVersion switch
		{
			ODataProtocolVersion.V1 => "V1",
			ODataProtocolVersion.V2 => "V2",
			ODataProtocolVersion.V3 => "V3",
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