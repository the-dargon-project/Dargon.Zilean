using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Zilean.Client {
   public interface RemoteChronokeeperConfiguration {
      string Host { get; }
      int Port { get; }
   }

   public class RemoteChronokeeperConfigurationImpl : RemoteChronokeeperConfiguration {
      public string Host { get; set; }
      public int Port { get; set; }
   }
}
