using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dynamic_ESS.Models;
using Dynamic_ESS.EnergyPrices;
using Dynamic_ESS.SolarForecast;
using Microsoft.AspNetCore.Http.Features;

namespace Dynamic_ESS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EntsoEClient _entsoEClient;
    private readonly SolarForecastClient _solarForecastClient;

    public HomeController(ILogger<HomeController> logger, EntsoEClient entsoEClient, SolarForecastClient solarForecastClient)
    {
        _logger = logger;
        _entsoEClient = entsoEClient;
        _solarForecastClient = solarForecastClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> GetPrices()
    {
        try {
            EnergyPrices.EnergyPrices model = await _entsoEClient.GetElecticityPrices( DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
            return View( model);        
        }
        catch( SolarForecastException)
        {
            return RedirectToAction( "Error");
        }
    }

    public async Task<IActionResult> GetSolarForecast()
    {
        SolarForecast.SolarForecast? solarForecast = null;//await _solarForecastClient.GetForecastAsync();
        solarForecast = await _solarForecastClient.GetForecastFromFileAsync( "SolarForecast/forecast.solar.api.json");
        return View( solarForecast);        
    }

    public async Task<IActionResult> Calculate()
    {
        AlgorithmResponseModel model = new AlgorithmResponseModel();

        EnergyPrices.EnergyPrices energyPrices = await _entsoEClient.GetElecticityPrices( DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
        foreach( EnergyPrice priceInTime in energyPrices.Prices)
        {
            float price = 0.0484f + (priceInTime.Price + 0.10880f) * 1.21f;
            model.Data.Add( new AlgorithmResponseDataItem{
                Hour = priceInTime.Time,
                Price = price
            });
        }
        
        // Set all average prices
        IEnumerable<DateTime> allDays = model.Data.Select(i=>i.Hour.Date).Distinct();
        foreach( DateTime date in allDays)
        {
            float averagePrice = model.Data.Where(i=>i.Hour.Date == date).Select( i=>i.Price).Average();
            foreach( var item in model.Data.Where(i=>i.Hour.Date == date)) {
                item.PriceDayAverage = averagePrice;
                item.PriceAboveAverage = averagePrice - item.Price;
            }
        }            

        SolarForecast.SolarForecast? solarForecast = null;//await _solarForecastClient.GetForecastAsync();
        solarForecast = await _solarForecastClient.GetForecastFromFileAsync( "SolarForecast/forecast.solar.api.json");
        foreach(SolarForecastPeriod period in solarForecast.EnergyPeriods)
        {
            DateTime hourStart = new DateTime( period.From.Year, period.From.Month, period.From.Day, period.From.Hour, 0, 0);
            AlgorithmResponseDataItem item = model.Data.FirstOrDefault( d=>d.Hour == hourStart);
            if (item != null)
                item.SolarYield = period.EnergyExpected;            
        }

        float startSoC = 65;
        float capacity = 14000;
        float runningSoC = startSoC;
        float runningCapacity = capacity * (startSoC / 100.0f);
        float loadPerHour = 400.0f;
        float batteryExpectedMax = 0;
        float chargeCapacity = 1600;
        DateTime d = DateTime.Today.Add( TimeSpan.FromHours( DateTime.Now.Hour));
        List<AlgorithmResponseDataItem> items = model.Data.Where( i=>i.Hour >= d).ToList();
        foreach( AlgorithmResponseDataItem item in items)
        {
            runningCapacity -= loadPerHour;
            runningCapacity += (item.SolarYield * 1000.0f);
            if (runningCapacity > capacity) runningCapacity = capacity;
            item.BatterySoCCalculated = (runningCapacity / capacity) * 100.0f;
            if (item.BatterySoCCalculated > batteryExpectedMax) batteryExpectedMax = item.BatterySoCCalculated;
        }

        float capacityToCharge = (100.0f - batteryExpectedMax) / 100.0f * capacity;
        List<AlgorithmResponseDataItem> workItems= new List<AlgorithmResponseDataItem>( items);
        // Remove all items after sunset
        for( int i=workItems.Count -1; (i>=0) && (workItems[i].SolarYield == 0); i--)
            workItems.RemoveAt(i);

        workItems.Sort((a, b) => a.Price.CompareTo( b.Price));
        for( int i=0; i<workItems.Count && capacityToCharge >0; i++)
        {
            float chargeInThisHour = capacityToCharge < chargeCapacity ? capacityToCharge : chargeCapacity;
            capacityToCharge -= chargeInThisHour;
            workItems[i].CapacityCharged = chargeInThisHour;
            workItems[i].BatterySoCDiffCharged = (chargeInThisHour / capacity) * 100.0f;
        }
        // Calculate new predictedSoC
        float predictedSoC = startSoC;
        float batterySoCDiffCharged = 0;
        foreach( AlgorithmResponseDataItem item in items)
        {
            item.PredictedSoCWhileCharging = item.BatterySoCCalculated + batterySoCDiffCharged;
            batterySoCDiffCharged += item.BatterySoCDiffCharged;
        }

        for( int i=0; i<items.Count; i++)
        {
            if (items[i].CapacityCharged > 0)
            {
                DateTime startCharging = items[i].Hour.Add( TimeSpan.FromHours( (chargeCapacity - items[i].CapacityCharged) / chargeCapacity));
                DateTime endCharging = items[i].Hour.AddHours(1);

                for( int j=i+1; j<items.Count && (items[j].CapacityCharged > 0); j++)
                {
                    endCharging = items[j].Hour.Add( TimeSpan.FromHours( items[j].CapacityCharged / chargeCapacity));
                    i = j;
                }

                model.ChargePeriods.Add( new AlgorithmResponseChargePeriod{
                    From= startCharging,
                    To = endCharging
                });

                Console.WriteLine( $"Period {startCharging} - {endCharging}");
            }

        }

        return View( model);        
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
