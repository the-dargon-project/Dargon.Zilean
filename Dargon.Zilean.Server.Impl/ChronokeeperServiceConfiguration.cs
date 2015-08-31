namespace Dargon.Zilean {
   public interface ChronokeeperServiceConfiguration {
      int DatacenterId { get; }
      int WorkerCount { get; }
   }

   public class ChronokeeperServiceConfigurationImpl : ChronokeeperServiceConfiguration {
      public int DatacenterId { get; set; }
      public int WorkerCount { get; set; }
   }
}