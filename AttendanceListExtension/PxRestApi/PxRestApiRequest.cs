using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// Request an die PROFFIX REST API.
    /// </summary>
    public class PxRestApiRequest : IPxRestApiMessage {
        /// <summary>
        /// Holt oder setzt das Protokoll.
        /// </summary>
        /// <value>Enthält das Protokoll, z.B. 1.1.</value>
        public string Protocol { get; set; }

        /// <summary>
        /// Holt oder setzt das Schema.
        /// </summary>
        /// <value>Enthält das Schema, wie http oder https.</value>
        public string Scheme { get; set; }

        /// <summary>
        /// Holt oder setzt die Methode.
        /// </summary>
        /// <value>Enthält die Methode, z.B. GET, POST, PUT, DELETE usw.</value>
        public string Method { get; set; }

        /// <summary>
        /// Holt oder setzt den Pfad.
        /// </summary>
        /// <value>Enthält den Pfad, z.B. /pxapi/v2/PRO/Info.</value>
        public string Path { get; set; }

        /// <summary>
        /// Holt oder setzt die URL-Parameter.
        /// </summary>
        /// <value>Enthält den Query-String, z.B. ?limit=5.</value>
        public string QueryString { get; set; }

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
