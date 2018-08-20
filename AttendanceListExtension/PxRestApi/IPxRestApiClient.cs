using System;
using System.Threading.Tasks;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// Definiert Methoden für die Kommunikation mit der PROFFIX REST API.
    /// </summary>
    public interface IPxRestApiClient : IDisposable {
        /// <summary>
        /// Sendet einen beliebigen Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="request">Der Request mit allen benötigten Informationen.</param>
        /// <param name="addKey">Gibt an, ob der Key-URL-Parameter dem Request hinzugefügt werden soll.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        Task<PxRestApiResponse> SendAsync(PxRestApiRequest request, bool addKey = false);

        /// <summary>
        /// Sendet einen GET Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <param name="addKey">Gibt an, ob der Key-URL-Parameter dem Request hinzugefügt werden soll.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        Task<PxRestApiResponse> GetAsync(string path, string queryString = null, bool addKey = false);

        /// <summary>
        /// Sendet einen POST Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="body">Der Body, der gesendet werden soll.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        Task<PxRestApiResponse> PostAsync(string path, object body = null, string queryString = null);

        /// <summary>
        /// Sendet einen PUT Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="body">Der Body, der gesendet werden soll.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        Task<PxRestApiResponse> PutAsync(string path, object body = null, string queryString = null);

        /// <summary>
        /// Sendet einen DELETE Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        Task<PxRestApiResponse> DeleteAsync(string path, string queryString = null);
    }
}
