using System.Web;
using Dynamic_ESS.SolarForecast;
using Microsoft.AspNetCore.Mvc;

namespace Dynamic_ESS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenusController: ControllerBase
    {
        private readonly ILogger<VenusController> _logger;
        private readonly SolarForecastClient _solarForecastClient;

        public VenusController( ILogger<VenusController> logger, SolarForecastClient solarForecastClient)
        {
            _logger = logger;
            _solarForecastClient = solarForecastClient;
        }

        [HttpGet]
        public async Task<ActionResult<ESSSetpointDTO>> Setpoint()
        {
            ESSSetpointDTO result = new ESSSetpointDTO { Setpoint = 1500.0f };

            await _solarForecastClient.GetForecastAsync();
            return Ok(result);
        }
    }
}