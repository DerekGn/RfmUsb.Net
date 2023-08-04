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

        [Option(Templates.NumberPings, "The number of echo requests to send", CommandOptionType.SingleValue)]
        public int PingCount { get; set; } = 3;

        [Option(Templates.Timeout, "The ping timeout", CommandOptionType.SingleValue)]
        public int PingTimeout { get; set; } = 1000;

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand(console, (device) =>
            {
                IRfm69 rfm69 = (IRfm69)device;

                rfm69.DataMode = Rfm69DataMode.Packet;
                rfm69.TxStartCondition = true;
                rfm69.Mode = Mode.Tx;
                rfm69.IntermediateMode = IntermediateMode.Rx;
                rfm69.AutoModeEnterCondition = EnterCondition.PacketSent;
                rfm69.AutoModeExitCondition = ExitCondition.CrcOk;
                rfm69.SetDioMapping(Dio.Dio0, DioMapping.DioMapping1);
                rfm69.DioInterruptMask = DioIrq.Dio0;

                console.WriteLine("Ping started");

                Stopwatch sw = new Stopwatch();

                for (int i = 0; i < PingCount; i++)
                {
                    rfm69.Fifo = new List<byte>() { 0xAA, 0x55 };

                    rfm69.Mode = Mode.Tx;

                    sw.Restart();

                    int signalSource = WaitForSignal(PingTimeout);

                    if (signalSource == 0)
                    {
                        sw.Stop();

                        console.WriteLine(rfm69.IrqFlags);
                        console.WriteLine($"Ping Received [{sw.Elapsed}] {sw.ElapsedMilliseconds}");
                    }
                    else if (signalSource == 1)
                    {
                        console.WriteLine("Ping Cancelled.");
                    }
                    else if (signalSource == 0x104)
                    {
                        console.WriteLine($"Ping [{i}] Timeout.");
                    }
                }

                return 0;
            });
        }
    }
}