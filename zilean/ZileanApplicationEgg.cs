using System.Net;
using Dargon.Nest.Egg;
using Dargon.Ryu;
using Dargon.Services;
using Dargon.Services.Clustering;
using ItzWarty;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Dargon.Zilean {
   public class ZileanApplicationEgg : INestApplicationEgg {
      private const int kZileanServicePort = 40001;
      private RyuContainer ryu;
      
      public static void InitializeLogging() {
         var config = new LoggingConfiguration();
         Target debuggerTarget = new DebuggerTarget() {
            Layout = "${longdate}|${level}|${logger}|${message} ${exception:format=tostring}"
         };
         Target consoleTarget = new ColoredConsoleTarget() {
            Layout = "${longdate}|${level}|${logger}|${message} ${exception:format=tostring}"
         };

#if !DEBUG
         debuggerTarget = new AsyncTargetWrapper(debuggerTarget);
         consoleTarget = new AsyncTargetWrapper(consoleTarget);
#else
         AsyncTargetWrapper a; // Placeholder for optimizing imports
#endif

         config.AddTarget("debugger", debuggerTarget);
         config.AddTarget("console", consoleTarget);

         var debuggerRule = new LoggingRule("*", LogLevel.Trace, debuggerTarget);
         config.LoggingRules.Add(debuggerRule);

         var consoleRule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
         config.LoggingRules.Add(consoleRule);

         LogManager.Configuration = config;
      }

      public NestResult Start(IEggParameters parameters) {
         InitializeLogging();

         ryu = new RyuFactory().Create();
         var clusteringConfiguration = new ClusteringConfigurationImpl(IPAddress.Loopback, kZileanServicePort, ClusteringRole.HostOnly);
         ryu.Set<ClusteringConfiguration>(clusteringConfiguration);
         ryu.Setup();
         ryu.Touch<ItzWartyProxiesRyuPackage>();
         ryu.Touch<ZileanImplRyuPackage>();
         return NestResult.Success;
      }

      public NestResult Shutdown() {
         return NestResult.Success;
      }
   }
}
