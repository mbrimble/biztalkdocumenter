
namespace Microsoft.Services.Tools.BizTalkOM
{
    using System.Collections;
    using System.Timers;

    /// <summary>
    /// ManagedCache
    /// </summary>
    public abstract class ManagedCache
    {
        private static Hashtable objectStore;
        private Timer cacheCleanupTimer;

        public ManagedCache()
        {
            objectStore = new Hashtable();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ManagedCache(int cacheTimeoutSeconds)
            : this()
        {
            this.InitializeTimer(cacheTimeoutSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        protected object SyncRoot
        {
            get { return objectStore.SyncRoot; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void CacheObject(object key, object value)
        {
            lock (objectStore)
            {
                if (!objectStore.ContainsKey(key))
                {
                    objectStore.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object GetObject(object key)
        {
            object val = null;

            lock (objectStore)
            {
                if (objectStore.ContainsKey(key))
                {
                    val = objectStore[key];
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool ContainsKey(object key)
        {
            return objectStore.ContainsKey(key);
        }

        /// <summary>
        /// InitializeTimer
        /// </summary>
        /// <param name="cacheTimeoutSeconds"></param>
        private void InitializeTimer(int cacheTimeoutSeconds)
        {
            if (cacheTimeoutSeconds > 0)
            {
                cacheCleanupTimer = new Timer(cacheTimeoutSeconds);
                cacheCleanupTimer.Elapsed += new ElapsedEventHandler(CacheCleanupTimerElapsed);
                cacheCleanupTimer.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CacheCleanupTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (objectStore)
            {
                if (objectStore != null && objectStore.Count > 0)
                {
                    objectStore.Clear();
                }
            }
            return;
        }
    }
}
