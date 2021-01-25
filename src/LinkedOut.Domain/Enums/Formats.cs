using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut
{
    public enum Formats
    {
        /// <summary>
        /// The related content should be rendered as recorded; what you see is what you get.
        /// </summary>
        PLAINTEXT,
        /// <summary>
        /// The related content is stored in HTML format and should be parsed before displaying it.
        /// </summary>
        HTML,
        /// <summary>
        /// The related content is stored in Markdown format and should be parsed before displaying it.
        /// </summary>
        MARKDOWN
    }
}
