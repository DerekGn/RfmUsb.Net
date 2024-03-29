﻿/*
* MIT License
*
* Copyright (c) 2023 Derek Goslin
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

using Microsoft.Extensions.DependencyInjection;
using RfmUsb.Net.Ports;
using System.Diagnostics.CodeAnalysis;

namespace RfmUsb.Net
{
    /// <summary>
    /// Extensions for the <see cref="IServiceCollection"/> to enable configuration of rfmusb dependencies
    /// </summary>
    public static class RfmUsbServiceExtensions
    {
        /// <summary>
        /// Add a singleton instance of an <see cref="IRfm69"/> implementation
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add the <see cref="IRfm69"/> and <see cref="IRfm9x"/> instance</param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddRfmUsb(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRfm69, Rfm69>();
            serviceCollection.AddSingleton<IRfm9x, Rfm9x>();
            serviceCollection.AddSerialPortFactory();
            return serviceCollection;
        }
    }
}