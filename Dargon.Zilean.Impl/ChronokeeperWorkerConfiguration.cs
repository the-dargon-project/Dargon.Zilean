namespace Dargon.Zilean {
   public interface ChronokeeperWorkerConfiguration {
      int DatacenterId { get; }
      int WorkerId { get; }
   }

   public class ChronokeeperWorkerConfigurationImpl : ChronokeeperWorkerConfiguration {
      public int DatacenterId { get; set; }
      public int WorkerId { get; set; }
   }
}