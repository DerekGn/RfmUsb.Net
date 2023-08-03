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
using System.ComponentModel.DataAnnotations;

namespace RfmUsbConsole.Commands
{
    internal abstract class BaseCommand
    {
        protected readonly IServiceProvider ServiceProvider;

        protected BaseCommand(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [Option(Templates.BaudRate, "The baud rate.", CommandOptionType.SingleValue)]
        public int BaudRate { get; set; } = 230400;

        [Required]
        [Option(Templates.SerialPort, "The serial port the RfmUsb device is connected.", CommandOptionType.SingleValue)]
        public string SerialPort { get; set; }

        protected abstract IRfm? CreatDeviceInstance();

        protected int ExecuteCommand(Func<IRfm, int> action)
        {
            int result = -1;

            IRfm? rfmDevice = null;

            try
            {
                rfmDevice = CreatDeviceInstance();

                if (rfmDevice != null)
                {
                    rfmDevice.Open(SerialPort, BaudRate);

                    rfmDevice.DioInterrupt += RfmDeviceDioInterrupt;

                    action(rfmDevice);
                }
                else
                {
                    Console.Error.WriteLine($"Unable to resolve RfmUsb device.");
                }
            }
            catch(RfmUsbInvalidDeviceTypeException ex)
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
            }

            return result;
        }

        private void RfmDeviceDioInterrupt(object? sender, DioIrq e)
        {
            HandleDioInterrupt(e);
        }

        protected abstract void HandleDioInterrupt(DioIrq e);
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.Error.WriteLine("You must specify a command. See --help for more details.");
            app.ShowHelp();

            return 0;
        }
    }
}