using System;
using System.Collections.Generic;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Cache to temporarily store and retrieve values during retime.  The cache's contents are scoped
    /// to the test case, accessible to its test step children, however not subsequent test cases.
    /// Each test case execution re-initializes making cache contents temporary.
    /// </summary>
    public static class TestCache
    {
        #region Data members

        private static Dictionary<string, object> _testCache = null;

        #endregion Internal methods

        #region internal methods

        /// <summary>
        /// Initializes the cache (i.e., creates dictionary).  Added to each
        /// public methods so if individual steps are run, exception is not thrown.
        /// Each test cases re-initializes so limit scope over test run.
        /// </summary>
        /// <returns>True of cache dictionary initialized , false if already initialized. </returns>
        internal static bool Initialize()
        {
            var started = false;

            if (_testCache == null)
            {
                _testCache = new Dictionary<string, object>();
                started = true;
            }

            return started;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Stashes an object into the cache's dictionary
        /// </summary>
        /// <param name="key">Key of object</param>
        /// <param name="object">value of object</param>
        public static void Stash(string key, object @object)
        {
            _testCache.Add(key, @object);
        }

        /// <summary>
        /// Grabs and an object of type T from the cache's dictionary identified by the key.
        /// Exception thrown if not found.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="key">Object key</param>
        /// <returns>Object of type T</returns>
        public static T Grab<T>(string key)
        {
            return (T)_testCache[key];
        }

        /// <summary>
        /// Grabs and an object from the cache's dictionary identified by the key.
        /// Exception thrown if not found.
        /// </summary>
        /// <param name="key">Object key</param>
        /// <returns>Object requested</returns>
        public static object Grab(string key)
        {
            return _testCache[key];
        }

        /// <summary>
        /// Tries to grab and object from cache's dictionary identified by key. 
        /// </summary>
        /// <param name="key">Object key</param>
        /// <param name="value">Object requested</param>
        /// <returns>Object requested if found, default value otherwise</returns>
        public static bool TryGrabValue(string key, out object value)
        {
            return _testCache.TryGetValue(key, out value);
        }

        /// <summary>
        /// Tries to grab and object of type T from cache's dictionary identified by key. 
        /// </summary>
        // <typeparam name="T">Type to return</typeparam>
        /// <param name="key">Object key</param>
        /// <param name="value">Object requested</param>
        /// <returns>Object of type T requested if found, default value otherwise</returns>
        public static bool TryGrabValue<T>(string key, out T value)
        {
            var found = _testCache.TryGetValue(key, out object valueObject);
            value = found ? (T)valueObject : default(T);

            return found;
        }

        /// <summary>
        /// Clears contents of TestCache dictionary
        /// </summary>
        public static void Clear()
        {
            _testCache.Clear();
        }

        /// <summary>
        /// Removes object from cache specified by key.
        /// </summary>
        /// <param name="key">Key of object to remove</param>
        /// <returns>True of item found and removed, false otherwise</returns>
        public static bool Remove(string key)
        {
            return _testCache.Remove(key);
        }

        internal static void Dispose()
        {
            _testCache = null;
        }


        #endregion
    }
}
