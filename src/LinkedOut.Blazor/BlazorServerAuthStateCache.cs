using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class BlazorServerAuthStateCache
    {
        private ConcurrentDictionary<string, BlazorServerAuthData> Cache = new ();

        public bool HasSubjectId(string subjectId) => Cache.ContainsKey(subjectId);

        public void Add(string subjectId, DateTimeOffset expiration, string accessToken, string refreshToken)
        {
            var data = new BlazorServerAuthData
            {
                SubjectId = subjectId,
                Expiration = expiration,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            Cache.AddOrUpdate(subjectId, data, (k, v) => data);
        }

        public BlazorServerAuthData Get(string subjectId)
        {
            Cache.TryGetValue(subjectId, out var data);
            return data;
        }

        public void Remove(string subjectId)
        {
            Cache.TryRemove(subjectId, out _);
        }
    }
}
