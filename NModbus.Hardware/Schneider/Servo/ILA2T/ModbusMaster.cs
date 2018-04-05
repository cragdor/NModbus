namespace NModbus.Hardware.Schneider.Servo.ILA2T
{
    using System;
    using NModbus.Extensions.Functions;

    public class ModbusMaster
    {
        private readonly IModbusMaster master;
        private readonly uint wordSize;
        private readonly Func<byte[], byte[]> endian;
        private readonly bool wordSwap;
        private readonly bool frontPadding;


        public ModbusMaster(IModbusMaster master)
        {
            this.master = master;
            this.wordSize = 32;
            this.endian = Endian.LittleEndian;
            this.wordSwap = true;
            this.frontPadding = false;
        }
        #region Motor Functions


        public int Speed
        {
            get
              => this.ReadUshortHoldingRegisters(1, Map.TargetSpeedAddress, 1)[0];
            set
              => this.WriteIntHoldingRegisters(1, Map.TargetSpeedAddress, new[] { value });
        }

        public int Possition
        {
            get
              => this.ReadIntHoldingRegisters(1, Map.TargetPossitionAddress, 1)[0];
            set
              => this.WriteIntHoldingRegisters(1, Map.TargetPossitionAddress, new[] { value });
        }

        public ushort Control
        {
            get
              => this.ReadUshortHoldingRegisters(1, Map.DCOMcontrolAddress, 1)[0];
            set
              => this.WriteUshortHoldingRegisters(1, Map.DCOMcontrolAddress, new[] { value });
        }

        public ushort ControlModeStatus => this.ReadUshortHoldingRegisters(1, Map.DCOMstatusAddress, 1)[0];

        public int OperatingMode
        {
            get
              => this.ReadIntHoldingRegisters(1, Map.DCOMopmodeAddress, 1)[0];
            set
              => this.WriteIntHoldingRegisters(1, Map.DCOMopmodeAddress, new[] { value });
        }

        public int AcessLock
        {
            get
                => this.ReadIntHoldingRegisters(1, Map.AccessExclAddress, 1)[0];
            set
                => this.WriteIntHoldingRegisters(1, Map.AccessExclAddress, new[] { value });
        }

        public int AcessLockStatus => this.ReadIntHoldingRegisters(1, Map.OperatingModeAddress, 1)[0];

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
