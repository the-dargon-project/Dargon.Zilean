using NMockito;
using Xunit;

namespace Dargon.Zilean.Tests {
   public class ChronokeeperServiceConfigurationTests : NMockitoInstance {
      [Fact]
      public void GettersReflectSettersTest() {
         var datacenterId = CreatePlaceholder<int>();
         var workerCount = CreatePlaceholder<int>();

         var testObj = new ChronokeeperServiceConfigurationImpl {
            DatacenterId = datacenterId,
            WorkerCount = workerCount
         };

         AssertEquals(datacenterId, testObj.DatacenterId);
         AssertEquals(workerCount, testObj.WorkerCount);
      } 
   }
}