using System;
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
      public void GenerateUniqueId_DelegatesToWorkersRoundRobin_Test() {
         When(workerA.GenerateSequentialId()).ThenReturn(100, 101);
         When(workerB.GenerateSequentialId()).ThenReturn(200, 201);
         When(workerC.GenerateSequentialId()).ThenReturn(300);

         AssertEquals(100, testObj.GenerateSequentialId());
         AssertEquals(200, testObj.GenerateSequentialId());
         AssertEquals(300, testObj.GenerateSequentialId());
         AssertEquals(101, testObj.GenerateSequentialId());
         AssertEquals(201, testObj.GenerateSequentialId());

         Verify(workerA, Times(2)).GenerateSequentialId();
         Verify(workerB, Times(2)).GenerateSequentialId();
         Verify(workerC, Once()).GenerateSequentialId();
         VerifyNoMoreInteractions();
      }

      [Fact]
      public void GenerateUniqueGuid_DelegatesToWorkersRoundRobin_Test() {
         Guid guid1 = Guid.NewGuid(),
              guid2 = Guid.NewGuid(),
              guid3 = Guid.NewGuid(),
              guid4 = Guid.NewGuid(),
              guid5 = Guid.NewGuid();

         When(workerA.GenerateSequentialGuid()).ThenReturn(guid1, guid4);
         When(workerB.GenerateSequentialGuid()).ThenReturn(guid2, guid5);
         When(workerC.GenerateSequentialGuid()).ThenReturn(guid3);

         AssertEquals(guid1, testObj.GenerateSequentialGuid());
         AssertEquals(guid2, testObj.GenerateSequentialGuid());
         AssertEquals(guid3, testObj.GenerateSequentialGuid());
         AssertEquals(guid4, testObj.GenerateSequentialGuid());
         AssertEquals(guid5, testObj.GenerateSequentialGuid());

         Verify(workerA, Times(2)).GenerateSequentialGuid();
         Verify(workerB, Times(2)).GenerateSequentialGuid();
         Verify(workerC, Once()).GenerateSequentialGuid();
         VerifyNoMoreInteractions();
      }
   }
}
