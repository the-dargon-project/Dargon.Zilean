using System;
using NLog;
using static Dargon.Zilean.Client.RemoteChronokeeperConfigurationConstants;

namespace Dargon.Zilean.Client {
   public class RemoteChronokeeperConfigurationFactory {
      private static readonly Logger logger = LogManager.GetCurrentClassLogger();
      private readonly SystemState systemState;

      public RemoteChronokeeperConfigurationFactory(SystemState systemState) {
         this.systemState = systemState;
      }

      public RemoteChronokeeperConfiguration Create() {
         var host = systemState.Get(kHostKey, null);
         var portString = systemState.Get(kPortKey, null);
         if (host == null) {
            logger.Warn($"Defaulting to {kDefaultHostValue} as remote chronokeeper host is undefined.");
            host = kDefaultHostValue;
         }
         int port;
         if (portString == null || !int.TryParse(portString, out port)) {
            logger.Warn($"Defaulting to {kDefaultHostValue} as remote chronokeeper port is undefined or unparsable.");
            port = kDefaultPortValue;
         }
         return new RemoteChronokeeperConfigurationImpl {
            Host = host,
            Port = port
         };
      }
   }
}