using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dargon.Zilean.Utilities;
using ItzWarty;
using NMockito;
using Xunit;

namespace Dargon.Zilean.Tests {
   public class ChronokeeperServiceImplIT : NMockitoInstance {
      [Fact]
      public void Run() {
         var timeProxy = new TimeProxyImpl();
         var workerCount = 1 << ChronokeeperWorkerImpl.kWorkerBits;
         ChronokeeperWorker[] workers = Util.Generate(
            workerCount, 
            workerId => new ChronokeeperWorkerImpl(
               new ChronokeeperWorkerConfigurationImpl {
                  DatacenterId = 0,
                  WorkerId = workerId
               },
               timeProxy
            )
         );
         var testObj = new ChronokeeperServiceImpl(workers);
         const int kSequentialIdCount = 1000;
         var sequentialIds = new long[kSequentialIdCount];
         for (var i = 0; i < kSequentialIdCount; i++) {
            sequentialIds[i] = testObj.GenerateSequentialId();
            if (i % workerCount == workerCount - 1) {
               Thread.Sleep(10); // 1 centisecond = chronokeeper clock precision
            }
         }
         sequentialIds.ForEach(x => Debug.WriteLine(x.ToString("X")));
         AssertTrue(sequentialIds.SequenceEqual(sequentialIds.OrderBy(x => x)));
      }
   }
}
