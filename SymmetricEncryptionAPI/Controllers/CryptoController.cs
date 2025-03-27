using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SymmetricEncryptionApp.Services;
using System.Diagnostics;
using System.Reflection;

namespace SymmetricEncryptionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly CryptoServiceFactory _cryptoServiceFactory;

        public CryptoController(CryptoServiceFactory cryptoServiceFactory)
        {
            _cryptoServiceFactory = cryptoServiceFactory;
        }

        // 1. Get all available encrypto services
        [HttpGet("methods")]
        public IActionResult GetMethods()
        {
            var methods = _cryptoServiceFactory.GetAvailableServices();

            return Ok(methods);
        }

        // 2. Generate Key/IV for a chosen method
        [HttpGet("generateKeyIV")]
        public IActionResult GenerateKeyAndIV([FromQuery] string methodName)
        {
            var service = _cryptoServiceFactory.GetServiceByName(methodName);
            var (key, iv) = service.GenerateKeyIV();
            return Ok(new { key, iv });
        }

        // 3. create request models for encrypt 
        public class EncryptRequest
        {
            public string MethodName { get; set; }
            public string Key { get; set; }
            public string IV { get; set; }
            public string PlainText { get; set; }
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] EncryptRequest request)
        {
            var service = _cryptoServiceFactory.GetServiceByName(request.MethodName);
            var stopwatch = Stopwatch.StartNew();  // Start measuring time

            var cipherText = service.Encrypt(request.PlainText, request.Key, request.IV);

            stopwatch.Stop(); // Stop measuring

            // to return both cipghertext and time, creating an anonymous type in C#.
            // An anonymous type is a simple class created on the fly without explicitly defining a class.
            // It is useful for encapsulating a set of read-only properties into a single object.
            return Ok(new { cipherText, elapsedTimeMs = stopwatch.ElapsedMilliseconds });
        }

        // 4. create request models for decrypt 
        public class DecryptRequest
        {
            public string MethodName { get; set; }
            public string Key { get; set; }
            public string IV { get; set; }
            public string CipherText { get; set; }
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] DecryptRequest request)
        {
            var service = _cryptoServiceFactory.GetServiceByName(request.MethodName);
            var stopwatch = Stopwatch.StartNew();  // Start measuring time

            var plainText = service.Decrypt(request.CipherText, request.Key, request.IV);

            stopwatch.Stop();

            return Ok(new { plainText, elapsedTimeMs = stopwatch.ElapsedMilliseconds });
        }

    }
}
