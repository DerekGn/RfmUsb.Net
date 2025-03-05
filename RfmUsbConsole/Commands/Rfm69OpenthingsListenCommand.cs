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

// Ignore Spelling: Openthings Rfm

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using RfmUsb.Net;

namespace RfmUsbConsole.Commands
{
    [Command(Description = "Listen for Openthings message transmissions")]
    internal class Rfm69OpenthingsListenCommand : BaseRfm69Command
    {
        public Rfm69OpenthingsListenCommand(ILogger<Rfm69OpenthingsListenCommand> logger, IRfm69 rfm) : base(logger, rfm)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand(console, () =>
            {
                IRfm69 rfm = (IRfm69)Rfm;

                rfm.RxBw = 14;
                rfm.SyncSize = 1;
                rfm.CrcOn = false;
                rfm.PreambleSize = 3;
                rfm.SyncEnable = true;
                rfm.SyncBitErrors = 0;
                rfm.PayloadLength = 66;
                rfm.PacketFormat = false;
                rfm.CrcAutoClearOff = false;
                rfm.FrequencyDeviation = 0x01EC;
                rfm.RssiThreshold = RssiThreshold;
                rfm.AddressFiltering = AddressFilter.None;
                rfm.Sync = new List<byte>() { 0x2D, 0xD4 };
                rfm.DcFreeEncoding = DcFreeEncoding.Manchester;

                rfm.SetDioMapping(Dio.Dio0, DioMapping.DioMapping1);
                rfm.DioInterruptMask = DioIrq.Dio0;

                rfm.Mode = Mode.Rx;

                do
                {
                    var source = WaitForSignal();

                    if (source == SignalSource.Irq)
                    {
                        Logger.LogInformation(
                            "Ping Response Received. {NewLineA} Irq: [{Irq}] {NewLineB} Rssi: [{Rssi}]",
                            Environment.NewLine,
                            rfm.IrqFlags,
                            Environment.NewLine,
                            rfm.Rssi);

                        Logger.LogInformation("Fifo: {Fifo}", BitConverter.ToString(rfm.Fifo.ToArray()).Replace("-", string.Empty));
                    }
                    else
                    {
                        Logger.LogInformation("Ping Listen Stop");
                    }
                } while (true);
            });
        }
    }
}