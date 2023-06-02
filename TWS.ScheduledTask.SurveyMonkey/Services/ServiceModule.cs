﻿using Ninject.Modules;
using Ninject.Extensions.NamedScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWS.ScheduledTask.SurveyMonkey.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Tws.SurveyMonkey.Data.Data;
using Tws.SurveyMonkey.Data;
using System.Configuration;
using TWS.ScheduledTask.SurveyMonkey.Config;
using TWS.ScheduledTask.SurveyMonkey.HttpClients;

namespace TWS.ScheduledTask.SurveyMonkey.Services
{
    public class ServiceModule:  NinjectModule
    {
        public override void Load()
        {
            var connectionString = ConfigurationManager.AppSettings["connectionstring"];

            Bind<IIntegrationService>().To<SurveyLoadService>().InSingletonScope();
            Bind<IRepository>().To<SurveyMonkeyRepository>().InSingletonScope();
            Bind<SurveyMonkeyClient>().ToSelf().InSingletonScope();
            Bind<SurveyMonkeyApiEndpoints>().ToSelf().InSingletonScope();
            
            Bind<ISqlConnectionFactory>().ToMethod(x => new SqlConnectionFactory(connectionString));

        }
    }
}
