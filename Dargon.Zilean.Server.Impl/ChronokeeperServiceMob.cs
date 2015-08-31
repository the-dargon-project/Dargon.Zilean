using System;
using Dargon.Management;

namespace Dargon.Zilean {
   public class ChronokeeperServiceMob : ChronokeeperService {
      private readonly ChronokeeperService chronokeeperService;

      public ChronokeeperServiceMob(ChronokeeperService chronokeeperService) {
         this.chronokeeperService = chronokeeperService;
      }

      [ManagedOperation]
      public long GenerateSequentialId() => chronokeeperService.GenerateSequentialId();

      [ManagedOperation]
      public Guid GenerateSequentialGuid() => chronokeeperService.GenerateSequentialGuid();
   }
}
