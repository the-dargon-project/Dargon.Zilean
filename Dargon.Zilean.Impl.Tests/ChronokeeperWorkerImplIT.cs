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
   public class ChronokeeperWorkerImplIT : NMockitoInstance {
      [Fact]
      public void Run_WithPrecomputedTime_Test() {
         const int kGuidsToGenerate = 4096;

         TimeProxy timeProxy = CreateMock<TimeProxy>();
         When(timeProxy.NowCentiseconds).ThenReturn(
            Util.Generate(kGuidsToGenerate / 4, i => 0x10000000L + 20L * (i / 2) + 1)
         );
         When(timeProxy.NowCentiseconds).ThenReturn(
            Util.Generate(kGuidsToGenerate / 4, i => 0x100000000L + 20L * (i / 2) + 2)
         );
         When(timeProxy.NowCentiseconds).ThenReturn(
            Util.Generate(kGuidsToGenerate / 4, i => 0x1000000000L + 20L * (i / 2) + 3)
         );
         When(timeProxy.NowCentiseconds).ThenReturn(
            Util.Generate(kGuidsToGenerate / 4, i => 0x10000000000L + 20L * (i / 2) + 4)
         );
         RunTestHelper(timeProxy, kGuidsToGenerate);
      }

      [Fact]
      public void Run_WithWallClockTime_Test() {
         const int kGuidsToGenerate = 4096;

         TimeProxy timeProxy = new TimeProxyImpl();
         RunTestHelper(timeProxy, kGuidsToGenerate);
      }

      private void RunTestHelper(TimeProxy timeProxy, int kGuidsToGenerate) {
         ChronokeeperWorkerConfiguration configuration = new ChronokeeperWorkerConfigurationImpl {
            DatacenterId = 2,
            WorkerId = 4
         };
         var worker = new ChronokeeperWorkerImpl(configuration, timeProxy);
         var guids = Util.Generate(kGuidsToGenerate, i => worker.GenerateSequentialGuid());
         guids.ForEach(x => Debug.WriteLine(x));
         AssertTrue(guids.SequenceEqual(guids.OrderBy(x => x)));
      }
   }
}
