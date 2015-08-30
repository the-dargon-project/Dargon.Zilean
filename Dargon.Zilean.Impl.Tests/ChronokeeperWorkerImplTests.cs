using System;
using System.Diagnostics;
using Dargon.Zilean.Utilities;
using ItzWarty;
using NMockito;
using Xunit;

namespace Dargon.Zilean.Tests {
   public class ChronokeeperWorkerImplTests : NMockitoInstance {
      private const int kWorkerId = 4;
      private const int kDatacenterId = 2;

      private const long kPreStartTime = 512;
      private const long kStartTime = 1024;

      [Mock] private readonly ChronokeeperWorkerConfiguration configuration = null;
      [Mock] private readonly TimeProxy timeProxy = null;
      private readonly ChronokeeperWorkerImpl testObj;

      public ChronokeeperWorkerImplTests() {
         this.testObj = new ChronokeeperWorkerImpl(configuration, timeProxy);

         When(configuration.WorkerId).ThenReturn(kWorkerId);
         When(configuration.DatacenterId).ThenReturn(kDatacenterId);
      }

      [Fact]
      public void GenerateUniqueId_HappyPathTest() {
         When(timeProxy.NowCentiseconds).ThenReturn(kStartTime);

         var firstId = testObj.GenerateSequentialId();
         var secondId = testObj.GenerateSequentialId();

         Debug.WriteLine($"Got ids: {firstId} {secondId}.");

         Verify(timeProxy, Times(2)).NowCentiseconds.Wrap();
         Verify(configuration, Times(2)).WorkerId.Wrap();
         Verify(configuration, Times(2)).DatacenterId.Wrap();
         VerifyNoMoreInteractions();

         AssertEquals(0x100044000, firstId);
         AssertEquals(0x100044001, secondId);
      }

      [Fact]
      public void GenerateUniqueId_ClocksMovedBackwards_ThrowsTest() {
         When(timeProxy.NowCentiseconds).ThenReturn(kStartTime, kPreStartTime);

         var firstId = testObj.GenerateSequentialId();
         Debug.WriteLine($"Got initial id: {firstId}.");
         Verify(timeProxy, Once()).NowCentiseconds.Wrap();
         Verify(configuration, Once()).WorkerId.Wrap();
         Verify(configuration, Once()).DatacenterId.Wrap();
         VerifyNoMoreInteractions();

         AssertThrows<ClockMovedBackwardsException>(() => testObj.GenerateSequentialId());
         Verify(timeProxy, Once()).NowCentiseconds.Wrap();
         VerifyNoMoreInteractions();
      }

      [Fact]
      public void GenerateUniqueId_SequenceCounterOverflowed_ThrowsTest() {
         When(timeProxy.NowCentiseconds).ThenReturn(kStartTime);

         for (var i = 0; i < (1 << ChronokeeperWorkerImpl.kSequenceBits); i++) {
            testObj.GenerateSequentialId();
         }
         ClearInteractions();

         AssertThrows<SequenceCounterOverflowedException>(() => testObj.GenerateSequentialId());
         Verify(timeProxy, Once()).NowCentiseconds.Wrap();
         VerifyNoMoreInteractions();
      }
   }
}
