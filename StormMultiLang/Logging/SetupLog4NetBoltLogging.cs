using log4net;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using StormMultiLang.Write;

namespace StormMultiLang.Logging
{
    public static class SetupLog4NetBoltLogging
    {
        public static void UsingWriter(IBoltWriter writer)
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var layout = new PatternLayout("%date %-5level %logger - %message");
            layout.ActivateOptions();

            var boltAppender = new StormBoltAppender(writer) {Layout = layout};
            boltAppender.ActivateOptions();

            hierarchy.Root.AddAppender(boltAppender);
            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }
    }
}