using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Zilean {
   public class SequenceCounterOverflowedException : Exception {
      private const string kMessage = "Sequence counter overflowed.";
      public SequenceCounterOverflowedException() : base(kMessage) { }
   }
}
