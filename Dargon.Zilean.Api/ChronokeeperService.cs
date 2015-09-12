using System;
using System.Runtime.InteropServices;

namespace Dargon.Zilean {
   [Guid("7C5DF5D6-9251-4F4D-A5BA-235CEEE8C300")]
   public interface ChronokeeperService : Chronokeeper {
      long[] GenerateSequentialIds(int count);
      Guid[] GenerateSequentialGuids(int count);
   }

   public interface Chronokeeper {
      long GenerateSequentialId();
      Guid GenerateSequentialGuid();
   }
}
