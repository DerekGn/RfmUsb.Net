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
using Microsoft.Extensions.Logging;
using RfmUsb.Net;
using RfmUsb.Net.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace RfmUsbConsole.Commands
{
    internal abstract class BaseCommand
    {
        //private readonly List<byte> ping = new() { 0x50, 0x49 };
        internal readonly ILogger Logger;
        internal AutoResetEvent IrqSignal;
        internal IRfm Rfm;

        private AutoResetEvent _consoleSignal;
        private AutoResetEvent[] waitHandles;

        protected BaseCommand(ILogger logger, IRfm rfm)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Rfm = rfm ?? throw new ArgumentNullException(nameof(rfm));

            IrqSignal = new AutoResetEvent(false);
            _consoleSignal = new AutoResetEvent(false);

            waitHandles = new List<AutoResetEvent>() { IrqSignal, _consoleSignal }.ToArray();
        }

        [Range(1200, 300000)]
        [Option(Templates.BaudRate, "The radio baud rate.", CommandOptionType.SingleValue)]
        public uint? BaudRate { get; set; }

        [Option(Templates.Frequency, "The radio frequency.", CommandOptionType.SingleValue)]
        public uint? Frequency { get; set; }

        [Option(Templates.Modulation, "The radio modulation.", CommandOptionType.SingleValue)]
        public ModulationType? Modulation { get; set; }

        [Required]
        [Option(Templates.SerialPort, "The serial port the RfmUsb device is connected.", CommandOptionType.SingleValue)]
        public string SerialPort { get; set; }

        internal static void EnterRxMode(IRfm rfm)
        {
            rfm.SetDioMapping(Dio.Dio0, DioMapping.DioMapping1);
            rfm.Mode = Mode.Rx;
        }

        internal static void EnterTxMode(IRfm rfm)
        {
            rfm.SetDioMapping(Dio.Dio0, DioMapping.DioMapping0);
            rfm.Mode = Mode.Tx;
        }

        internal static void SetupPingConfiguration(IRfm rfm, byte? rxbw)
        {
            if (rxbw.HasValue)
            {
                rfm.RxBw = rxbw.Value;
            }

            rfm.CrcOn = true;
            rfm.PayloadLength = 2;

            rfm.SyncSize = 1;
            rfm.Sync = new List<byte>() { 0xDE, 0xAD };

            rfm.FifoThreshold = 1;
        }

        internal int ExecuteCommand(IConsole console, Func<int> action)
        {
            int result = -1;

            try
            {
                console.CancelKeyPress += ConsoleCancelKeyPress;

                if (Rfm != null)
                {
                    Rfm.Open(SerialPort, 230400);

                    Rfm.ExecuteReset();

                    if (Frequency.HasValue)
                    {
                        Rfm.Frequency = Frequency.Value;
                    }

                    if (BaudRate.HasValue)
                    {
                        Rfm.BitRate = BaudRate.Value;
                    }

                    if (Modulation.HasValue)
                    {
                        Rfm.ModulationType = Modulation.Value;
                    }

                    Rfm.DioInterrupt += RfmDeviceDioInterrupt;

                    action();
                }
                else
                {
                    Logger.LogInformation($"Unable to resolve RfmUsb device.");
                }
            }
            catch (RfmUsbInvalidDeviceTypeException ex)
            {
                Logger.LogError("Invalid Device Type: {message}", ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError("An unhandled exception occurred: {ex}", ex);
            }
            finally
            {
                if (Rfm != null)
                {
                    Rfm.DioInterrupt -= RfmDeviceDioInterrupt;
                    Rfm.Close();
                    Rfm.Dispose();
                }

                console.CancelKeyPress -= ConsoleCancelKeyPress;
            }

            return result;
        }

        internal int ExecutePing(byte? rxbw, int pingCount, int pingTimeout, int pingInterval)
        {
            SetupPingConfiguration(Rfm, rxbw);

            Rfm.DioInterruptMask = DioIrq.Dio0;

            Logger.LogInformation("Ping started");

            for (int i = 0; i < pingCount; i++)
            {
                Rfm.Fifo = new List<byte>() { 0x55, 0xAA };

                EnterTxMode(Rfm);

                var source = WaitForSignal(pingTimeout);

                PrintIrqFlags();

                if (source == SignalSource.Irq)
                {
                    WaitForPingResponse(pingTimeout, i);
                }
                else if (source == SignalSource.Console)
                {
                    Logger.LogInformation("Ping Cancelled.");
                }
                else if (source == SignalSource.None)
                {
                    Logger.LogInformation("Ping [{i}] Timeout.", i);
                }

                Thread.Sleep(pingInterval);
            }

            return 0;
        }

        internal int ExecutePingListen(byte? rxbw)
        {
            SetupPingConfiguration(Rfm, rxbw);

            Rfm.DioInterruptMask = DioIrq.Dio0;

            do
            {
                EnterRxMode(Rfm);

                var source = WaitForSignal();

                if (source == SignalSource.Irq)
                {
                    if (CheckPacketReceived())
                    {
                        Rfm.Mode = Mode.Standby;

                        var fifo = Rfm.Fifo.ToList();

                        if ((fifo[0] == 0x55) && (fifo[1] == 0xAA))
                        {
                            Logger.LogInformation("Ping Received. Rssi: {rssi}", Rfm.Rssi);

                            SendPingResponse();
                        }
                        else
                        {
                            Logger.LogWarning("Invalid Packet Received. Packet:[{packet}] Rssi: {rssi}",
                                BitConverter.ToString(fifo.Take(2).ToArray()), Rfm.Rssi);
                        }
                    }
                    else
                    {
                        Logger.LogWarning("No Packet Recieved.");
                    }
                }
                else if (source == SignalSource.Console)
                {
                    Logger.LogInformation("Ping Listen Stop");
                }
            } while (true);
        }

        internal SignalSource WaitForSignal(int timeout = -1)
        {
            return (SignalSource)AutoResetEvent.WaitAny(waitHandles, timeout);
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.Error.WriteLine("You must specify a command. See --help for more details.");
            app.ShowHelp();

            return 0;
        }

        protected abstract void PrintIrqFlags();

        protected abstract bool CheckPacketReceived();

        private void ConsoleCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _consoleSignal.Set();
        }

        private void RfmDeviceDioInterrupt(object? sender, DioIrq e)
        {
            IrqSignal.Set();

            Logger.LogDebug("Dio Irq [{e}]", e);
        }

        private void SendPingResponse()
        {
            Rfm.Fifo = new List<byte>() { 0xAA, 0x55 };

            EnterTxMode(Rfm);

            if (WaitForSignal() == SignalSource.Irq)
            {
                PrintIrqFlags();

                Logger.LogInformation("Ping Reply Sent");
            }
        }

        private void WaitForPingResponse(int pingTimeout, int pingNumber)
        {
            EnterRxMode(Rfm);

            Stopwatch sw = new Stopwatch();

            sw.Start();

            var source = WaitForSignal(pingTimeout);

            PrintIrqFlags();

            if (source == SignalSource.Irq)
            {
                sw.Stop();

                Logger.LogInformation(
                    "Ping Response Received. Elapsed: [{elapsed}]",
                    sw.Elapsed);
            }
            else if (source == SignalSource.Console)
            {
                Logger.LogInformation("Ping Cancelled.");
            }
            else if (source == SignalSource.None)
            {
                Logger.LogInformation("Ping [{pingNumber}] Timeout.", pingNumber);
            }

            Rfm.Mode = Mode.Standby;
        }
    }
}