using System;
using BenchmarkDotNet.Running;
using PanoramicData.OData.Client.Tests;

namespace PanoramicData.OData.Client.Benchmarks
{
	internal class Program
	{
		private static void Main() =>
			//BenchmarkRunner.Run<CrmEmployee>();
			BenchmarkRunner.Run<TripPinPeople>();
	}

	public static class Utils
	{
		public static ODataClient GetClient(string metadataFilename, string responseFilename) => new(new ODataClientSettings(new Uri("http://localhost/odata"))
		{
			MetadataDocument = MetadataResolver.GetMetadataDocument(metadataFilename),
			IgnoreUnmappedProperties = true
		}.WithHttpResponses(new[] { @"..\..\..\..\..\..\..\Resources\" + responseFilename }));
	}
}
