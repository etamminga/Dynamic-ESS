using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Xml;
using Microsoft.Extensions.Options;

namespace Dynamic_ESS.EnergyPrices 
{
public class EntsoEClient
    {
        private ILogger<EntsoEClient> _logger;
        private readonly EntsoEClientOptions _options;
        public EntsoEClient(ILogger<EntsoEClient> logger, IOptions<EntsoEClientOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task<EnergyPrices?> GetElecticityPrices( DateTime startUtc, DateTime endUtc)
        {
            string apiKey = _options.ApiKey?.Length > 0 ? HttpUtility.UrlEncode(_options.ApiKey): "";
            EntosEAreas allAreas = new EntosEAreas();
            EntsoEArea? area = allAreas.FirstOrDefault( a=>a.Name == _options.Area);

            string documentType = "A44";
            string inDomain = area.Code;
            string outDomain = area.Code;
            string periodStart = startUtc.ToString("yyyyMMddHH") + "00";
            string periodEnd = endUtc.ToString("yyyyMMddHH") + "00";

            Uri uri = new Uri($"https://web-api.tp.entsoe.eu/api?securityToken={apiKey}&documentType={documentType}&in_Domain={inDomain}&out_Domain={outDomain}&periodStart={periodStart}&periodEnd={periodEnd}");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/xml");

            HttpResponseMessage httpResponse = await client.GetAsync(uri);
            if (httpResponse.IsSuccessStatusCode)
            {
                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogDebug( responseBody);

                EnergyPrices result = new EnergyPrices();
                result.EnergyType = EnergyType.Electricity;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml( responseBody);
                if (xmlDocument.DocumentElement?.Name == "Publication_MarketDocument") 
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager( xmlDocument.NameTable);
                    nsmgr.AddNamespace( "doc", xmlDocument.DocumentElement.NamespaceURI);
                    foreach( XmlNode timeSerie in xmlDocument.DocumentElement.SelectNodes( "doc:TimeSeries", nsmgr))
                    {
                        XmlNode periodNode = timeSerie["Period"];
                        XmlNode timeInterval = periodNode["timeInterval"];
                        DateTime timeSerieStart = DateTime.Parse( timeInterval["start"].InnerText);
                        DateTime timeSerieEnd = DateTime.Parse( timeInterval["end"].InnerText);
                        result.Resolution = periodNode["resolution"].InnerText;
                        foreach( XmlNode pointNode in periodNode.SelectNodes( "doc:Point", nsmgr))
                        {
                            int position = int.Parse(pointNode["position"].InnerText);
                            float price = float.Parse(pointNode["price.amount"].InnerText, CultureInfo.InvariantCulture);
                            price = price / 1000.0f;

                            result.Prices.Add( new EnergyPrice { Time = timeSerieStart.AddHours( position-1), Price = price });
                        }
                    }
                }
                
                return result;
            }
            return null;
        }
    }

    public enum EnergyType: int
    {
        Electricity = 0
    }

    public class EnergyPrice
    {
        public DateTime Time { get; set;}
        public float Price { get; set;}
    }

    public class EnergyPrices
    {
        public EnergyPrices()
        {
            Prices = new List<EnergyPrice>();
        }

        public EnergyType EnergyType { get; set;}
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string? CountryCode { get; set;}

        public string? Resolution { get;set; }
        public List<EnergyPrice> Prices { get; set; }
    }

    public class EntsoEClientOptions
    {
        public const string EntsoE = "EntsoE";
        public string ApiKey { get; set; }
        public string Area {get;set;}
    }
}