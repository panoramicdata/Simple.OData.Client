using PanoramicData.OData.Client.Tests.Entities;
using Xunit;

namespace PanoramicData.OData.Client.Tests.Core
{
    public class DynamicContainerTests
    {
        private static ITypeCache _typeCache => TypeCaches.TypeCache("test", null);

        [Fact]
        public void ContainerName()
        {
			_typeCache.Register<Animal>();

            Assert.Equal("DynamicProperties", _typeCache.DynamicContainerName(typeof(Animal)));
        }

        [Fact]
        public void ExplicitContainerName()
        {
			_typeCache.Register<Animal>("Foo");

            Assert.Equal("Foo", _typeCache.DynamicContainerName(typeof(Animal)));
        }

        [Fact]
        public void SubTypeContainerName()
        {
			_typeCache.Register<Animal>();

            Assert.Equal("DynamicProperties", _typeCache.DynamicContainerName(typeof(Mammal)));
        }
    }
}