﻿using NLog;
using Tws.SurveyMonkey.Data.Data;
using Tws.SurveyMonkey.Data.Entities;
using TWS.ScheduledTask.SurveyMonkey.HttpClients;
using TWS.ScheduledTask.SurveyMonkey.Interfaces;
using TWS.ScheduledTask.SurveyMonkey.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TWS.ScheduledTask.SurveyMonkey.Services
{
    internal class SurveyLoadService : IIntegrationService
    {
        private readonly SurveyMonkeyClient httpClient;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository _repository;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public SurveyLoadService(SurveyMonkeyClient httpClient, IRepository repository)
        {
            this.httpClient = httpClient;
            this._repository = repository;
        }
        public async Task Run()
        {
            _logger.Info("SurveyLoadService Start Run");
            try
            {
                await _repository.LoadSurveyMonkeyEntities();
            }
            catch (Exception ex)
            {
                _logger.Error("{@ex}", ex);
            }

            var currentSurveyPage = 1;
            var surveysFromSM = httpClient.GetSurveys(currentSurveyPage, _cancellationTokenSource.Token);

            if (surveysFromSM != null && surveysFromSM.data.Count() > 0)
            {
                LoadSurveysByPage(surveysFromSM.data);
                while (surveysFromSM?.links.next != null)
                {
                    surveysFromSM = httpClient.GetSurveys(++currentSurveyPage, _cancellationTokenSource.Token);
                    if (surveysFromSM != null)
                        LoadSurveysByPage(surveysFromSM.data);
                }
            }
            _logger.Info("SurveyLoadService End Run");
        }

        private void LoadSurveysByPage(IEnumerable<Datum> surveysFromSM)
        {
            _logger.Info("LoadSurveysByPage Start");
            foreach (var survey in surveysFromSM)
            {
                LoadSurveyDetails(survey);
                
            }
            _logger.Info("LoadSurveysByPage End");
        }

        public void LoadSurveyDetails(Datum surveysFromSM)
        {
            //todo: add try/catch
            _logger.Info("LoadSurveyDetails Start");

            try
            {
                var surveyDetailsSM = httpClient.GetSurveyDetails(surveysFromSM.id, _cancellationTokenSource.Token);


                if (surveyDetailsSM != null)
                {
                    SMSurvey smSurvey = new SMSurvey(surveyDetailsSM.id, surveyDetailsSM.title, surveyDetailsSM.category,
                                                                        surveyDetailsSM.question_count, surveyDetailsSM.page_count,
                                                                        surveyDetailsSM.response_count, surveyDetailsSM.date_created,
                                                                        surveyDetailsSM.date_modified);

                    smSurvey = _repository.SurveySet.Item(smSurvey).GetAwaiter().GetResult();

                    LoadSurveyCollectors(smSurvey);

                    foreach (var page in surveyDetailsSM.pages)
                    {
                        SMPage smPage = new SMPage(page.id, smSurvey.Id, page.title, page.description, page.question_count, page.position);
                        smPage =  _repository.SurveyPageSet.Item(smPage).GetAwaiter().GetResult();

                        foreach (var question in page.questions)
                        {
                            SMQuestion smQuestion = new SMQuestion(question.id, smPage.Id, question.family, question.headings.First().heading, question.position);
                            smQuestion =  _repository.SurveyQuestionSet.Item(smQuestion).GetAwaiter().GetResult(); 

                            if (question.family == "single_choice" || question.family == "matrix")
                            {
                                foreach (var choice in question.answers.choices)
                                {
                                    SMChoice smChoice = new SMChoice(choice.id, smQuestion.Id, choice.text, choice.position);
                                    smChoice =  _repository.SurveyQuestionChoiceSet.Item(smChoice).GetAwaiter().GetResult();
                                }
                            }

                        }
                    }

                }

            }
            catch (Exception e)
            {
                _logger.Error("{@e}", e);

            }
            _logger.Info("LoadSurveyDetails End");
        }

        public void LoadSurveyCollectors(SMSurvey survey)
        {
            _logger.Info("LoadSurveyCollectors Start");
            var surveyCollectorsSM = httpClient.GetSurveyCollectors(survey.ObjectId, _cancellationTokenSource.Token);

            if (surveyCollectorsSM != null)
            {
                foreach (var collector in surveyCollectorsSM.data.Where(coll => coll.type == "email"))
                {
                    SMCollector smCollector = new SMCollector(collector.id, survey.Id, collector.name, collector.type, collector.email);
                    smCollector =  _repository.SurveyCollectorSet.Item(smCollector).GetAwaiter().GetResult();

                    LoadCollectorRecipients(smCollector);
                    LoadCollectorMessages(smCollector);
                }
            }
            _logger.Info("LoadSurveyCollectors End");
        }

        public void LoadCollectorRecipients(SMCollector smCollector)
        {
            _logger.Info("LoadCollectorRecipients Start");
            var collector = httpClient.GetCollectorRecipients(smCollector.ObjectId, _cancellationTokenSource.Token);

            foreach (var recipient in collector.data)
            {
                var dbRecipient = new SMRecipient(recipient.id, smCollector.Id, recipient.email, recipient.phone_number);
                dbRecipient =  _repository.CollectorRecipientSet.Item(dbRecipient).GetAwaiter().GetResult();
            }
            _logger.Info("LoadCollectorRecipients end");
        }

        public void LoadCollectorMessages(SMCollector smCollector)
        {
            _logger.Info("LoadCollectorMessages Start");
            var message = httpClient.GetSurveyCollectorMessages(smCollector.ObjectId, _cancellationTokenSource.Token);

            foreach(var m in message.data)
            {
                var surveyMessageDetail = httpClient.GetSurveyCollectorMessageDetails(smCollector.ObjectId, m.id, _cancellationTokenSource.Token);
                var dbMessage = new SMMessage(surveyMessageDetail.id, smCollector.Id, surveyMessageDetail.status, surveyMessageDetail.is_scheduled, surveyMessageDetail.embed_first_question, surveyMessageDetail.subject, surveyMessageDetail.body, surveyMessageDetail.type, DateTime.Now);
                _repository.MergeSMMessage(dbMessage).GetAwaiter().GetResult();
            }
            _logger.Info("LoadCollectorMessages end");
        }
    }
}
