using NMockito;
using Xunit;

namespace Dargon.Zilean.Client.Tests {
   public class RemoteChronokeeperConfigurationFactoryTests : NMockitoInstance {
      [Mock] private readonly SystemState systemState = null;
      private readonly RemoteChronokeeperConfigurationFactory testObj;

      public RemoteChronokeeperConfigurationFactoryTests() {
         testObj = new RemoteChronokeeperConfigurationFactory(systemState);
      }

      [Fact]
      public void Create_WithDefinedHostAndPort_Test() {
         var host = CreatePlaceholder<string>();
         var port = CreatePlaceholder<int>();

         When(systemState.Get(RemoteChronokeeperConfigurationConstants.kHostKey, null)).ThenReturn(host);
         When(systemState.Get(RemoteChronokeeperConfigurationConstants.kPortKey, null)).ThenReturn(port.ToString());

         var result = testObj.Create();

         Verify(systemState).Get(RemoteChronokeeperConfigurationConstants.kHostKey, null);
         Verify(systemState).Get(RemoteChronokeeperConfigurationConstants.kPortKey, null);
         VerifyNoMoreInteractions();

         AssertEquals(host, result.Host);
         AssertEquals(port, result.Port);
      }

      [Fact]
      public void Create_WithoutDefinedNorPort_Test() {
         When(systemState.Get(RemoteChronokeeperConfigurationConstants.kHostKey, null)).ThenReturn(null);
         When(systemState.Get(RemoteChronokeeperConfigurationConstants.kPortKey, null)).ThenReturn(null);

         var result = testObj.Create();

         Verify(systemState).Get(RemoteChronokeeperConfigurationConstants.kHostKey, null);
         Verify(systemState).Get(RemoteChronokeeperConfigurationConstants.kPortKey, null);
         VerifyNoMoreInteractions();

         AssertEquals(RemoteChronokeeperConfigurationConstants.kDefaultHostValue, result.Host);
         AssertEquals(RemoteChronokeeperConfigurationConstants.kDefaultPortValue, result.Port);
      }
   }
}
