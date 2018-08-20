using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// Response von der PROFFIX REST API.
    /// </summary>
    public class PxRestApiResponse : IPxRestApiMessage {
        /// <summary>
        /// Holt oder setzt den Status-Code.
        /// </summary>
        /// <value>Enthält den Status-Code, z.B. 200.</value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Holt oder setzt die Headers.
        /// </summary>
        /// <value>Enthält die Headers.</value>
        public IDictionary<string, string[]> Headers { get; set; }

        /// <summary>
        /// Holt oder setzt den Body.
        /// </summary>
        /// <value>Enthält den Body.</value>
        public JContainer Body { get; set; }
    }
}
