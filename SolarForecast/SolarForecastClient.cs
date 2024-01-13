using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamic_ESS.SolarForecast
{
    public class SolarForecastClient
    {
        private ILogger<SolarForecastClient> _logger;
        private readonly SolarForecastOptions _options;
        public SolarForecastClient(ILogger<SolarForecastClient> logger, IOptions<SolarForecastOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        private async Task<JObject> GetDataFromAPI( Uri uri)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            
            HttpResponseMessage httpResponse = await client.GetAsync(uri);

            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogDebug( responseBody);
            return JObject.Parse( responseBody);
        }

        private SolarForecast ParseResponseObject(JObject responseObject)
        {
            JObject messageObject = (JObject)responseObject["message"];
            if (messageObject != null)
            {
                int code = messageObject["code"].Value<int>();
                string messageType = messageObject["type"].Value<string>();
                string messageText = messageObject["text"].Value<string>();

                if (code != 0)
                {
                    throw new SolarForecastException { Code=code, Type=messageType, Text=messageText};
                }
            }

            JObject resultObject = (JObject)responseObject["result"];
            if (resultObject != null)
            {
                SolarForecast result = new SolarForecast( DateTime.Now);

                JObject wattsObject = (JObject)resultObject["watts"];
                if (wattsObject != null)
                {
                    DateTime lastPeriodStart = DateTime.MinValue;
                    foreach( JProperty period in wattsObject.Properties())
                    {
                        DateTime periodStart = DateTime.ParseExact( period.Name, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        long wattsThisPeriod = period.Value.Value<long>();
                        if (lastPeriodStart != DateTime.MinValue)
                        {
                            result.EnergyPeriods.Add( new SolarForecastPeriod{
                                EnergyExpected = wattsThisPeriod / 1000.0f,
                                From = lastPeriodStart,
                                To = periodStart
                            });
                        }
                        lastPeriodStart = periodStart;
                    }
                }
                return result;
            }
            return null;
        }

        public async Task<SolarForecast?> GetForecastFromFileAsync( string fileName)
        {
            JObject responseObject = null;

            using (StreamReader file = File.OpenText( fileName))
                using (JsonReader reader = new JsonTextReader( file))
                    responseObject = (JObject)JToken.ReadFrom( reader);

            SolarForecast result = ParseResponseObject( responseObject);
            return result;            
        }

        public async Task<SolarForecast?> GetForecastAsync()
        {
            string apiKey = _options.ApiKey?.Length > 0 ? HttpUtility.UrlEncode(_options.ApiKey) +"/": "";

            Uri uri = new Uri($"https://api.forecast.solar/{apiKey}estimate/{_options.Latitude}/{_options.Longitude}/{_options.Declination}/{_options.Azimuth}/{_options.CapacityInstalledKWP}");
            JObject responseObject = await GetDataFromAPI( uri);
            SolarForecast result = ParseResponseObject( responseObject);
            return result;            
        }
    }

    public class SolarForecast
    {
        private readonly DateTime _generatedAt;
        private readonly List<SolarForecastPeriod> _energyPeriods;
        public SolarForecast( DateTime generatedAt)
        {
            _generatedAt = generatedAt;
            _energyPeriods = new List<SolarForecastPeriod>();
        }

        public DateTime GeneratedAt { get {return _generatedAt; } }
        public List<SolarForecastPeriod> EnergyPeriods { get {return _energyPeriods; } }

    }
    public class SolarForecastPeriod
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public float EnergyExpected { get; set; }
    }

    public class SolarForecastException: ApplicationException
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }

    public class SolarForecastOptions
    {
        public const string SolarForecast = "SolarForecast";
        public string ApiKey { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Azimuth { get; set; }
        public float Declination { get; set; }
        public float CapacityInstalledKWP { get; set; }
    }
}