using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace Dynamic_ESS.Models;

public class AlgorithmResponseModel
{
    public float EnergyRemainingTillSunrise { get; set;}
    public float EnergyRequiredTillSunrise { get; set;}

    public List<AlgorithmResponseDataItem> Data { get;set;} = new List<AlgorithmResponseDataItem>();

    public List<AlgorithmResponseChargePeriod> ChargePeriods {get;set;} = new List<AlgorithmResponseChargePeriod>();
}

public class AlgorithmResponseChargePeriod
{
    public DateTime From;
    public DateTime To;
}

public class AlgorithmResponseDataItem
{
    public DateTime Hour {get;set;}
    public string X {
        get { 
            return Hour.ToString( "dd-MM-yyyy HH:00");
        }
    }

    public float Price { get;set;} =0.0f;
    public float SolarYield { get;set;} =0.0f;
    public float PriceAboveAverage { get;set;} =0.0f;
    public float PriceDayAverage { get;set;} =0.0f;
    public float BatterySoCCalculated {get;set;} =0.0f;
    public float CapacityCharged {get;set;} =0.0f;
    public float BatterySoCDiffCharged { get;set;} =0.0f;
    public float PredictedSoCWhileCharging { get;set;} =0.0f;
}
