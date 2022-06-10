using ActivityMonitorAPI;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ActivityMonitor.UNIT_TESTS
{
    public class Activity_UnitTest 
    {
        private dynamic controller;
        private IMemoryCache _cache;
        public Activity_UnitTest()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            controller = new ActivityMonitorAPI.Controllers.MonitorController(_cache);

        }

        [Fact]
        public void Add_Activity_With_Integer_Value()
        {
            Activity activity = new Activity()
            {
                Value = 50
            };

            controller.Activity(activity);
            var result = controller.Get();

            Assert.Equal(50,result);
        }

        [Fact]
        public void Add_Activity_With_Decimal_Value()
        {
            Activity activity = new Activity()
            {
                Value = 5.8F
            };

            controller.Activity(activity);
            var result = controller.Get();

            Assert.Equal(6, result);
        }

        [Fact]
        public void Add_Activity_Multiple_Values()
        {
            Activity activity = new Activity()
            {
                Value = 20
            };

            controller.Activity(activity);
            var result = controller.Get();

            Assert.Equal(20, result);

            activity = new Activity()
            {
                Value = 15
            };

            controller.Activity(activity);
            result = controller.Get();

            Assert.Equal(35, result);
        }
    }
}