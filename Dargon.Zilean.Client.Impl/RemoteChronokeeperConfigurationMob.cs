using Dargon.Management;

namespace Dargon.Zilean.Client {
   public class RemoteChronokeeperConfigurationMob {
      private readonly RemoteChronokeeperConfiguration configuration;

      public RemoteChronokeeperConfigurationMob(RemoteChronokeeperConfiguration configuration) {
         this.configuration = configuration;
      }

      [ManagedProperty]
      public string RemoteHost => configuration.Host;

      [ManagedProperty]
      public int RemotePort => configuration.Port;
   }
}