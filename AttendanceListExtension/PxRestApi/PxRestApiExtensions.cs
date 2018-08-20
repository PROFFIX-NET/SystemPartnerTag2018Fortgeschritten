using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AttendanceListExtension.PxRestApi {
    /// <summary>
    /// IServiceCollection Extensions. 
    /// </summary>
    public static class PxRestApiExtensions {
        /// <summary>
        /// Registriert die benötigten Services für die Kommunikation mit der PROFFIX REST API.
        /// </summary>
        /// <param name="services">Die bestehenden Services.</param>
        /// <returns>Die bestehenden Services mit den PROFFIX REST API spezifischen Services.</returns>
        public static IServiceCollection AddPxRestApi(this IServiceCollection services) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services.AddSingleton<IPxRestApiClient, PxRestApiClient>();
        }
    }
}
