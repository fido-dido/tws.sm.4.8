using Newtonsoft.Json.Linq;
using Ninject;
using NLog;
using System;
using System.Configuration;
using Tws.SurveyMonkey.Data;
using TWS.ScheduledTask.SMResponseCollector.Services;
using TWS.ScheduledTask.SMResponseCollector.Services;

namespace TWS.ScheduledTask.SMResponseCollector
{
    public class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly SurveyResponseCollectorLoadService  _service;
        public static void Main(string[] args)
        {
            try
            {
                Logger.Info("Start Main");


                var kernel = new StandardKernel(new ServiceModule());
                var _service = kernel.Get<SurveyResponseCollectorLoadService>();

                _service.Run().GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "End Main");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
