namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// PROFFIX REST API Ereignis.
    /// </summary>
    /// <typeparam name="TData">Bestimmt den Typ für die Daten des Ereignisses fest.</typeparam>
    public class PxRestApiEvent<TData> {
        /// <summary>
        /// Holt oder setzt den Namen des Ereignisses.
        /// </summary>
        /// <value>Der Name des Ereignisses.</value>
        public string Name { get; set; }

        /// <summary>
        /// Holt oder setzt die Daten des Ereignisses.
        /// </summary>
        /// <value>Die Daten des Ereignisses.</value>
        public TData Data { get; set; }
    }
}
