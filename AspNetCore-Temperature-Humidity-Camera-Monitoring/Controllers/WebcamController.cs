using System.Net.Http;
using System.Threading.Tasks;
using Domain.Configuration;
using Domain.Webcam;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AspNetCore_Temperature_Humidity_Camera_Monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebcamController : Controller
    {
        private readonly IWebcamService _webcamService;
        private readonly CameraConfiguration _cameraConfiguration;

        public WebcamController(IWebcamService webcamService, CameraConfiguration cameraConfiguration)
        {
            _webcamService = webcamService;
            _cameraConfiguration = cameraConfiguration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new FileStreamResult(await _webcamService.StreamProxy(), _cameraConfiguration.ContentType) {
                EnableRangeProcessing = true
            };
        }
    }
}