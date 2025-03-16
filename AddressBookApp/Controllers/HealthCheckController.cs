using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace AddressBookApp.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthCheckController : Controller
    {
        private readonly IConnectionMultiplexer _redis;

        public HealthCheckController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("redis")]
        public IActionResult CheckRedis()
        {
            try
            {
                var db = _redis.GetDatabase();
                db.StringSet("RedisTestKey", "Redis is working!", TimeSpan.FromSeconds(10));

                // Convert RedisValue to string
                string value = db.StringGet("RedisTestKey").ToString();

                return Ok(new { success = true, message = value });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

    }
}
