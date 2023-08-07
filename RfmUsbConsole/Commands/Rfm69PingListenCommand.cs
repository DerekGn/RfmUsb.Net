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

namespace RfmUsbConsole.Commands
{
    internal class Rfm69PingListenCommand : BaseRfm69Command
    {
        public Rfm69PingListenCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand(console, (device) =>
            {
                IRfm69 rfm = (IRfm69)device;

                SetupPingConfiguration(rfm);

                rfm.DioInterruptMask = DioIrq.Dio0;

                do
                {
                    EnterRxMode(rfm);

                    var source = WaitForSignal();

                    if (source == SignalSource.Irq)
                    {
                        var rssi = rfm.Rssi;
                        var irq = rfm.IrqFlags;

                        console.WriteLine(
                            $"Ping Received." + Environment.NewLine +
                            $"Irq: {irq}" + Environment.NewLine +
                            $"Rssi: {rssi}");

                        var fifo = rfm.Fifo.ToList();

                        if ((fifo[0] == 0x55) && (fifo[1] == 0xAA))
                        {
                            SendPingResponse(console, rfm);
                        }
                    }
                    else if (source == SignalSource.Console)
                    {
                        console.WriteLine("Ping Listen Stop");
                    }
                } while (true);
            });
        }

        private void SendPingResponse(IConsole console, IRfm69 rfm)
        {
            rfm.Mode = Mode.Standby;

            rfm.Fifo = new List<byte>() { 0xAA, 0x55 };

            EnterTxMode(rfm);

            if (WaitForSignal() == SignalSource.Irq)
            {
                console.WriteLine(rfm.IrqFlags);
                console.WriteLine("Ping Reply Sent");
            }
        }
    }
}