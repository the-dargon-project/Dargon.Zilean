using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Dargon.Zilean {
   public class ChronokeeperServiceImpl : ChronokeeperService {
      private readonly ChronokeeperWorker[] workers;
      private long roundRobinCounter = 0;

      public ChronokeeperServiceImpl(ChronokeeperWorker[] workers) {
         this.workers = workers;
      }

      internal long GetNextRoundRobinCounter() {
         return Interlocked.Increment(ref roundRobinCounter) - 1;
      }

      public long GenerateSequentialId() {
         var worker = workers[GetNextRoundRobinCounter() % workers.Length];
         return worker.GenerateSequentialId();
      }

      public Guid GenerateSequentialGuid() {
         var worker = workers[GetNextRoundRobinCounter() % workers.Length];
         return worker.GenerateSequentialGuid();
      }
   }
}
