using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut
{
    public enum ApplicationResolutions
    {
        /// <summary>
        /// The application is still in progress.
        /// </summary>
        UNRESOLVED,
        NOT_INTERESTED,
        WITHDRAWN,
        CANCELLED,
        REJECTED,
        OFFER_DECLINED,
        OFFER_ACCEPTED
    }
}
