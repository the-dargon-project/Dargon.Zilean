using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Zilean.Client {
   public static class RemoteChronokeeperConfigurationConstants {
      public const string kHostKey = "dargon.zilean.remote_host";
      public const string kDefaultHostValue = "localhost";

      public const string kPortKey = "dargon.zilean.remote_port";
      public const int kDefaultPortValue = 40001;
   }
}
