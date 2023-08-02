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

namespace RfmUsb.Net
{
    internal static class Commands
    {
        public const string ExecuteAfcClear = "e-ac";
        public const string ExecuteAfcStart = "e-a";
        public const string ExecuteAgcStart = "e-as";
        public const string ExecuteBootloader = "e-bl";
        public const string ExecuteFeiStart = "e-fei";
        public const string ExecuteImageCalibration = "e-ic";
        public const string ExecuteListenModeAbort = "e-lma";
        public const string ExecuteMeasureTemperature = "e-tm";
        public const string ExecuteRcCalibration = "e-rc";
        public const string ExecuteReset = "e-r";
        public const string ExecuteRestartRx = "e-rr";
        public const string ExecuteRestartRxWithoutPllLock = "e-rrwop";
        public const string ExecuteRestartRxWithPllLock = "e-rrwp";
        public const string ExecuteSequencerStart = "e-ss";
        public const string ExecuteSequencerStop = "e-sst";
        public const string ExecuteSetAesKey = "s-aes";
        public const string ExecuteStartRssi = "e-rssi";
        public const string ExecuteTransmit = "e-tx";
        public const string ExecuteTransmitReceive = "e-txrx";
        public const string GetAccessSharedRegisters = "g-asr";
        public const string GetAddressFiltering = "g-af";
        public const string GetAesOn = "g-ae";
        public const string GetAfc = "g-afc";
        public const string GetAfcAutoClear = "g-aac";
        public const string GetAfcAutoOn = "g-aao";
        public const string GetAfcLowBetaOn = "g-ab";
        public const string GetAgcAutoOn = "g-agcao";
        public const string GetAutoImageCalibrationOn = "g-aico";
        public const string GetAutoModeEnterCondition = "g-amec";
        public const string GetAutoModeExitCondition = "g-amexc";
        public const string GetAutoRestartRxMode = "g-arrm";
        public const string GetAutoRxRestartOn = "g-arre";
        public const string GetBeaconOn = "g-bo";
        public const string GetBitRate = "g-br";
        public const string GetBitRateFractional = "g-brf";
        public const string GetBitSyncOn = "g-bs";
        public const string GetBroadcastAddress = "g-ba";
        public const string GetContinuousDagc = "g-cd";
        public const string GetCrcAutoClearOff = "g-caco";
        public const string GetCrcOn = "g-crc";
        public const string GetCrcWhiteningType = "g-cwt";
        public const string GetCurrentLnaGain = "g-lnag";
        public const string GetDataMode = "g-dm";
        public const string GetDccFreq = "g-df";
        public const string GetDccFreqAfc = "g-dfa";
        public const string GetDcFree = "g-dfe";
        public const string GetDioInterruptMask = "g-dim";
        public const string GetDioMapping = "g-dio";
        public const string GetErrorCodingRate = "g-ecr";
        public const string GetFastHopOn = "g-fh";
        public const string GetFei = "g-fei";
        public const string GetFifo = "g-fifo";
        public const string GetFifoAddressPointer = "g-fap";
        public const string GetFifoFill = "g-ffc";
        public const string GetFifoRxBaseAddress = "g-frba";
        public const string GetFifoRxByteAddressPointer = "g-frbap";
        public const string GetFifoRxBytesNumber = "g-frbn";
        public const string GetFifoRxCurrentAddress = "g-frca";
        public const string GetFifoThreshold = "g-ft";
        public const string GetFifoTxBaseAddress = "g-ftba";
        public const string GetFirmwareVersion = "g-fv";
        public const string GetFormerTemperatureValue = "g-fmt";
        public const string GetFreqError = "g-fe";
        public const string GetFreqHoppingPeriod = "g-fhp";
        public const string GetFrequency = "g-f";
        public const string GetFrequencyDeviation = "g-fd";
        public const string GetFromIdle = "g-fi";
        public const string GetFromPacketReceived = "g-fpr";
        public const string GetFromReceive = "g-fr";
        public const string GetFromRxTimeout = "g-frto";
        public const string GetFromStart = "g-fs";
        public const string GetFromTransmit = "g-frmt";
        public const string GetFskModulationShaping = "g-fs";
        public const string GetHopChannel = "g-hc";
        public const string GetIdleMode = "g-im";
        public const string GetImpedance = "g-lnaz";
        public const string GetImplicitHeaderModeOn = "g-ihm";
        public const string GetIntermediateMode = "g-im";
        public const string GetInterPacketRxDelay = "g-iprd";
        public const string GetIoHomeOn = "g-iho";
        public const string GetIoHomePowerFrame = "g-ihpf";
        public const string GetIrqFlags = "g-irq";
        public const string GetLastPacketSnr = "g-lpsnr";
        public const string GetLastRssiAfterRxReady = "g-lrssi";
        public const string GetListenCoefficentIdle = "g-lic";
        public const string GetListenCoefficentRx = "g-lrc";
        public const string GetListenCriteria = "g-lc";
        public const string GetListenEnd = "g-lem";
        public const string GetListenerOn = "g-lo";
        public const string GetListenResolutionIdle = "g-ir";
        public const string GetListenResolutionRx = "g-rr";
        public const string GetLnaBoostHf = "g-lbhf";
        public const string GetLnaGainSelect = "g-lnags";
        public const string GetLongRangeMode = "g-lrm";
        public const string GetLoraAgcAutoOn = "g-lagcao";
        public const string GetLoraIrqFlags = "g-lirq";
        public const string GetLoraIrqFlagsMask = "g-lim";
        public const string GetLoraMode = "g-lm";
        public const string GetLoraPayloadLength = "g-lpl";
        public const string GetLowBatteryOn = "g-lbo";
        public const string GetLowBatteryTrim = "g-lbt";
        public const string GetLowBetaAfcOffset = "g-lbao";
        public const string GetLowDataRateOptimize = "g-ldro";
        public const string GetLowFrequencyMode = "g-lfm";
        public const string GetLowPowerSelection = "g-lps";
        public const string GetMapPreambleDetect = "g-mpd";
        public const string GetMode = "g-om";
        public const string GetModemBandwidth = "g-bw";
        public const string GetModemStatus = "g-ms";
        public const string GetModulationType = "g-mt";
        public const string GetNodeAddress = "g-na";
        public const string GetOcpEnable = "g-ocp";
        public const string GetOcpTrim = "g-ocpt";
        public const string GetOokAverageOffset = "g-oao";
        public const string GetOokAverageThresholdFilter = "g-oatf";
        public const string GetOokFixedThreshold = "g-oft";
        public const string GetOokModulationShaping = "g-os";
        public const string GetOokPeakThresholdDec = "g-optd";
        public const string GetOokPeakThresholdStep = "g-opts";
        public const string GetOokThresholdType = "g-ott";
        public const string GetOutputPower = "g-op";
        public const string GetPacketFormat = "g-pf";
        public const string GetPacketRssi = "g-ps";
        public const string GetPaRamp = "g-par";
        public const string GetPayloadLength = "g-pl";
        public const string GetPayloadMaxLength = "g-pml";
        public const string GetPpmCorrection = "g-ppm";
        public const string GetPreambleDetectorOn = "g-pdo";
        public const string GetPreambleDetectorSize = "g-pds";
        public const string GetPreambleDetectorTotalerance = "g-pdt";
        public const string GetPreambleLength = "g-prl";
        public const string GetPreamblePolarity = "g-pp";
        public const string GetPreambleSize = "g-ps";
        public const string GetRadioVersion = "g-v";
        public const string GetRestartRxOnCollision = "g-rroc";
        public const string GetRssi = "g-r";
        public const string GetRssiCollisionThreshold = "g-rct";
        public const string GetRssiOffset = "g-ro";
        public const string GetRssiSmoothing = "g-rs";
        public const string GetRssiThreshold = "g-rt";
        public const string GetRssiWideband = "g-rwb";
        public const string GetRxBw = "g-rxbw";
        public const string GetRxBwAfc = "g-rxbwa";
        public const string GetRxCodingRate = "g-rcr";
        public const string GetRxPayloadCrcOn = "g-rpc";
        public const string GetSensitivityBoost = "g-sb";
        public const string GetSequencer = "g-so";
        public const string GetSerialNumber = "g-mcu";
        public const string GetSpreadingFactor = "g-sf";
        public const string GetSymbolTimeout = "g-st";
        public const string GetSync = "g-sync";
        public const string GetSyncBitErrors = "g-sbe";
        public const string GetSyncEnable = "g-se";
        public const string GetSyncSize = "g-ss";
        public const string GetTcxoInputOn = "g-tio";
        public const string GetTemperatureChange = "g-tpc";
        public const string GetTemperatureThreshold = "g-tt";
        public const string GetTemperatureValue = "g-t";
        public const string GetTempMonitorOff = "g-tm";
        public const string GetTimeoutRssiThreshold = "g-trt";
        public const string GetTimeoutRxPreamble = "g-trp";
        public const string GetTimeoutRxRssi = "g-trr";
        public const string GetTimeoutRxStart = "g-trs";
        public const string GetTimeoutSignalSync = "g-tss";
        public const string GetTimerCoefficient = "g-tc";
        public const string GetTimerResolution = "g-tr";
        public const string GetTxContinuousMode = "g-tcm";
        public const string GetTxStartCondition = "g-tsc";
        public const string GetValidHeaderCount = "g-vhc";
        public const string GetValidPacketCount = "g-vpc";
        public const string SetAccessSharedRegisters = "s-asr";
        public const string SetAddressFiltering = "s-af";
        public const string SetAesOn = "s-ae";
        public const string SetAfcAutoClear = "s-aac";
        public const string SetAfcAutoOn = "s-aao";
        public const string SetAfcLowBetaOn = "s-ab";
        public const string SetAgcAutoOn = "s-agcao";
        public const string SetAutoImageCalibrationOn = "s-aico";
        public const string SetAutoModeEnterCondition = "s-amec";
        public const string SetAutoModeExitCondition = "s-amexc";
        public const string SetAutoRestartRxMode = "s-arrm";
        public const string SetAutoRxRestartOn = "s-arre";
        public const string SetBeaconOn = "s-bo";
        public const string SetBitRate = "s-br";
        public const string SetBitRateFractional = "s-brf";
        public const string SetBitSyncOn = "s-bs";
        public const string SetBroadcastAddress = "s-ba";
        public const string SetContinuousDagc = "s-cd";
        public const string SetCrcAutoClearOff = "s-caco";
        public const string SetCrcOn = "s-crc";
        public const string SetCrcWhiteningType = "s-cwt";
        public const string SetDataMode = "s-dm";
        public const string SetDccFreq = "s-df";
        public const string SetDccFreqAfc = "s-dfa";
        public const string SetDcFree = "s-dfe";
        public const string SetDioInterruptMask = "s-dim";
        public const string SetDioMapping = "s-dio";
        public const string SetErrorCodingRate = "s-ecr";
        public const string SetFastHopOn = "s-fh";
        public const string SetFifo = "s-fifo";
        public const string SetFifoAddressPointer = "s-fap";
        public const string SetFifoFill = "s-ffc";
        public const string SetFifoRxBaseAddress = "s-frba";
        public const string SetFifoThreshold = "s-ft";
        public const string SetFifoTxBaseAddress = "s-ftba";
        public const string SetFormerTemperatureValue = "s-fmt";
        public const string SetFreqHoppingPeriod = "s-fhp";
        public const string SetFrequency = "s-f";
        public const string SetFrequencyDeviation = "s-fd";
        public const string SetFromIdle = "s-fi";
        public const string SetFromPacketReceived = "s-fpr";
        public const string SetFromReceive = "s-fr";
        public const string SetFromRxTimeout = "s-frt";
        public const string SetFromStart = "s-fs";
        public const string SetFromTransmit = "s-frmt";
        public const string SetFskModulationShaping = "s-fs";
        public const string SetIdleMode = "s-im";
        public const string SetImpedance = "s-lnaz";
        public const string SetImplicitHeaderModeOn = "s-ihm";
        public const string SetIntermediateMode = "s-im";
        public const string SetInterPacketRxDelay = "s-iprd";
        public const string SetIoHomeOn = "s-iho";
        public const string SetIoHomePowerFrame = "s-ihpf";
        public const string SetIrqFlags = "s-irq";
        public const string SetListenCoefficentIdle = "s-lic";
        public const string SetListenCoefficentRx = "s-lrc";
        public const string SetListenCriteria = "s-lc";
        public const string SetListenEnd = "s-lem";
        public const string SetListenerOn = "s-lo";
        public const string SetListenResolutionIdle = "s-ir";
        public const string SetListenResolutionRx = "s-rr";
        public const string SetLnaBoostHf = "s-lbhf";
        public const string SetLnaGainSelect = "s-lnags";
        public const string SetLongRangeMode = "s-lrm";
        public const string SetLoraAgcAutoOn = "s-lagcao";
        public const string SetLoraIrqFlags = "s-irq";
        public const string SetLoraIrqFlagsMask = "s-lim";
        public const string SetLoraMode = "s-lm";
        public const string SetLoraPayloadLength = "s-lpl";
        public const string SetLowBatteryOn = "s-lbo";
        public const string SetLowBatteryTrim = "s-lbt";
        public const string SetLowBetaAfcOffset = "s-lbao";
        public const string SetLowDataRateOptimize = "s-ldro";
        public const string SetLowFrequencyMode = "s-lfm";
        public const string SetLowPowerSelection = "s-lps";
        public const string SetMapPreambleDetect = "s-mpd";
        public const string SetMode = "s-om";
        public const string SetModemBandwidth = "s-bw";
        public const string SetModulationType = "s-mt";
        public const string SetNodeAddress = "s-na";
        public const string SetOcpEnable = "s-ocp";
        public const string SetOcpTrim = "s-ocpt";
        public const string SetOokAverageOffset = "s-oao";
        public const string SetOokAverageThresholdFilter = "s-oatf";
        public const string SetOokFixedThreshold = "s-oft";
        public const string SetOokModulationShaping = "s-os";
        public const string SetOokPeakThresholdDec = "s-optd";
        public const string SetOokPeakThresholdStep = "s-opts";
        public const string SetOokThresholdType = "s-ott";
        public const string SetOutputbase = "s-ob";
        public const string SetOutputPower = "s-op";
        public const string SetPacketFormat = "s-pf";
        public const string SetPaRamp = "s-par";
        public const string SetPayloadLength = "s-pl";
        public const string SetPayloadMaxLength = "s-pml";
        public const string SetPpmCorrection = "s-ppm";
        public const string SetPreambleDetectorOn = "s-pdo";
        public const string SetPreambleDetectorSize = "s-pds";
        public const string SetPreambleDetectorTotalerance = "s-pdt";
        public const string SetPreambleLength = "s-prl";
        public const string SetPreamblePolarity = "s-pp";
        public const string SetPreambleSize = "s-ps";
        public const string SetRestartRxOnCollision = "s-rroc";
        public const string SetRssiCollisionThreshold = "s-rct";
        public const string SetRssiOffset = "s-ro";
        public const string SetRssiSmoothing = "s-rs";
        public const string SetRssiThreshold = "s-rt";
        public const string SetRxBw = "s-rxbw";
        public const string SetRxBwAfc = "s-rxbwa";
        public const string SetRxPayloadCrcOn = "s-rpc";
        public const string SetSensitivityBoost = "s-sb";
        public const string SetSequencer = "s-so";
        public const string SetSpreadingFactor = "s-sf";
        public const string SetSymbolTimeout = "s-st";
        public const string SetSync = "s-sync";
        public const string SetSyncBitErrors = "s-sbe";
        public const string SetSyncEnable = "s-se";
        public const string SetSyncSize = "s-ss";
        public const string SetTcxoInputOn = "s-tio";
        public const string SetTemperatureThreshold = "s-tt";
        public const string SetTempMonitorOff = "s-tm";
        public const string SetTimeoutRssiThreshold = "s-trt";
        public const string SetTimeoutRxPreamble = "s-trp";
        public const string SetTimeoutRxRssi = "s-trr";
        public const string SetTimeoutRxStart = "s-trs";
        public const string SetTimeoutSignalSync = "s-tss";
        public const string SetTimerCoefficient = "s-tc";
        public const string SetTimerResolution = "s-tr";
        public const string SetTxContinuousMode = "s-tcm";
        public const string SetTxStartCondition = "s-tsc";
    }
}