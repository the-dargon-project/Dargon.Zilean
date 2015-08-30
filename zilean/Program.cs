using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dargon.Zilean {
   public static class Program {
      public static void Main(string[] args) {
         new ZileanApplicationEgg().Start(null);

         var countdownLatch = new CountdownEvent(1);
         countdownLatch.Wait();
      }
   }
}
