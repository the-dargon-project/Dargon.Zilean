using ItzWarty;
using NMockito;
using Xunit;

namespace Dargon.Zilean.Client.Tests {
   public class RemoteChronokeeperConfigurationMobTests : NMockitoInstance {
      [Mock] private readonly RemoteChronokeeperConfiguration configuration = null;

      private readonly RemoteChronokeeperConfigurationMob testObj;

      public RemoteChronokeeperConfigurationMobTests() {
         testObj = new RemoteChronokeeperConfigurationMob(configuration);
      }

      [Fact]
      public void RemoteHost_DelegatesToConfiguration_Test() {
         string host = CreatePlaceholder<string>();

         When(configuration.Host).ThenReturn(host);

         AssertEquals(host, testObj.RemoteHost);

         Verify(configuration).Host.Wrap();
         VerifyNoMoreInteractions();
      }


      [Fact]
      public void RemotePort_DelegatesToConfiguration_Test() {
         var port = CreatePlaceholder<int>();

         When(configuration.Port).ThenReturn(port);

         AssertEquals(port, testObj.RemotePort);

         Verify(configuration).Port.Wrap();
         VerifyNoMoreInteractions();
      }
   }
}