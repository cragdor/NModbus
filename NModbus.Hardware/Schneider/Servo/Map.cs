namespace NModbus.Hardware.Schneider.Servo
{
    public partial class ILA2T
    {
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
        }
    }
}
