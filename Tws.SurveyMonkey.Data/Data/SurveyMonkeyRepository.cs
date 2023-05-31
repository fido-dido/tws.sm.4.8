using Tws.SurveyMonkey.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data;
using Dapper;

namespace Tws.SurveyMonkey.Data.Data
{
    using SurveySet = NaturalEntitySet<ISMObjectId, SMSurvey, SMObjectIdComparer>;
    using SurveyCollectorSet = NaturalEntitySet<ISMObjectId, SMCollector, SMObjectIdComparer>;
    using CollectorRecipientSet = NaturalEntitySet<ISMObjectId, SMRecipient, SMObjectIdComparer>;
    using SurveyPageSet = NaturalEntitySet<ISMObjectId, SMPage, SMObjectIdComparer>;
    using SurveyQuestionSet = NaturalEntitySet<ISMObjectId, SMQuestion, SMObjectIdComparer>;
    using SurveyQuestionChoiceSet = NaturalEntitySet<ISMObjectId, SMChoice, SMObjectIdComparer>;
    public interface IRepository
    {
        SurveySet SurveySet { get; }
        SurveyPageSet SurveyPageSet { get; }
        SurveyQuestionSet SurveyQuestionSet { get; }
        SurveyQuestionChoiceSet SurveyQuestionChoiceSet { get; }
        SurveyCollectorSet SurveyCollectorSet { get; }
        CollectorRecipientSet CollectorRecipientSet { get; }    
        Task LoadSurveyMonkeyEntities();

        Task InsertResponses(IEnumerable<SMResponse> responses);

        Task MergeSMResponse(SMResponse smResponse);
        Task MergeSMMessage(SMMessage smMessage);
    }
    public class SurveyMonkeyRepository : IRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public SurveySet SurveySet { get; }
        public SurveyPageSet SurveyPageSet { get; }
        public SurveyQuestionSet SurveyQuestionSet { get; }
        public SurveyQuestionChoiceSet SurveyQuestionChoiceSet { get; }
        public SurveyCollectorSet SurveyCollectorSet { get; }
        public CollectorRecipientSet CollectorRecipientSet { get; }

        public SurveyMonkeyRepository(ISqlConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;

            SurveySet = new SurveySet(_connectionFactory, Sql.SurveyInsert, Sql.SurveySelectAll);
            SurveyCollectorSet = new SurveyCollectorSet(_connectionFactory, Sql.SurveyCollectorInsert, Sql.SurveyCollectorSellectAll);
            SurveyPageSet = new SurveyPageSet(_connectionFactory, Sql.SurveyPageInsert, Sql.SurveyPageSelectAll);
            SurveyQuestionSet = new SurveyQuestionSet(_connectionFactory, Sql.SurveyQuestionInsert, Sql.SurveyQuestionSelectAll);
            SurveyQuestionChoiceSet = new SurveyQuestionChoiceSet(_connectionFactory, Sql.SurveyQuestionChoiceInsert, Sql.SurveyQuestionChoiceSelectAll);
            CollectorRecipientSet = new CollectorRecipientSet(_connectionFactory, Sql.CollectorRecipientInsert, Sql.CollectorRecipientSelectAll);
        }

        public async Task LoadSurveyMonkeyEntities()
        {
            _logger.Info("Start Loading Sets");
            try
            {
                var tasks = new[]
                {
                    SurveySet.Load(),
                    SurveyPageSet.Load(),
                    SurveyQuestionSet.Load(),
                    SurveyQuestionChoiceSet.Load(),
                    SurveyCollectorSet.Load(),
                    CollectorRecipientSet.Load(),
                };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.Error("Error loading Survey Monkey Entities {@error}", ex);
            }
            _logger.Info("Finish Loading Sets");
        }

        public Task InsertResponses(IEnumerable<SMResponse> responses)
        {
            string sql = "INSERT INTO Users (UserName) Values (@UserName);";
            var connection = _connectionFactory.CreateConnection();

            //connection.ExecuteAsync()

            return Task.FromResult(0);
        }

        public Task MergeSMResponse(SMResponse smResponse)
        {
            return Connection.ExecuteAsync(Sql.MergeSMResponse, smResponse);
        }

        public Task MergeSMMessage(SMMessage smMessage)
        {
            return Connection.ExecuteAsync(Sql.MergeSMMessage, smMessage);
        }

        private IDbConnection Connection => _connectionFactory.CreateConnection();
    }
}

