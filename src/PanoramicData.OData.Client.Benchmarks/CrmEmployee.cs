using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Xunit;

namespace PanoramicData.OData.Client.Benchmarks
{
	public class The_employee
	{
		public int The_employeenumber { get; set; }
	}

	public class CrmEmployee
	{
		[Benchmark]
		public async static Task GetAll()
		{
			var result = await Utils.GetClient("crm_schema.xml", "crm_result_10.json")
				.For<The_employee>()
				.FindEntriesAsync();

			Assert.Equal(10, result.ToList().Count);
		}

		[Benchmark]
		public async static Task GetSingle()
		{
			var result = await Utils.GetClient("crm_schema.xml", "crm_result_1.json")
				.For<The_employee>()
				.Filter(x => x.The_employeenumber == 123456)
				.FindEntryAsync();

			Assert.Equal(123456, result.The_employeenumber);
		}
	}
}