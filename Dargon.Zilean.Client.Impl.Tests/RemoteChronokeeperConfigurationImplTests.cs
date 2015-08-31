using NMockito;
using Xunit;

namespace Dargon.Zilean.Client.Tests {
   public class RemoteChronokeeperConfigurationImplTests : NMockitoInstance {
      [Fact]
      public void GettersReflectSettersTest() {
         var host = CreatePlaceholder<string>();
         var port = CreatePlaceholder<int>();

         var testObj = new RemoteChronokeeperConfigurationImpl();
         testObj.Host = host;
         testObj.Port = port;

         AssertEquals(host, testObj.Host);
         AssertEquals(port, testObj.Port);
      }
   }
}
