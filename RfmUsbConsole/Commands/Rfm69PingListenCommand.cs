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

// Ignore Spelling: Rfm

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using RfmUsb.Net;

namespace RfmUsbConsole.Commands
{
    [Command(Description = "Ping listen using RfmUsb Rfm69 radio")]
    internal class Rfm69PingListenCommand : BaseRfm69Command
    {
        public Rfm69PingListenCommand(ILogger<Rfm69PingListenCommand> logger, IRfm69 rfm) : base(logger, rfm)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand(console, () =>
            {
                var rfm69 = (IRfm69)Rfm;

                if (OutputPower.HasValue)
                {
                    rfm69.OutputPower = OutputPower.Value;
                }

                rfm69.RssiThreshold = RssiThreshold;

                return ExecutePingListen(RxBw);
            });
        }
    }
}