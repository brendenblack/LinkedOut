using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class BlazorServerAuthStateCache
    {
        private ConcurrentDictionary<string, BlazorServerAuthData> _cache = new ();

        public bool HasSubjectId(string subjectId) => _cache.ContainsKey(subjectId);

        public void Add(string subjectId, DateTimeOffset expiration, string accessToken, string refreshToken)
        {
            var data = new BlazorServerAuthData
            {
                SubjectId = subjectId,
                Expiration = expiration,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            _cache.AddOrUpdate(subjectId, data, (k, v) => data);
        }

        public BlazorServerAuthData Get(string subjectId)
        {
            _cache.TryGetValue(subjectId, out var data);
            return data;
        }

        public void Remove(string subjectId)
        {
            _cache.TryRemove(subjectId, out _);
        }
    }
}
