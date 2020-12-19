using System.IO;
using System.Threading.Tasks;

namespace Domain.Webcam
{
    public interface IWebcamService
    {
        Task<Stream> StreamProxy();
    }
}