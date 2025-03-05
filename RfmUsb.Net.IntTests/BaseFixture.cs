/*
* MIT License
*
* Copyright (c) 2023 Derek Goslin
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

// Ignore Spelling: Rfm

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RfmUsb.Net.IntTests.Options;
using Serilog;
using Serilog.Core;

namespace RfmUsb.Net.IntTests
{
    public class BaseFixture
    {
        private readonly Logger _logger;

        public BaseFixture()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddSerilog(_logger);
                })
                .AddRfmUsb()
                .AddOptions();

            serviceCollection
                .AddOptions<DeviceConfigurationOptions>()
                .Bind(Configuration.GetSection(nameof(DeviceConfigurationOptions)));

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public Logger Logger => _logger;

        public IConfiguration Configuration { get; }

        public ServiceProvider ServiceProvider { get; }
    }
}