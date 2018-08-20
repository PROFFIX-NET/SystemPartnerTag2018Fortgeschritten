using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// HTTP Client für die Kommunikation mit der PROFFIX REST API.
    /// </summary>
    public class PxRestApiClient : IPxRestApiClient {
        /// <summary>
        /// Header mit der aktuellen Session-ID der PROFFIX REST API.
        /// </summary>
        public const string PxSessionIdHeader = "PxSessionId";

        /// <summary>
        /// Header mit der öffentlichen URL der PROFFIX REST API.
        /// </summary>
        public const string PxPublicUrlHeader = "PxPublicUrl";

        /// <summary>
        /// Header mit allen Adressen, auf die die PROFFIX REST API hört.
        /// </summary>
        public const string PxAddressHeader = "PxAddress";

        /// <summary>
        /// Header mit dem Key der PROFFIX REST API, um Zugriff auf Endpunkte ohne Login zu erhalten.
        /// </summary>
        public const string PxKeyHeader = "PxKey";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpClient httpClient;
        private readonly Encoding utf8WithoutBom;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="PxRestApiClient"/> Klasse.
        /// </summary>
        /// <param name="httpContextAccessor">Ermöglicht den Zugriff auf den aktuellen HTTP-Context.</param>
        public PxRestApiClient(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            this.httpClient = new HttpClient();
            this.utf8WithoutBom = new UTF8Encoding(false);
        }

        /// <summary>
        /// Sendet einen beliebigen Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="request">Der Request mit allen benötigten Informationen.</param>
        /// <param name="addKey">Gibt an, ob der Key-URL-Parameter dem Request hinzugefügt werden soll.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        public async Task<PxRestApiResponse> SendAsync(PxRestApiRequest request, bool addKey = false) {
            HttpContext context = this.httpContextAccessor.HttpContext;

            string sessionId = context.Request.Headers[PxSessionIdHeader];
            string publicUrl = context.Request.Headers[PxPublicUrlHeader];
            string[] addresses = context.Request.Headers[PxAddressHeader];
            string key = addKey ? (string)context.Request.Headers[PxKeyHeader] : null;

            List<string> baseUrls = new List<string>(2);

            const string scheme = "http://";

            string httpAddress = addresses?.FirstOrDefault(address => address?.StartsWith(scheme) ?? false);

            if (httpAddress != null) {
                string host = this.GetHostFromAddress(httpAddress);

                if (host != null) {
                    string afterHost = httpAddress.Substring(scheme.Length + host.Length);

                    if (host == "+" ||
                        host == "*" || (
                            IPAddress.TryParse(host, out IPAddress address) && (
                                address.Equals(IPAddress.Any) ||
                                address.Equals(IPAddress.IPv6Any)))) {
                        host = "localhost";
                    }

                    baseUrls.Add($"{scheme}{host}{afterHost}");
                }
            }

            if (!string.IsNullOrWhiteSpace(publicUrl)) {
                baseUrls.Add(publicUrl);
            }

            foreach (string baseUrl in baseUrls) {
                using (HttpRequestMessage requestMessage = await CreateRequestAsync(baseUrl, sessionId, key, request)) {
                    try {
                        using (HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted)) {
                            return await CreateResponseAsync(responseMessage);
                        }
                    }
                    catch (HttpRequestException) { }
                    catch (OperationCanceledException) {
                        if (context.RequestAborted.IsCancellationRequested) {
                            throw;
                        }
                    }
                }
            }

            throw new InvalidOperationException("Die Anfrage konnte nicht an die PROFFIX REST API gesendet werden, da keine gültige Basis-URL verfügbar ist.");
        }

        /// <summary>
        /// Sendet einen GET Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <param name="addKey">Gibt an, ob der Key-URL-Parameter dem Request hinzugefügt werden soll.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        public async Task<PxRestApiResponse> GetAsync(string path, string queryString = null, bool addKey = false) {
            return await SendAsync(
                new PxRestApiRequest() {
                    Method = HttpMethod.Get.Method,
                    Path = path,
                    QueryString = queryString
                },
                addKey);
        }

        /// <summary>
        /// Sendet einen POST Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="body">Der Body, der gesendet werden soll.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        public async Task<PxRestApiResponse> PostAsync(string path, object body = null, string queryString = null) {
            var request = new PxRestApiRequest() {
                Method = HttpMethod.Post.Method,
                Path = path,
                QueryString = queryString
            };

            request.SetBody(body);

            return await SendAsync(request);
        }

        /// <summary>
        /// Sendet einen PUT Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="body">Der Body, der gesendet werden soll.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        public async Task<PxRestApiResponse> PutAsync(string path, object body = null, string queryString = null) {
            var request = new PxRestApiRequest() {
                Method = HttpMethod.Put.Method,
                Path = path,
                QueryString = queryString
            };

            request.SetBody(body);

            return await SendAsync(request);
        }

        /// <summary>
        /// Sendet einen DELETE Request an die PROFFIX REST API asynchron.
        /// </summary>
        /// <param name="path">Der Pfad zum Endpunkt der PROFFIX REST API.</param>
        /// <param name="queryString">Benötigte URL-Parameter.</param>
        /// <returns>Gibt einen Task mit der Response der PROFFIX REST API zurück.</returns>
        public async Task<PxRestApiResponse> DeleteAsync(string path, string queryString = null) {
            return await SendAsync(
                new PxRestApiRequest() {
                    Method = HttpMethod.Delete.Method,
                    Path = path,
                    QueryString = queryString
                });
        }

        /// <summary>
        /// Gibt die verwendeten Ressourcen frei.
        /// </summary>
        public void Dispose() {
            this.httpClient.Dispose();
        }

        private async Task<HttpRequestMessage> CreateRequestAsync(string baseUrl, string sessionId, string key, PxRestApiRequest request) {
            HttpRequestMessage requestMessage = new HttpRequestMessage();

            requestMessage.Method = new HttpMethod(request.Method);
            requestMessage.Version = new Version(request.Protocol ?? "1.1");
            requestMessage.Headers.TryAddWithoutValidation(PxSessionIdHeader, sessionId);

            if (request.Body != null) {
                MemoryStream body = new MemoryStream();

                using (StreamWriter writer = new StreamWriter(body, this.utf8WithoutBom, 1024, true)) {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(writer, request.Body);
                }

                body.Position = 0;
                requestMessage.Content = new StreamContent(body);
            }

            if (request.Headers != null) {
                foreach (KeyValuePair<string, string[]> header in request.Headers) {
                    if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value)) {
                        requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }
            }

            if (request.Body != null) {
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            string uri = baseUrl.EndsWith("/") ? baseUrl.TrimEnd('/') : baseUrl;

            string path = request.Path;

            if (!string.IsNullOrWhiteSpace(path)) {
                uri += path.StartsWith("/") ? path : "/" + path;
            }

            string queryString = request.QueryString;

            if (!string.IsNullOrWhiteSpace(key)) {
                if (string.IsNullOrWhiteSpace(queryString)) {
                    queryString = $"?key={key}";
                }
                else {
                    queryString += $"&key={key}";
                }
            }

            if (!string.IsNullOrWhiteSpace(queryString)) {
                uri += queryString.StartsWith("?") ? queryString : "?" + queryString;
            }

            requestMessage.RequestUri = new Uri(uri);
            requestMessage.Headers.Host = requestMessage.RequestUri.Host;

            return await Task.FromResult(requestMessage);
        }

        private async Task<PxRestApiResponse> CreateResponseAsync(HttpResponseMessage responseMessage) {
            PxRestApiResponse response = new PxRestApiResponse();

            response.StatusCode = (int)responseMessage.StatusCode;
            response.Headers = new Dictionary<string, string[]>();

            foreach (KeyValuePair<string, IEnumerable<string>> header in responseMessage.Headers) {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in responseMessage.Content.Headers) {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("Transfer-Encoding");

            response.Body = JsonConvert.DeserializeObject<JContainer>(await responseMessage.Content.ReadAsStringAsync());

            return response;
        }

        private string GetHostFromAddress(string address) {
            if (address == null) {
                return null;
            }

            const string endOfScheme = "://";

            int startIndex = address.IndexOf(endOfScheme);

            if (startIndex != 4 && startIndex != 5) {
                return null;
            }

            startIndex += endOfScheme.Length;

            int ipv6Length = address.IndexOf(']', startIndex) - startIndex + 1;
            int lengthToSlash = address.IndexOf('/', startIndex) - startIndex;

            if (ipv6Length >= 0 && (lengthToSlash < 0 || ipv6Length <= lengthToSlash)) {
                return address.Substring(startIndex, ipv6Length);
            }

            int lengthToColon = address.IndexOf(':', startIndex) - startIndex;

            if (lengthToColon >= 0 && lengthToSlash >= 0) {
                if (lengthToColon <= lengthToSlash) {
                    return address.Substring(startIndex, lengthToColon);
                }

                return address.Substring(startIndex, lengthToSlash);
            }

            if (lengthToColon >= 0) {
                return address.Substring(startIndex, lengthToColon);
            }

            if (lengthToSlash >= 0) {
                return address.Substring(startIndex, lengthToSlash);
            }

            return address.Substring(startIndex);
        }
    }
}
