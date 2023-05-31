using NLog;
using Tws.SurveyMonkey.Data.Data;
using Tws.SurveyMonkey.Data.Entities;
using TWS.ScheduledTask.SMResponseCollector.HttpClients;
using TWS.ScheduledTask.SMResponseCollector.Interfaces;
using TWS.ScheduledTask.SMResponseCollector.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TWS.ScheduledTask.SMResponseCollector.Services
{
    internal class SurveyResponseCollectorLoadService : IIntegrationService
    {
        private readonly SurveyMonkeyClient httpClient;
        private readonly IRepository _repository;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public SurveyResponseCollectorLoadService(SurveyMonkeyClient httpClient, IRepository repository)
        {
            this.httpClient = httpClient;
            this._repository = repository;

        }
        public async Task Run()
        {
            _logger.Info("SurveyResponseCollectorLoadService Start Run");
            try
            {
                await _repository.LoadSurveyMonkeyEntities();
            }
            catch (Exception ex)
            {
                _logger.Error("{@ex}", ex);
            }
           

            var currentSurveyPage = 1;
            var surveysFromSM = await httpClient.GetSurveys(currentSurveyPage, _cancellationTokenSource.Token);

            if (surveysFromSM != null && surveysFromSM.data.Count() > 0)
            {
                LoadResponsesByPage(surveysFromSM.data);
                while (surveysFromSM?.links.next != null)
                {
                    surveysFromSM = await httpClient.GetSurveys(++currentSurveyPage, _cancellationTokenSource.Token);
                    if (surveysFromSM != null)
                        LoadResponsesByPage(surveysFromSM.data);
                }
            }
            _logger.Info("SurveyResponseCollectorLoadService End Run");
        }

        private async void LoadResponsesByPage(IEnumerable<Datum> surveysFromSM)
        {
            _logger.Info("SurveyResponseCollectorLoadService LoadResponsesByPage Start");
            foreach (var survey in surveysFromSM)
            {
                await LoadResponse(survey);
            }
            _logger.Info("SurveyResponseCollectorLoadService LoadResponsesByPage End");
        }

        public async Task LoadResponse(Datum surveysFromSM)
        {
            var surveyReponsesSM = await httpClient.GetSurveyResponses(surveysFromSM.id, _cancellationTokenSource.Token);

            if (surveyReponsesSM != null)
            {
                foreach (var response in surveyReponsesSM.data)
                {
                    foreach (var page in response.pages)
                        foreach (var question in page.questions)
                            foreach (var answer in question.answers) {
                               SMResponse dbReponse = new SMResponse(response.id, response.survey_id, response.collector_id, response.recipient_id, page.id, question.id, answer.choice_id, answer.text, response.ip_address, response.response_status);
                                _repository.MergeSMResponse(dbReponse).GetAwaiter().GetResult();
                            }
                 }
            }
        }
    }
}
