using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using AttendanceListExtension.PxRestApi;

namespace AttendanceListExtension {
    /// <summary>
    /// Die Startup Klasse initialisiert und konfiguriert die Services und die Request-Response-Pipeline.
    /// </summary>
    public class Startup {
        /// <summary>
        /// Konfiguriert die Services für Dependency Injection.
        /// </summary>
        /// <param name="services">Die bestehenden Services.</param>
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddPxRestApi()
                .AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        /// <summary>
        /// Konfiguriert die Request-Response-Pipeline.
        /// </summary>
        /// <param name="builder">Der Application-Builder.</param>
        public void Configure(IApplicationBuilder builder) {
            builder.UseMvc();
        }
    }
}
