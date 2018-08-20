using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// Interface für Request und Response.
    /// </summary>
    public interface IPxRestApiMessage {
        /// <summary>
        /// Holt oder setzt die Headers.
        /// </summary>
        /// <value>Enthält die Headers.</value>
        IDictionary<string, string[]> Headers { get; set; }

        /// <summary>
        /// Holt oder setzt den Body.
        /// </summary>
        /// <value>Enthält den Body.</value>
        JContainer Body { get; set; }
    }
}
