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
    [Command(Description = "Listen for Openthings message transmissions")]
    internal class Rfm69OpenthingsListenCommand : BaseRfm69Command
    {
        public Rfm69OpenthingsListenCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand(console, (device) =>
            {
                IRfm69 rfm = (IRfm69)device;

                rfm.FrequencyDeviation = 0x01EC;
                rfm.RxBw = 14;
                rfm.SyncSize = 1;
                rfm.PreambleSize = 3;
                rfm.SyncEnable = true;
                rfm.SyncBitErrors = 0;
                rfm.Sync = new List<byte>() { 0x2D, 0xD4 };
                rfm.PacketFormat = false;
                rfm.DcFreeEncoding = DcFreeEncoding.Manchester;
                rfm.CrcOn = false;
                rfm.CrcAutoClearOff = false;
                rfm.AddressFiltering = AddressFilter.None;
                rfm.PayloadLength = 66;
                rfm.SetDioMapping(Dio.Dio0, DioMapping.DioMapping1);
                rfm.DioInterruptMask = DioIrq.Dio0;
                rfm.Mode = Mode.Rx;

                do
                {
                    var source = WaitForSignal();

                    if (source == SignalSource.Irq)
                    {
                        rfm.Mode = Mode.Standby;

                        var fifo = rfm.Fifo;

                        console.WriteLine("Packet Received");

                        rfm.Mode = Mode.Rx;
                    }
                    else
                    {
                        console.WriteLine("Ping Listen Stop");
                    }
                } while (true);
            });
        }
    }
}