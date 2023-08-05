﻿/*
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

                rfm.DataMode = Rfm69DataMode.Packet;
                rfm.Mode = Mode.Rx;
                rfm.IntermediateMode = IntermediateMode.Sleep;
                rfm.AutoModeEnterCondition = EnterCondition.CrcOk;
                rfm.AutoModeExitCondition = ExitCondition.FifoNotEmpty;
                rfm.SetDioMapping(Dio.Dio0, DioMapping.DioMapping1);
                rfm.DioInterruptMask = DioIrq.Dio0;

                do
                {
                    var source = WaitForSignal();

                    if (source == SignalSource.Irq)
                    {

                    }
                    else
                    {
                        console.WriteLine("Ping Listen Stop");
                    }
                } while (true);

                return 0;
            });
        }
    }
}
