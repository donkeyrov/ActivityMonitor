namespace ActivityMonitorAPI
{
    public static class Services
    {
        private static Dictionary<string, int> cache = new Dictionary<string, int>();

        private static object cacheLock = new object();
        public static Dictionary<string, int> AppCache
        {
            get
            {
                lock (cacheLock)
                {
                    if (cache == null)
                    {
                        cache = new Dictionary<string, int>();
                    }
                    return cache;
                }
            }
        }
    }
}
