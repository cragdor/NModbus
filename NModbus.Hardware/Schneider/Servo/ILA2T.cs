namespace NModbus.Hardware.Schneider.Servo
{
    using System;
    using System.Collections;
    using NModbus.Extensions.Functions;
    public partial class ILA2T
    {
        private readonly IModbusMaster master;
        private readonly uint wordSize;
        private readonly Func<byte[], byte[]> endian;
        private readonly bool wordSwap;
        private readonly bool frontPadding;
        public const int EncoderScalling = 200000;
        public const ushort RpmScalling = 200;


        public ILA2T(IModbusMaster master)
        {
            this.master = master;
            this.wordSize = 32;
            this.endian = Endian.LittleEndian;
            this.wordSwap = true;
            this.frontPadding = false;
        }
        #region Motor Functions


        public ushort Speed
        {
            get
              => (ushort)(this.ReadUshortHoldingRegisters(1, Map.TargetSpeedAddress, 1)[0] / ILA2T.RpmScalling);
            set
              => this.WriteIntHoldingRegisters(1, Map.TargetSpeedAddress, new[] { (int)(value * ILA2T.RpmScalling) });
        }

        public int Possition
        {
            get
              => this.ReadIntHoldingRegisters(1, Map.TargetPossitionAddress, 1)[0] / ILA2T.EncoderScalling;
            set
              => this.WriteIntHoldingRegisters(1, Map.TargetPossitionAddress, new[] { (int)(value * ILA2T.EncoderScalling) });
        }

        public uint ControlMode
        {
            get
              => this.ReadUintHoldingRegisters(1, Map.ControlModeAddress, 1)[0];
            set
              => this.WriteUIntHoldingRegisters(1, Map.ControlModeAddress, new[] { value });
        }

        public int OperatingMode
        {
            get
              => this.ReadIntHoldingRegisters(1, Map.OperatingModeAddress, 1)[0];
            set
              => this.WriteIntHoldingRegisters(1, Map.OperatingModeAddress, new[] { value });
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }


        #endregion

        #region Read/Write Registers

        public ushort[] ReadUshortHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
            => RegisterFunctions.ByteValueArraysToUShorts(
              RegisterFunctions.ReadRegisters(slaveAddress, startAddress, numberOfPoints, this.master, this.wordSize, this.endian, this.wordSwap), this.frontPadding);

        public short[] ReadShortHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
          => RegisterFunctions.ByteValueArraysToShorts(
            RegisterFunctions.ReadRegisters(slaveAddress, startAddress, numberOfPoints, this.master, this.wordSize, this.endian, this.wordSwap), this.frontPadding);

        public uint[] ReadUintHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
          => RegisterFunctions.ByteValueArraysToUInts(
            RegisterFunctions.ReadRegisters(slaveAddress, startAddress, numberOfPoints, this.master, this.wordSize, this.endian, this.wordSwap), this.frontPadding);

        public int[] ReadIntHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
          => RegisterFunctions.ByteValueArraysToInts(
            RegisterFunctions.ReadRegisters(slaveAddress, startAddress, numberOfPoints, this.master, this.wordSize, this.endian, this.wordSwap), this.frontPadding);

        public float[] ReadFloatHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
          => RegisterFunctions.ByteValueArraysToFloats(
               RegisterFunctions.ReadRegisters(slaveAddress, startAddress, numberOfPoints, this.master, this.wordSize, this.endian, this.wordSwap), this.frontPadding);

        public void WriteUshortHoldingRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
          => RegisterFunctions.WriteRegistersFunc(
            slaveAddress,
            startAddress,
            RegisterFunctions.UShortsToByteValueArrays(data, this.wordSize, this.frontPadding),
            this.master,
            this.wordSize,
            this.endian,
            this.wordSwap);

        public void WriteShortHoldingRegisters(byte slaveAddress, ushort startAddress, short[] data)
          => RegisterFunctions.WriteRegistersFunc(
            slaveAddress,
            startAddress,
            RegisterFunctions.ShortsToByteValueArrays(data, this.wordSize, this.frontPadding),
            this.master,
            this.wordSize,
            this.endian,
            this.wordSwap);

        public void WriteIntHoldingRegisters(byte slaveAddress, ushort startAddress, int[] data)
          => RegisterFunctions.WriteRegistersFunc(
            slaveAddress,
            startAddress,
            RegisterFunctions.IntToByteValueArrays(data, this.wordSize, this.frontPadding),
            this.master,
            this.wordSize,
            this.endian,
            this.wordSwap);

        public void WriteUIntHoldingRegisters(byte slaveAddress, ushort startAddress, uint[] data)
          => RegisterFunctions.WriteRegistersFunc(
            slaveAddress,
            startAddress,
            RegisterFunctions.UIntToByteValueArrays(data, this.wordSize, this.frontPadding),
            this.master,
            this.wordSize,
            this.endian,
            this.wordSwap);

        public void WriteFloatHoldingRegisters(byte slaveAddress, ushort startAddress, float[] data)
          => RegisterFunctions.WriteRegistersFunc(
            slaveAddress,
            startAddress,
            RegisterFunctions.FloatToByteValueArrays(data, this.wordSize, this.frontPadding),
            this.master,
            this.wordSize,
            this.endian,
            this.wordSwap);
        #endregion
    }
}
