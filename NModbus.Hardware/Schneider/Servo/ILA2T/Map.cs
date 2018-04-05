namespace NModbus.Hardware.Schneider.Servo.ILA2T
{
    using System;
    public class Map
    {
        public const ushort AccessStatusAddress = 280; //AccessInfo
        public const ushort AccessExclAddress = 282; //AccessCommand

        public const ushort ActionStatusAddress = 7176;
        public const ushort SpeedPrefStatusAddress = 7950;
        public const ushort SpeedActualAddress = 7696;
        public const ushort SpeedMaxAddress = 4612;
        public const ushort SpeedTargetAddress = 6942;

        public const ushort DCOMcompatibAddress = 6950;
        public const ushort DCOMcontrolAddress = 6914;
        public const ushort DCOMopmodeAddress = 6918;
        public const ushort DCOMstatusAddress = 6916;

        public const ushort TargetPossitionAddress = 6940;
        public const ushort TargetSpeedAddress = 6942;
        public const ushort ControlModeAddress = 6914;
        public const ushort OperatingModeAddress = 6918;

        public const ushort ControlModeBitSwitchOn = 0;
        public const ushort ControlModeBitVoltageOn = 1;
        public const ushort ControlModeBitQuickStop = 2;
        public const ushort ControlModeBitEnableOperation = 3;

        [Flags]
        public enum DCOMControl
        {
            SwitchOn = 1 << 0,
            EnableVoltage = 1 << 1,
            QuickStop = 1 << 2,
            EnableOperation = 1 << 3,
            // Bits 4->6 are mode specific
            FaultReset = 1 << 7,
            Halt = 1 << 8
            // Bits 9->15 are reserved and must be zero
        }

        [Flags]
        public enum DCOMControlProfilePosition
        {
            SwitchOn = 1 << 0,
            EnableVoltage = 1 << 1,
            QuickStop = 1 << 2,
            EnableOperation = 1 << 3,
            NewSetPoint = 1 << 4,
            ChangeSetImmediately = 1 << 5,
            RelativePositioning = 1 << 6,
            FaultReset = 1 << 7,
            Halt = 1 << 8
        }

        [Flags]
        public enum DCOMcontrolProfileVerlocity
        {
            SwitchOn = 1 << 0,
            EnableVoltage = 1 << 1,
            QuickStop = 1 << 2,
            EnableOperation = 1 << 3,
            NewSetPoint = 1 << 4,
            ChangeSetImmediately = 1 << 5,
            RelativePositioning = 1 << 6,
            FaultReset = 1 << 7,
            Halt = 1 << 8
        }

        [Flags]
        public enum DCOMcontrolHoming
        {
            SwitchOn = 1 << 0,
            EnableVoltage = 1 << 1,
            QuickStop = 1 << 2,
            EnableOperation = 1 << 3,
            Home = 1 << 4,
            ChangeSetImmediately = 1 << 5,
            RelativePositioning = 1 << 6,
            FaultReset = 1 << 7,
            Halt = 1 << 8
        }


        public enum DCOMOpmode
        {
            ProfilePosition = 1,
            ProfileVerlocity = 3,
            Homing = 6
        }

        [Flags]
        public enum DCOMStatus
        {
            // Bit 0-3,5,6: Status bits
            VoltageEnabled = 1 << 4, //Equiverlent to bits 0000000000001000
            Warning = 1 << 7,
            HALTRequestActive = 1 << 8,
            Remote = 1 << 9,
            TargetReached = 1 << 10,
            Reserved = 1 << 11, // ????
            OpModeSpecific = 1 << 12, // Decode using buisness logic
            x_err = 1 << 13, // Software limit switch hit
            x_end = 1 << 14, // Limit switch hit 
            ref_ok = 1 << 15
        }
    }
}
