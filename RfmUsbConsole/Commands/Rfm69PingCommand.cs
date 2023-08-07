/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin https://github.com/DerekGn
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

using McMaster.Extensions.CommandLineUtils;
using RfmUsb.Net;
using System.Diagnostics;

namespace RfmUsbConsole.Commands
{
    [Command(Description = "Ping using RfmUsb Rfm9x radio")]
    internal class Rfm69PingCommand : BaseRfm69Command
    {
        public Rfm69PingCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option(Templates.PingCount, "The number of echo requests to send", CommandOptionType.SingleValue)]
        public int PingCount { get; set; } = 3;

        [Option(Templates.PingInterval, "The interval between pings", CommandOptionType.SingleValue)]
        public int PingInterval { get; set; } = 1000;

        [Option(Templates.PingTimeout, "The ping timeout", CommandOptionType.SingleValue)]
        public int PingTimeout { get; set; } = 1000;

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand(console, (device) =>
            {
                IRfm69 rfm = (IRfm69)device;

                rfm.FifoThreshold = 1;

                SetupPingConfiguration(rfm);

                rfm.SetDioMapping(Dio.Dio0, DioMapping.DioMapping0);
                rfm.DioInterruptMask = DioIrq.Dio0;

                console.WriteLine("Ping started");

                for (int i = 0; i < PingCount; i++)
                {
                    rfm.Fifo = new List<byte>() { 0x55, 0xAA };

                    rfm.Mode = Mode.Tx;

                    var source = WaitForSignal(PingTimeout);

                    console.WriteLine($"Irq: {rfm.IrqFlags}");

                    if (source == SignalSource.Irq)
                    {
                        console.WriteLine("Packet Transmitted waiting for response.");

                        WaitForPingResponse(rfm, i);
                    }
                    else if (source == SignalSource.Console)
                    {
                        console.WriteLine("Ping Cancelled.");
                    }
                    else if (source == SignalSource.None)
                    {
                        console.WriteLine($"Ping [{i}] Timeout.");
                    }

                    Thread.Sleep(PingInterval);
                }

                return 0;
            });
        }

        private void WaitForPingResponse(IRfm69 rfm, int i)
        {
            rfm.Mode = Mode.Rx;

            Stopwatch sw = new Stopwatch();
            
            sw.Start();

            var source = WaitForSignal(PingTimeout);

            if (source == SignalSource.Irq)
            {
                sw.Stop();

                Console.WriteLine(
                    $"Ping Response Received." + Environment.NewLine +
                    $"Irq: {rfm.IrqFlags}" + Environment.NewLine +
                    $"Elapsed: [{sw.Elapsed}]");
            }
            else if (source == SignalSource.Console)
            {
                Console.WriteLine("Ping Cancelled.");
            }
            else if (source == SignalSource.None)
            {
                Console.WriteLine($"Ping [{i}] Timeout.");
            }

            rfm.Mode = Mode.Standby;
        }
    }
}