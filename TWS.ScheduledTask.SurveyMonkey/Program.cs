﻿using Newtonsoft.Json.Linq;
using Ninject;
using NLog;
using System;
using System.Configuration;
using Tws.SurveyMonkey.Data;
using TWS.ScheduledTask.SurveyMonkey.Services;

namespace TWS.ScheduledTask.SurveyMonkey
{
    public class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            try
            {
                Logger.Info("Start Main");


                var kernel = new StandardKernel(new ServiceModule());
                var _service = kernel.Get<SurveyLoadService>();

                _service.Run().GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                Logger.Error("{@ex}", ex);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
}
    }
}
