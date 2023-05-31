using Ninject.Modules;
using Ninject.Extensions.NamedScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWS.ScheduledTask.SMResponseCollector.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Tws.SurveyMonkey.Data.Data;
using Tws.SurveyMonkey.Data;
using System.Configuration;
using TWS.ScheduledTask.SMResponseCollector.Config;
using TWS.ScheduledTask.SMResponseCollector.HttpClients;

namespace TWS.ScheduledTask.SMResponseCollector.Services
{
    public class ServiceModule:  NinjectModule
    {
        public override void Load()
        {
            //string con = ConfigurationManager.ConnectionStrings[0].ConnectionString;

            Bind<IIntegrationService>().To<SurveyResponseCollectorLoadService>().InSingletonScope();
            Bind<IRepository>().To<SurveyMonkeyRepository>().InSingletonScope();
            Bind<SurveyMonkeyClient>().ToSelf().InSingletonScope();
            Bind<SurveyMonkeyApiEndpoints>().ToSelf().InSingletonScope();
            
            Bind<ISqlConnectionFactory>().ToMethod(x => new SqlConnectionFactory("server=LUIS-SURFACE; database=affirstv2; Integrated Security=SSPI;trusted_connection=true;encrypt=false"));

        }
    }
}
