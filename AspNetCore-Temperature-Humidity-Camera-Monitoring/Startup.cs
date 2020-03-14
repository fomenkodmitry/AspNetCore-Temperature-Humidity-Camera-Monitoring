using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore_Temperature_Humidity_Camera_Monitoring
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        await context.WebSockets.AcceptWebSocketAsync();
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // using Microsoft.AspNetCore.HttpOverrides;

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            app.UseAuthorization();
            Task.Factory.StartNew(
                _runDht,
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            ).Unwrap();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private async Task _runDht()
        {
            var webSocket = new ClientWebSocket();
            // var ws = new ClientWebSocket();
            // var uri = new Uri("wss://ws.binaryws.com/websockets/v3");
 
            // await ws.ConnectAsync(uri, CancellationToken.None);
            
            using var dht = new Dht22(4);
            while (true)
            {
                // Try to read the temperature.
                var temp = dht.Temperature;
                if (!dht.IsLastReadSuccessful) continue;

                // Try to read the humidity.
                var humidity = dht.Humidity;
                if (!dht.IsLastReadSuccessful) continue;

                var res = $"Temperature: {temp.Celsius:0.0} °C, 'Humidity: {humidity:0.0} %";
                Console.WriteLine(res);
                await webSocket.SendAsync(
                    buffer: new ArraySegment<byte>(
                        array: Encoding.ASCII.GetBytes(res),
                        offset: 0,
                        count: res.Length
                    ),
                    messageType: WebSocketMessageType.Text,
                    endOfMessage: true,
                    cancellationToken: CancellationToken.None
                );
                await Task.Delay(2000);
            }
            
        }
    }
}