using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Zilean {
   public class ClockMovedBackwardsException : Exception {
      public ClockMovedBackwardsException(long currentTime, long lastTime) : base(GetMessage(currentTime, lastTime)) { }

      private static string GetMessage(long currentTime, long lastTime) {
         return $"The clock has moved backwards. Current time: {currentTime}, Last time: {lastTime}. Refusing to generate unique id.";
      }
   }
}
