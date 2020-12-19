using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Configuration;
using Domain.Webcam;

namespace Services
{
    public class WebcamService : IWebcamService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly CameraConfiguration _cameraConfiguration;

        public WebcamService(CameraConfiguration cameraConfiguration)
        {
            _cameraConfiguration = cameraConfiguration;
        }

        public async Task<Stream> StreamProxy()
        {
            var stream = await _httpClient.GetStreamAsync($"{_cameraConfiguration.Host}:{_cameraConfiguration.Port}");
            return stream;
        }
    }
}