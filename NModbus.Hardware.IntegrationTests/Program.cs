using System;

namespace NModbus.Hardware.IntegrationTests
{
    using System.Net.Sockets;
    using NModbus.Hardware.Schneider.Servo.ILA2T;

    class Program
    {
        static void Main(string[] args)
        {
            Program.ModbusTcpMasterReadInputs();
        }

        public static void ModbusTcpMasterReadInputs()
        {
            System.Console.WriteLine("Connecting to turntable.");
            using (TcpClient client = new TcpClient("192.168.100.10", 502))
            {
                var factory = new ModbusFactory();

                IModbusMaster master = factory.CreateMaster(client);
                var schniederMotion = new ModbusMaster(master);
                System.Console.WriteLine("Connected");
                var pos = schniederMotion.Possition;
                System.Console.WriteLine($"Current possition: {pos}");
                


                //Wild goose chase
                //master.WriteMultipleRegisters(1, 6922, new ushort[] { 0, 0 });
                //master.WriteMultipleRegisters(255, 6922, new ushort[] { 0, 2 });
                //master.WriteMultipleRegisters(1, 6922, new ushort[] { 0, 0 });
                //master.WriteMultipleRegisters(1, 6922, new ushort[] { 0, 0 });
                //master.WriteMultipleRegisters(1, 6922, new ushort[] { 0, 0 });
                //master.WriteMultipleRegisters(255, 6922, new ushort[] { 0, 1000 });

                //master.WriteMultipleRegisters(255, 6922, new ushort[] { (512 + 3 + 128), 1000 });
                //enhancedModbus.WriteUIntHoldingRegisters(255, 6922, new uint[] { 33554432 });

                //enhancedModbus.WriteUshortHoldingRegisters(255, 6922, new ushort[] { 512 });
                //enhancedModbus.WriteUshortHoldingRegisters(255, 6922, new ushort[] { 256 });
                //enhancedModbus.WriteUshortHoldingRegisters(255, 6922, new ushort[] { 512 });
                //enhancedModbus.WriteUshortHoldingRegisters(255, 6922, new ushort[] { (512 + 32 + 3), 1900 });


                try
                {
                    System.Console.WriteLine("Trying to access lock turntable");
                    var accesslock = schniederMotion.ReadUshortHoldingRegisters(1, 282, 1)[0];
                    if (accesslock == 0)
                        schniederMotion.WriteUshortHoldingRegisters(1, 282, new ushort[] { 1 });
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    accesslock = schniederMotion.ReadUshortHoldingRegisters(1, 282, 1)[0];
                    var control = schniederMotion.ReadUshortHoldingRegisters(1, 6914, 1)[0];
                    if (accesslock == 1)
                    {
                        System.Console.WriteLine("Access locked");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 128 });
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    System.Console.WriteLine("Turning on power train.");
                    schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 6 });
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    control = schniederMotion.ReadUshortHoldingRegisters(1, 6914, 1)[0];
                    if (control == 6)
                    {
                        System.Console.WriteLine("Setting Operation ready.");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    control = schniederMotion.ReadUshortHoldingRegisters(1, 6914, 1)[0];
                    if (control == 15)
                    {
                        System.Console.WriteLine("Setting Operation Mode Homing");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6918, new ushort[] { 6 });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        schniederMotion.WriteUshortHoldingRegisters(1, 6936, new ushort[] { 24 });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        schniederMotion.WriteUshortHoldingRegisters(1, 10250, new ushort[] { 40 });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        schniederMotion.WriteUshortHoldingRegisters(1, 10248, new ushort[] { 600 });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        schniederMotion.WriteIntHoldingRegisters(1, 10252, new int[] { (360 * 200000) });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        schniederMotion.WriteIntHoldingRegisters(1, 10262, new int[] { (int)(-3.2*200000) });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        System.Console.WriteLine("Home (+45sec wait)");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(20));
                    }
                    schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                    var opmode = schniederMotion.ReadUshortHoldingRegisters(1, 6918, 1)[0];

                    if (control == 15)
                    {
                        System.Console.WriteLine("Set operation mode Profile Possition");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6918, new ushort[] { 1 });
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    opmode = schniederMotion.ReadUshortHoldingRegisters(1, 6918, 1)[0];
                    if (opmode == 1)
                    {
                        System.Console.WriteLine("Set Max speed 15.1rpm");
                        schniederMotion.WriteUshortHoldingRegisters(1, 4612, new ushort[] { 3100 });
                        var timespan = TimeSpan.FromSeconds(5);
                        schniederMotion.Possition = 0*200000;
                        System.Console.WriteLine("Set Target speed 15rpm");
                        ushort speed = 10*200;
                        schniederMotion.Speed = speed;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.Possition = 45 * 200000;
                        schniederMotion.Speed = speed;
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        schniederMotion.Possition = 135 * 200000;
                        schniederMotion.Speed = speed;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        schniederMotion.Possition = 180 * 200000;
                        schniederMotion.Speed = speed;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        schniederMotion.Possition = 225 * 200000;
                        schniederMotion.Speed = speed;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        schniederMotion.Possition = 315 * 200000;
                        schniederMotion.Speed = speed;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        schniederMotion.Possition = 360 * 200000;
                        schniederMotion.Speed = speed;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
                        System.Threading.Thread.Sleep(timespan);
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                        schniederMotion.Speed = speed;
                        schniederMotion.Possition = 0;
                        System.Console.WriteLine($"Move to {schniederMotion.Possition} degrees");
                        schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });



                    }
                    schniederMotion.WriteUshortHoldingRegisters(1, 282, new ushort[] { 0 });


                }
                catch
                {
                }
                //try
                //{
                //  enhancedModbus.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
                //}
                //catch { }

                //var operatingMode = schniederMotion.ControlMode;
                //schniederMotion.WriteUIntHoldingRegisters(1,6914, new uint[] { 6 });
                //schniederMotion.SwitchOn = true;
                //schniederMotion.EnableVoltage = true;
                //schniederMotion.QuickStop = true;
                //schniederMotion.EnableOperation = true;

                //operatingMode = schniederMotion.ControlMode;
                //System.Console.WriteLine($"OpertaionMode: {operatingMode}");




                //var encoder = enhancedModbus.ReadIntHoldingRegisters(1, 7706, 1);
                //var newvalue = encoder[0] + 10;
                //enhancedModbus.WriteIntHoldingRegisters(1,1324, new int[]{newvalue});

            }
        }
    }
}
