using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Services.Providers;
using System.Linq;
using System.ServiceModel.Web;
using ActionProviderImplementation;
using NorthwindModel;
using PanoramicData.OData.NorthwindModel.Entities;

namespace PanoramicData.OData.NorthwindModel
{
	public class NorthwindService : EntityFrameworkDataService<NorthwindContext>, IServiceProvider
	{
		public static void InitializeService(DataServiceConfiguration config)
		{
			config.SetEntitySetAccessRule("*", EntitySetRights.All);
			config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
			config.SetServiceActionAccessRule("*", ServiceActionRights.Invoke);
			config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
			config.UseVerboseErrors = true;
		}

		public object GetService(Type serviceType)
		{
			if (typeof(IDataServiceActionProvider) == serviceType)
			{
				return new EntityFrameworkActionProvider(CurrentDataSource);
			}
			return null;
		}

		protected override void HandleException(HandleExceptionArgs args) => base.HandleException(args);

		[WebGet]
		public int ParseInt(string number) => int.Parse(number);

		[WebGet]
		public string ReturnString(string text) => text;

		[WebGet]
		public IQueryable<int> ReturnIntCollection(int count)
		{
			var numbers = new List<int>();
			for (var index = 1; index <= count; index++)
			{
				numbers.Add(index);
			}
			return numbers.AsQueryable();
		}

		[WebGet]
		public long PassThroughLong(long number) => number;

		[WebGet]
		public DateTime PassThroughDateTime(DateTime dateTime) => dateTime;

		[WebGet]
		public Guid PassThroughGuid(Guid guid) => guid;

		[WebGet]
		public IQueryable<Address> ReturnAddressCollection(int count)
		{
			var address = new Address { City = "Oslo", Country = "Norway", Region = "Oslo", PostalCode = "1234" };
			var addresses = new List<Address>();
			for (var index = 1; index <= count; index++)
			{
				addresses.Add(address);
			}
			return addresses.AsQueryable();
		}
	}
}

