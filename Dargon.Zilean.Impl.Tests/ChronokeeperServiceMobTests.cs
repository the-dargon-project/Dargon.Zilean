using System;
using NMockito;
using Xunit;

namespace Dargon.Zilean.Tests {
   public class ChronokeeperServiceMobTests : NMockitoInstance {
      [Mock] private readonly ChronokeeperService chronokeeperService = null;

      private readonly ChronokeeperServiceMob testObj;

      public ChronokeeperServiceMobTests() {
         testObj = new ChronokeeperServiceMob(chronokeeperService);
      }

      [Fact]
      public void GenerateSequentialId_DelegatesToService_Test() {
         var id = CreatePlaceholder<long>();
         When(chronokeeperService.GenerateSequentialId()).ThenReturn(id);
         AssertEquals(id, testObj.GenerateSequentialId());
         Verify(chronokeeperService).GenerateSequentialId();
         VerifyNoMoreInteractions();
      }

      [Fact]
      public void GenerateSequentialGuid_DelegatesToService_Test() {
         var guid = CreatePlaceholder<Guid>();
         When(chronokeeperService.GenerateSequentialGuid()).ThenReturn(guid);
         AssertEquals(guid, testObj.GenerateSequentialGuid());
         Verify(chronokeeperService).GenerateSequentialGuid();
         VerifyNoMoreInteractions();
      }
   }
}