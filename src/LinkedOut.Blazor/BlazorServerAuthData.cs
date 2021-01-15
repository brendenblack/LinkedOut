﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class BlazorServerAuthData
    {
        public string SubjectId;
        public DateTimeOffset Expiration;
        public string AccessToken;
        public string RefreshToken;
    }
}
