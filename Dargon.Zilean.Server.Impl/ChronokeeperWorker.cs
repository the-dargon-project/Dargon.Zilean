using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Dargon.Zilean.Utilities;
using ItzWarty;

namespace Dargon.Zilean {
   public interface ChronokeeperWorker : Chronokeeper { }

   public class ChronokeeperWorkerImpl : ChronokeeperWorker {
      internal const int kSequenceOffset = 0;
      internal const int kSequenceBits = 12;
      internal const ulong kSequenceMask = (~(-1L << kSequenceBits)) << kSequenceOffset;

      internal const int kWorkerOffset = kSequenceOffset + kSequenceBits;
      public const int kWorkerBits = 5;
      internal const ulong kWorkerMask = (~(-1L << kWorkerBits)) << kWorkerOffset;

      internal const int kDatacenterOffset = kWorkerOffset + kWorkerBits;
      internal const int kDatacenterBits = 5;
      internal const ulong kDatacenterMask = (~(-1L << kDatacenterBits)) << kDatacenterOffset;

      private const int kTimeOffset = kDatacenterOffset + kDatacenterBits;

      private readonly object synchronization = new object();
      private readonly ChronokeeperWorkerConfiguration configuration;
      private readonly TimeProxy timeProxy;

      private long lastTime = -1;
      private int sequenceCounter = 0;

      public ChronokeeperWorkerImpl(ChronokeeperWorkerConfiguration configuration, TimeProxy timeProxy) {
         this.configuration = configuration;
         this.timeProxy = timeProxy;
      }

      internal int GenerateUniqueNumeric(long currentTime) {
         lock (synchronization) {

            if (currentTime < lastTime) {
               throw new ClockMovedBackwardsException(currentTime, lastTime);
            }

            if (currentTime > lastTime) {
               sequenceCounter = 0;
            } else {
               sequenceCounter++;
               if ((((ulong)sequenceCounter << kSequenceOffset) & kSequenceMask) == 0) {
                  throw new SequenceCounterOverflowedException();
               }
            }

            lastTime = currentTime;

            return ((int)configuration.DatacenterId << kDatacenterOffset) |
                   ((int)configuration.WorkerId << kWorkerOffset) |
                   ((int)sequenceCounter << kSequenceOffset);
         }
      }

      public long GenerateSequentialId() {
         long currentTime = timeProxy.NowCentiseconds;
         return (currentTime << kTimeOffset) | (long)GenerateUniqueNumeric(currentTime);
      }

      public Guid GenerateSequentialGuid() {
         // Guids look like: 001f2b5b-5c44-4000-9316-4b79484e362c
         //                  a        b    c    ...
         // 
         // A is more significant than B, B is more significant than C when we 
         // are comparing guids.
         // 
         // When generating a unique guid, we simply generate a unique int64 and
         // copy it into the significant bits of the guid.
         long uniqueId = GenerateSequentialId();
         var uniqueNumericBytes = BitConverter.GetBytes(uniqueId);
         var guidBytes = Guid.NewGuid().ToByteArray();
         
         // Set Guid.A to high quad
         Array.Copy(uniqueNumericBytes, 4, guidBytes, 0, 4);

         // Set Guid.B to middle short
         Array.Copy(uniqueNumericBytes, 2, guidBytes, 4, 2);

         // Set Guid.C to low short
         Array.Copy(uniqueNumericBytes, 0, guidBytes, 6, 2);

         return new Guid(guidBytes);
      }
   }
}
