/*
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

namespace RfmUsb.Net
{
    internal static class Commands
    {
        public const string ExecuteAfcClear = "e-ac";
        public const string ExecuteAfcStart = "e-a";
        public const string ExecuteBootloader = "e-bl";
        public const string ExecuteFeiStart = "e-fei";
        public const string ExecuteListenAbort = "e-lma";
        public const string ExecuteMeasureTemperature = "e-tm";
        public const string ExecuteRcCalibration = "e-rc";
        public const string ExecuteReset = "e-r";
        public const string ExecuteRestartRx = "e-rr";
        public const string ExecuteSetAesKey = "s-aes";
        public const string ExecuteStartRssi = "e-rssi";
        public const string ExecuteTransmit = "e-tx";
        public const string ExecuteTransmitReceive = "e-txrx";
        public const string GetAddressFiltering = "g-af";
        public const string GetAesOn = "g-ae";
        public const string GetAfc = "g-a";
        public const string GetAfcAutoClear = "g-aac";
        public const string GetAfcAutoOn = "g-aao";
        public const string GetAfcLowBetaOn = "g-ab";
        public const string GetAutoRxRestartOn = "g-arre";
        public const string GetBitRate = "g-br";
        public const string GetBroadcastAddress = "g-ba";
        public const string GetContinuousDagc = "g-cd";
        public const string GetCrcAutoClear = "g-caco";
        public const string GetCrcOn = "g-cc";
        public const string GetCurrentLnaGain = "g-lnag";
        public const string GetDccFreq = "g-df";
        public const string GetDccFreqAfc = "g-dfa";
        public const string GetDcFree = "g-dfe";
        public const string GetDio = "g-di";
        public const string GetDioInterrupt = "g-di";
        public const string GetDioMapping = "g-dio";
        public const string GetEnterCondition = "g-amec";
        public const string GetExitCondition = "g-amexc";
        public const string GetFei = "g-a";
        public const string GetFifo = "g-fifo";
        public const string GetFifoFill = "g-ffc";
        public const string GetFifoThreshold = "g-ft";
        public const string GetFrequency = "g-f";
        public const string GetFrequencyDeviation = "g-fd";
        public const string GetFskModulationShaping = "g-fs";
        public const string GetImpedance = "g-lnaz";
        public const string GetIntermediateMode = "g-im";
        public const string GetInterPacketRxDelay = "g-iprd";
        public const string GetIrq = "g-irq";
        public const string GetListenCoefficentIdle = "g-lic";
        public const string GetListenCoefficentRx = "g-lrc";
        public const string GetListenCriteria = "g-lc";
        public const string GetListenEnd = "g-lem";
        public const string GetListenerOn = "g-lo";
        public const string GetListenResolutionIdle = "g-ir";
        public const string GetListenResolutionRx = "g-rr";
        public const string GetLnaGainSelect = "g-lnags";
        public const string GetLowBetaAfcOffset = "g-lbao";
        public const string GetMode = "g-om";
        public const string GetModulation = "g-mt";
        public const string GetNodeAddress = "g-na";
        public const string GetOcpEnable = "g-ocp";
        public const string GetOcpTrim = "g-ocpt";
        public const string GetOokAverageThresholdFilter = "g-oatf";
        public const string GetOokFixedThreshold = "g-oft";
        public const string GetOokModulationShaping = "g-os";
        public const string GetOokPeakThresholdDec = "g-optd";
        public const string GetOokPeakThresholdStep = "g-ots";
        public const string GetOokThresholdType = "g-ott";
        public const string GetOutputPower = "g-op";
        public const string GetPacketFormat = "g-pf";
        public const string GetPaRamp = "g-par";
        public const string GetPayloadLength = "g-pl";
        public const string GetPreambleSize = "g-ps";
        public const string GetRadioConfig = "g-rc";
        public const string GetRssi = "g-rssi";
        public const string GetRssiThreshold = "g-rt";
        public const string GetRxBw = "g-rxbw";
        public const string GetRxBwAfc = "g-rxbwa";
        public const string GetSensitivityBoost = "g-sb";
        public const string GetSequencer = "g-so";
        public const string GetSync = "g-sync";
        public const string GetSyncBitErrors = "g-sbe";
        public const string GetSyncEnable = "g-se";
        public const string GetSyncSize = "g-ss";
        public const string GetTemperatureValue = "g-t";
        public const string GetTimeoutRssiThreshold = "g-trt";
        public const string GetTimeoutRxStart = "g-trs";
        public const string GetTxStartCondition = "g-tsc";
        public const string GetVersion = "g-fv";
        public const string SetAddressFiltering = "s-af";
        public const string SetAesOn = "s-ae";
        public const string SetAfcAutoClear = "s-aac";
        public const string SetAfcAutoOn = "s-aao";
        public const string SetAfcLowBetaOn = "s-ab";
        public const string SetAutoRxRestartOn = "s-arre";
        public const string SetBitRate = "s-br";
        public const string SetBroadcastAddress = "s-ba";
        public const string SetContinuousDagc = "s-cd";
        public const string SetCrcAutoClear = "s-caco";
        public const string SetCrcOn = "s-cc";
        public const string SetDccFreq = "s-df";
        public const string SetDccFreqAfc = "s-dfa";
        public const string SetDcFree = "s-dfe";
        public const string SetDio = "s-di";
        public const string SetDioInterrupt = "s-di";
        public const string SetDioMapping = "s-dio";
        public const string SetEnterCondition = "s-amec";
        public const string SetExitCondition = "s-amexc";
        public const string SetFifo = "s-fifo";
        public const string SetFifoFill = "s-ffc";
        public const string SetFifoThreshold = "s-ft";
        public const string SetFrequency = "s-f";
        public const string SetFrequencyDeviation = "s-fd";
        public const string SetFskModulationShaping = "s-fs";
        public const string SetImpedance = "s-lnaz";
        public const string SetIntermediateMode = "s-im";
        public const string SetInterPacketRxDelay = "s-iprd";
        public const string SetListenCoefficentIdle = "s-lic";
        public const string SetListenCoefficentRx = "s-lrc";
        public const string SetListenCriteria = "s-lc";
        public const string SetListenEnd = "s-lem";
        public const string SetListenerOn = "s-lo";
        public const string SetListenResolutionIdle = "s-ir";
        public const string SetListenResolutionRx = "s-rr";
        public const string SetLnaGainSelect = "s-lnags";
        public const string SetLowBetaAfcOffset = "s-lbao";
        public const string SetMode = "s-om";
        public const string SetModulation = "s-mt";
        public const string SetNodeAddress = "s-na";
        public const string SetOcpEnable = "s-ocp";
        public const string SetOcpTrim = "s-ocpt";
        public const string SetOokAverageThresholdFilter = "s-oatf";
        public const string SetOokFixedThreshold = "s-oft";
        public const string SetOokModulationShaping = "s-os";
        public const string SetOokPeakThresholdDec = "s-optd";
        public const string SetOokPeakThresholdStep = "s-ots";
        public const string SetOokThresholdType = "s-ott";
        public const string SetOutputPower = "s-op";
        public const string SetPacketFormat = "s-pf";
        public const string SetPaRamp = "s-par";
        public const string SetPayloadLength = "s-pl";
        public const string SetPreambleSize = "s-ps";
        public const string SetRadioConfig = "s-rc";
        public const string SetRssiThreshold = "s-rt";
        public const string SetRxBw = "s-rxbw";
        public const string SetRxBwAfc = "s-rxbwa";
        public const string SetSensitivityBoost = "s-sb";
        public const string SetSequencer = "s-so";
        public const string SetSync = "s-sync";
        public const string SetSyncBitErrors = "s-sbe";
        public const string SetSyncEnable = "s-se";
        public const string SetSyncSize = "s-ss";
        public const string SetTimeoutRssiThreshold = "s-trt";
        public const string SetTimeoutRxStart = "s-trs";
        public const string SetTxStartCondition = "s-tsc";
        public const string GetLoraAgcAutoOn = "g-laao";
        public const string SetLoraAgcAutoOn = "s-laao";
    }
}

