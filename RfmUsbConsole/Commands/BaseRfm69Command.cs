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
using System.ComponentModel.DataAnnotations;

namespace RfmUsbConsole.Commands
{
    internal abstract class BaseRfm69Command : BaseCommand
    {
        protected BaseRfm69Command(ILogger logger, IRfm69 rfm) : base(logger, rfm)
        {
        }

        [Range(-2, 20)]
        [Option(Templates.OutputPower, "The radio output power.", CommandOptionType.SingleValue)]
        public sbyte? OutputPower { get; set; }

        [Range(-115, 0)]
        [Option(Templates.RssiThreshold, "The Rssi Threshold", CommandOptionType.SingleValue)]
        public sbyte RssiThreshold { get; set; } = -40;

        [Range(0, 23)]
        [Option(Templates.RxBw, "The recieve filter bandwidth.", CommandOptionType.SingleValue)]
        public byte? RxBw { get; set; }
    }
}