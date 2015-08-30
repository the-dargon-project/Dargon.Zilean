using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Zilean.Utilities {
   public interface TimeProxy {
      long NowCentiseconds { get; }
   }

   public class TimeProxyImpl : TimeProxy {
      private static readonly DateTime kEpoch = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      public long NowCentiseconds => (long)((DateTime.UtcNow - kEpoch).TotalSeconds * 100);
   }
}
