using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace AttendanceListExtension {
    /// <summary>
    /// Klasse mit dem Eintrittspunkt.
    /// </summary>
    public class Program {
        /// <summary>
        /// Eintrittspunkt des Programms.
        /// </summary>
        /// <param name="args">Enthält die Kommandozeilenargumente.</param>
        public static void Main(string[] args) {
            try {
                string contentRoot = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                Directory.SetCurrentDirectory(contentRoot);

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(contentRoot)
                    .AddCommandLine(args)
                    .Build();

                IWebHost host = new WebHostBuilder()
                    .UseContentRoot(contentRoot)
                    .UseConfiguration(configuration)
                    .ConfigureLogging(logging =>
                        logging.AddConsole())
                    .UseStartup<Startup>()
                    .UseHttpSys(options => {
                        options.Authentication.Schemes = AuthenticationSchemes.None;
                        options.Authentication.AllowAnonymous = true;
                    })
                    .Build();

                if (Environment.UserInteractive) {
                    host.Run();
                }
                else {
                    host.RunAsService();
                }
            }
            catch (Exception e) {
                new LoggerFactory()
                    .AddConsole()
                    .CreateLogger<Program>()
                    .LogCritical(e.Message);
            }
        }
    }
}
