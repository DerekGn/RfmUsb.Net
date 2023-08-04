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
using RfmUsb.Net.Exceptions;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace RfmUsbConsole.Commands
{
    internal abstract class BaseCommand
    {
        protected readonly IServiceProvider ServiceProvider;
        protected AutoResetEvent IrqSignal;

        private AutoResetEvent _consoleSignal;
        private AutoResetEvent[] waitHandles;

        protected BaseCommand(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            IrqSignal = new AutoResetEvent(false);
            _consoleSignal = new AutoResetEvent(false);

            waitHandles = new List<AutoResetEvent>() { IrqSignal, _consoleSignal }.ToArray();
        }

        [Range(1200, 300000)]
        [Option(Templates.BaudRate, "The radio baud rate.", CommandOptionType.SingleValue)]
        public uint BaudRate { get; set; } = 4800;

        [Option(Templates.Frequency, "The radio frequency.", CommandOptionType.SingleValue)]
        public uint Frequency { get; set; } = 433920000;

        [Option(Templates.Modulation, "The radio modulation.", CommandOptionType.SingleValue)]
        public ModulationType Modulation { get; set; } = ModulationType.Fsk;

        [Range(-2, 20)]
        [Option(Templates.OutputPower, "The radio output power.", CommandOptionType.SingleValue)]
        public byte OutputPower { get; set; } = 0;

        [Required]
        [Option(Templates.SerialPort, "The serial port the RfmUsb device is connected.", CommandOptionType.SingleValue)]
        public string SerialPort { get; set; }

        internal int WaitForSignal(int timeout = -1)
        {
            return AutoResetEvent.WaitAny(waitHandles, timeout);
        }

        protected abstract IRfm? CreatDeviceInstance();

        protected int ExecuteCommand(IConsole console, Func<IRfm, int> action)
        {
            int result = -1;

            IRfm? rfmDevice = null;

            try
            {
                console.CancelKeyPress += ConsoleCancelKeyPress;

                rfmDevice = CreatDeviceInstance();

                if (rfmDevice != null)
                {
                    rfmDevice.Open(SerialPort, 230400);

                    rfmDevice.ExecuteReset();
                    rfmDevice.Frequency = Frequency;
                    rfmDevice.BitRate = BaudRate;
                    rfmDevice.ModulationType = Modulation;
                    rfmDevice.LnaGainSelect = LnaGain.Auto;
                    rfmDevice.AfcAutoOn = true;

                    rfmDevice.DioInterrupt += RfmDeviceDioInterrupt;

                    action(rfmDevice);
                }
                else
                {
                    Console.Error.WriteLine($"Unable to resolve RfmUsb device.");
                }
            }
            catch (RfmUsbInvalidDeviceTypeException ex)
            {
                Console.Error.WriteLine($"Invalid Device Type: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unhandled exception occurred: {ex}");
            }
            finally
            {
                if (rfmDevice != null)
                {
                    rfmDevice.DioInterrupt -= RfmDeviceDioInterrupt;
                    rfmDevice.Close();
                    rfmDevice.Dispose();
                }

                console.CancelKeyPress -= ConsoleCancelKeyPress;
            }

            return result;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.Error.WriteLine("You must specify a command. See --help for more details.");
            app.ShowHelp();

            return 0;
        }

        private void ConsoleCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _consoleSignal.Set();
        }

        private void RfmDeviceDioInterrupt(object? sender, DioIrq e)
        {
            IrqSignal.Set();

            Log.Debug("Dio Irq [{e}]", e);
        }
    }
}