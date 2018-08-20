using Newtonsoft.Json.Linq;
using System;
using System.Collections;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// IPxRestApiMessage Extensions.
    /// </summary>
    public static class PxRestApiMessageExtensions {
        /// <summary>
        /// Gibt den Body im gewünschten Typ zurück.
        /// </summary>
        /// <typeparam name="TBody">Typ des Bodys.</typeparam>
        /// <param name="message">Request oder Response.</param>
        /// <returns>Gibt den Body zurück.</returns>
        public static TBody GetBody<TBody>(this IPxRestApiMessage message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.Body == null) {
                return default(TBody);
            }

            return message.Body.ToObject<TBody>();
        }

        /// <summary>
        /// Setzt den Body.
        /// </summary>
        /// <param name="message">Request oder Response.</param>
        /// <param name="body">Body.</param>
        public static void SetBody(this IPxRestApiMessage message, object body) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            }

            if (body is JContainer) {
                message.Body = (JContainer)body;
            }
            else if (body is IEnumerable) {
                message.Body = JArray.FromObject(body);
            }
            else {
                message.Body = JObject.FromObject(body);
            }
        }
    }
}
