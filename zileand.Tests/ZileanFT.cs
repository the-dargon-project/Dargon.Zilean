using System.Diagnostics;
using Dargon.Management.Server;
using Dargon.Ryu;
using Dargon.Services;
using Dargon.Zilean.Client;
using ItzWarty;
using NMockito;
using Xunit;

namespace Dargon.Zilean.Tests {
   public class ZileanFT : NMockitoInstance {
      [Fact]
      public void Run() {
         ZileanApplicationEgg.InitializeLogging();

         var serverHatchling = new ZileanApplicationEgg();
         serverHatchling.Start(null);

         var systemState = CreateMock<SystemState>();
         var managementServer = CreateMock<ILocalManagementServer>();

         var clientRyu = new RyuFactory().Create();
         clientRyu.Set(systemState);
         clientRyu.Set(managementServer);
         clientRyu.Touch<ItzWartyProxiesRyuPackage>();
         clientRyu.Touch<ServicesRyuPackage>();
         clientRyu.Touch<ZileanClientApiRyuPackage>();

         var chronokeeper = clientRyu.Get<ChronokeeperService>();
         Debug.WriteLine("Got sequential id: " + chronokeeper.GenerateSequentialId());
         Debug.WriteLine("Got sequential guid: " + chronokeeper.GenerateSequentialGuid());

         serverHatchling.Shutdown();
      }
   }
}
