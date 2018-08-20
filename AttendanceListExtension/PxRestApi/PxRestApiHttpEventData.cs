namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// Die Daten eines PROFFIX REST API HTTP Ereignisses.
    /// </summary>
    public class PxRestApiHttpEventData {
        /// <summary>
        /// Holt oder setzt den Request.
        /// </summary>
        /// <value>Enthält den Request eines Clients an die PROFFIX REST API.</value>
        public PxRestApiRequest Request { get; set; }

        /// <summary>
        /// Holt oder setzt die Response.
        /// </summary>
        /// <value>Enthält die Response der PROFFIX REST API an einen Client.</value>
        public PxRestApiResponse Response { get; set; }
    }
}
