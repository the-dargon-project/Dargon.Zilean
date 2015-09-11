using System.Diagnostics;
using Dargon.Ryu;
using Dargon.Services;
using ItzWarty.Networking;

namespace Dargon.Zilean.Client {
   public class ZileanClientApiRyuPackage : RyuPackageV1 {
      public ZileanClientApiRyuPackage() {
         Singleton<RemoteChronokeeperConfigurationFactory>();
         Singleton<RemoteChronokeeperConfiguration>(GetRemoteChronokeeperConfiguration);
         Singleton<ChronokeeperService>(GetChronokeeperService);
         Mob<RemoteChronokeeperConfigurationMob>(RyuTypeFlags.None);
      }

      public RemoteChronokeeperConfiguration GetRemoteChronokeeperConfiguration(RyuContainer ryu) {
         var configurationFactory = ryu.Get<RemoteChronokeeperConfigurationFactory>();
         return configurationFactory.Create();
      }

      public ChronokeeperService GetChronokeeperService(RyuContainer ryu) {
         var remoteChronokeeperConfiguration = ryu.Get<RemoteChronokeeperConfiguration>();
         var networkingProxy = ryu.Get<INetworkingProxy>();
         var endpoint = networkingProxy.CreateEndPoint(remoteChronokeeperConfiguration.Host, remoteChronokeeperConfiguration.Port);
         var serviceClientFactory = ryu.Get<ServiceClientFactory>();
         var serviceClient = serviceClientFactory.Remote(endpoint.ToIPEndPoint());
         return serviceClient.GetService<ChronokeeperService>();
      }
   }
}
