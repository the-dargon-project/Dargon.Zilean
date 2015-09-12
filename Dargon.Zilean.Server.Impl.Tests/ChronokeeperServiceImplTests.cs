using System;
using System.Configuration;
using System.Linq;
using NMockito;
using Xunit;

namespace Dargon.Zilean.Tests {
   public class ChronokeeperServiceImplTests : NMockitoInstance {
      [Mock] private readonly ChronokeeperWorker workerA = null;
      [Mock] private readonly ChronokeeperWorker workerB = null;
      [Mock] private readonly ChronokeeperWorker workerC = null;

      private readonly ChronokeeperServiceImpl testObj;

      public ChronokeeperServiceImplTests() {
         this.testObj = new ChronokeeperServiceImpl(new[] { workerA, workerB, workerC });
      }

      [Fact]
      public void GetNextRoundRobinCounter_ReturnsSequentialInteger_Test() {
         AssertEquals(0, testObj.GetNextRoundRobinCounter());
         AssertEquals(1, testObj.GetNextRoundRobinCounter());
         AssertEquals(2, testObj.GetNextRoundRobinCounter());
         AssertEquals(3, testObj.GetNextRoundRobinCounter());
         AssertEquals(4, testObj.GetNextRoundRobinCounter());
      }

      [Fact]
      public void GenerateUniqueIds_DelegatesToWorkersRoundRobin_Test() {
         When(workerA.GenerateSequentialId()).ThenReturn(100, 101);
         When(workerB.GenerateSequentialId()).ThenReturn(200, 201);
         When(workerC.GenerateSequentialId()).ThenReturn(300);

         var result = testObj.GenerateSequentialIds(5);
         AssertTrue(new long[] { 100, 200, 300, 101, 201 }.SequenceEqual(result));

         Verify(workerA, Times(2)).GenerateSequentialId();
         Verify(workerB, Times(2)).GenerateSequentialId();
         Verify(workerC, Once()).GenerateSequentialId();
         VerifyNoMoreInteractions();
      }

      [Fact]
      public void GenerateUniqueGuids_DelegatesToWorkersRoundRobin_Test() {
         Guid guid1 = Guid.NewGuid(),
              guid2 = Guid.NewGuid(),
              guid3 = Guid.NewGuid(),
              guid4 = Guid.NewGuid(),
              guid5 = Guid.NewGuid();

         When(workerA.GenerateSequentialGuid()).ThenReturn(guid1, guid4);
         When(workerB.GenerateSequentialGuid()).ThenReturn(guid2, guid5);
         When(workerC.GenerateSequentialGuid()).ThenReturn(guid3);

         var result = testObj.GenerateSequentialGuids(5);
         AssertTrue(new [] { guid1, guid2, guid3, guid4, guid5 }.SequenceEqual(result));

         Verify(workerA, Times(2)).GenerateSequentialGuid();
         Verify(workerB, Times(2)).GenerateSequentialGuid();
         Verify(workerC, Once()).GenerateSequentialGuid();
         VerifyNoMoreInteractions();
      }
   }
}
