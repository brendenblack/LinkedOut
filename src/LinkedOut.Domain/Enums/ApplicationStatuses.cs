using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut
{
    public enum ApplicationStatuses
    {
        UNKNOWN,
        /// <summary>
        /// A newly created application that is not yet in progress.
        /// </summary>
        CREATED,
        /// <summary>
        /// Indicates that the application is still be constructed, and has not yet been submitted to the employer.
        /// </summary>
        INPROGRESS,
        /// <summary>
        /// The application has been submitted to the employer, but there has been no feedback from them about next steps.
        /// </summary>
        SUBMITTED,
        /// <summary>
        /// The application has been closed and is no longer under consideration.
        /// </summary>
        CLOSED
    }
}
