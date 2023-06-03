
using TWS.ScheduledTask.SMResponseCollector.Config;
using TWS.ScheduledTask.SMResponseCollector.Models;
using TWS.ScheduledTask.SMResponseCollector.Models.Response;
using TWS.ScheduledTask.SMResponseCollector.Models.Collector;
using TWS.ScheduledTask.SMResponseCollector.Models.CollectorRecipient;
using NLog;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TWS.ScheduledTask.SMResponseCollector.HttpClients
{
    public class SurveyMonkeyClient
    {
        private HttpClient _client;
        private SurveyMonkeyApiOptions _clientOptions;
        private SurveyMonkeyApiEndpoints _apiEndpoints;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public SurveyMonkeyClient(HttpClient client, SurveyMonkeyApiOptions options, SurveyMonkeyApiEndpoints apiEndpoints)
        {
            _client = client;
            _clientOptions = options;
            _apiEndpoints = apiEndpoints;

            _client.BaseAddress = new Uri(_clientOptions.BaseAddress);
            _client.Timeout = new TimeSpan(0, 0, _clientOptions.TimeOutInSeconds);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientOptions.AccessToken);
        }

        public async Task<Survey> GetSurveys(int currentPage, CancellationToken cancellationToken)
        {
            _logger.Info("GetSurveys Start");

            string requestNumberOfDaysToLoad = GetRequestDateRangeByNumDays();
            string requestDateRangeParams = string.IsNullOrEmpty(requestNumberOfDaysToLoad) && ValidSurveyDateRangeParams() ? SetRequestDateParams() : requestNumberOfDaysToLoad;
            string requestPageParams = ValidSurveyParams(currentPage) ? SetRequestPageParams(currentPage) : string.Empty;

            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            System.Net.Http.WebRequestHandler handler = new WebRequestHandler();
            using (HttpClient httpClient = new HttpClient(handler, true))
            {
                httpClient.BaseAddress = new Uri(_clientOptions.BaseAddress);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientOptions.AccessToken);
                var request = $"/{_clientOptions.ApiVersion}/{_apiEndpoints.Surveys}?{requestPageParams}&{requestDateRangeParams}";

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    Console.WriteLine(("GetSurveyDetails Start Request"));

                    response = httpClient.GetAsync(request).Result;

                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Survey>(content);

                    return result;

                    //using (Stream stream = httpClient.GetStreamAsync(request).Result)
                    //{
                    //    var result = new Survey();// stream.ReadAndDeserializeFromJson<Survey>();
                    //    _logger.Info("GetSurveys End: {@result}", result);
                    //    return result;

                    //}
                }
                catch (WebException ex)
                {
                    _logger.Error("{@ex}", ex);
                }
                catch (Exception ex)
                {

                    _logger.Error("- There was a problem GetSurveys{@ex} ", ex);
                    throw;
                }

            }
            return null;
        }

        public async Task<SurveyResponse> GetSurveyResponses(string surveyId, CancellationToken cancellationToken)
        {
            _logger.Info("GetSurveyResponses End");
            WebRequestHandler handler = new WebRequestHandler();
            using (HttpClient httpClient = new HttpClient(handler, true))
            {
                httpClient.BaseAddress = new Uri(_clientOptions.BaseAddress);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientOptions.AccessToken);
                var request = $"/{_clientOptions.ApiVersion}/{_apiEndpoints.SurveyReponse.Replace("{id}", surveyId.ToString())}";

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    Console.WriteLine(("GetSurveyCollectors Start Request"));

                    response = httpClient.GetAsync(request).Result;

                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<SurveyResponse>(content);

                    return result;

                    //using (Stream stream = httpClient.GetStreamAsync(request).Result)
                    //{
                    //    var result = new SurveyResponse();// stream.ReadAndDeserializeFromJson<SurveyResponse>();
                    //    _logger.Info("GetSurveyResponses End: {@result}", result);
                    //    return result;

                    //}
                }
                catch (WebException ex)
                {
                    _logger.Error("{@ex}", ex);
                }
                catch (Exception ex)
                {

                    _logger.Error("- There was a problem GetSurveyCollectors{@ex} ", ex);
                    throw;
                }

            }
            return null;
        }

        private string SetRequestDateParams() { return $"start_modified_at={_apiEndpoints.SurveyStartModifiedAt}&end_modified_at={_apiEndpoints.SurveyEndModifiedAt}";  }
        private string SetRequestPageParams(int currentPage) { return $"per_page={_apiEndpoints.SurveysPerPageParam}&page={currentPage}"; }
        private string GetRequestDateRangeByNumDays()
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(_apiEndpoints.SurveyNumberOfDaysToLoad) && int.TryParse(_apiEndpoints.SurveyNumberOfDaysToLoad, out var numberOfDays))
            {
                var startDate = DateTime.Now.AddDays(numberOfDays * -1);

                result = $"start_modified_at={startDate.ToString("yyyy-MM-dd")}T00:00:00&end_modified_at={DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}T00:00:00";
            }

            return result;
        }
        private bool ValidSurveyDateRangeParams()
        {
            if (string.IsNullOrEmpty(_apiEndpoints.SurveyStartModifiedAt) || string.IsNullOrEmpty(_apiEndpoints.SurveyEndModifiedAt))
            {
                return false;
            }
            //todo: validate that they params are good dates

            return true;
        }
        private bool ValidSurveyParams(int currentPage)
        {
            if (!string.IsNullOrEmpty(_apiEndpoints.SurveysPerPageParam) && currentPage > 0)
            {
                if (int.TryParse(_apiEndpoints.SurveysPerPageParam, out var surveysPerPage))
                {
                    return true;
                }
                return false;
            }

            return true;
        }
    }
}

