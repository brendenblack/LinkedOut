using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Domain
{
    public enum ApplicationResolutions
    {
        /// <summary>
        /// The application is still in progress.
        /// </summary>
        UNRESOLVED,
        WITHDRAWN,
        CANCELLED,
        REJECTED,
        OFFER_DECLINED,
        OFFER_ACCEPTED
    }
}
