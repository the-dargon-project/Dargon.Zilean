using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ryu;
using Dargon.Zilean.Utilities;
using ItzWarty;

namespace Dargon.Zilean {
   public class ZileanImplRyuPackage : RyuPackageV1 {
      public ZileanImplRyuPackage() {
         Singleton<ChronokeeperServiceConfiguration>(CreateChronokeeperServiceConfiguration);
         Singleton<TimeProxy, TimeProxyImpl>();
         Singleton<ChronokeeperServiceImpl>(CreateChronokeeperServiceImpl);
         LocalService<ChronokeeperService, ChronokeeperServiceImpl>(RyuTypeFlags.None);
      }

      private ChronokeeperServiceConfiguration CreateChronokeeperServiceConfiguration(RyuContainer ryu) {
         return new ChronokeeperServiceConfigurationImpl {
            DatacenterId = 0,
            WorkerCount = 1 << ChronokeeperWorkerImpl.kWorkerBits
         };
      }

      public static ChronokeeperServiceImpl CreateChronokeeperServiceImpl(RyuContainer ryu) {
         var configuration = ryu.Get<ChronokeeperServiceConfiguration>();
         var timeProxy = ryu.Get<TimeProxy>();
         var workers = Util.Generate(
            configuration.WorkerCount,
            workerId => (ChronokeeperWorker)new ChronokeeperWorkerImpl(
               new ChronokeeperWorkerConfigurationImpl {
                  DatacenterId = configuration.DatacenterId,
                  WorkerId = workerId
               }, timeProxy
            )
         );
         return new ChronokeeperServiceImpl(workers);
      }
   }
}
