using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace ActivityMonitorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorController : Controller
    {
        private IMemoryCache _cache;
        private const int EXPIRATION_TIME = 5;
        private ConcurrentDictionary<string,int> activityDictionary = new ConcurrentDictionary<string, int>();

        public MonitorController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        [Route("api/activity/total")]
        public int Get()
        {
            int total = 0;
            if (_cache.TryGetValue("activityData", out activityDictionary))
            {
                var data = _cache.Get("activityData");
                activityDictionary = (ConcurrentDictionary<string, int>)data;

                total = activityDictionary
                    .Where(activity => DateTime.Parse(activity.Key).AddMinutes(EXPIRATION_TIME) > DateTime.Now)
                    .Sum(k => k.Value);
            }

            return total;
        }

        // POST 
        [HttpPost]
        [Route("api/activity")]
        public  ActionResult Activity(Activity activity)
        {
            DateTime currentTime;
            try
            {
                ConcurrentDictionary<string, int> dictionary;

                if (_cache.TryGetValue("activityData", out dictionary))
                {
                    var data = _cache.Get("activityData");
                    activityDictionary = (ConcurrentDictionary<string, int>)data;
                }
                
                activityDictionary.TryAdd(DateTime.Now.ToString(), (int) Math.Round(activity.Value,MidpointRounding.ToPositiveInfinity) );
                
                currentTime = DateTime.Now;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(EXPIRATION_TIME));

                _cache.Set("activityData", activityDictionary, cacheEntryOptions);

                return new JsonResult(new {
                    StatusCode = 200,
                    Content = "{ \n success }",
                    ContentType = "application/json"
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new {
                    StatusCode = 406,
                    Content = "{ \n error : " + ex.Message + "}",
                    ContentType = "application/json"
                });
            }
            
        }
        
    }
}
