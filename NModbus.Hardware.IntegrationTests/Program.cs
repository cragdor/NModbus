using System;

namespace NModbus.Hardware.IntegrationTests
{
  using System.Net.Sockets;

  class Program
  {
    static void Main(string[] args)
    {
      Program.ModbusTcpMasterReadInputs();
    }

    public static void ModbusTcpMasterReadInputs()
    {
      using (TcpClient client = new TcpClient("192.168.100.10", 502))
      {
        var factory = new ModbusFactory();

        IModbusMaster master = factory.CreateMaster(client);
        var schniederMotion = new NModbus.Hardware.Schneider.Servo.ILA2T(master);

        var pos = schniederMotion.Possition;
        schniederMotion.Possition = 360;
        pos = schniederMotion.Possition;

        //var temp = master.ReadHoldingRegisters(1, 4612, 2);
        //read five input values
        var enhancedModbus = new NModbus.Extensions.ModbusMasterEnhanced(master, 32, NModbus.Extensions.Functions.Endian.LittleEndian);
        var floats = enhancedModbus.ReadUshortHoldingRegisters(1, 4612, 1);



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
          var accesslock = schniederMotion.ReadUshortHoldingRegisters(1, 282, 1)[0];
          if (accesslock == 0)
            schniederMotion.WriteUshortHoldingRegisters(1, 282, new ushort[] { 1 });
          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
          accesslock = schniederMotion.ReadUshortHoldingRegisters(1, 282, 1)[0];
          var control = schniederMotion.ReadUshortHoldingRegisters(1, 6914, 1)[0];
          if (accesslock == 1)

            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 128 });
          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
          schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 6 });
          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
          control = schniederMotion.ReadUshortHoldingRegisters(1, 6914, 1)[0];
          if (control == 6)
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
          control = schniederMotion.ReadUshortHoldingRegisters(1, 6914, 1)[0];
          //if (control == 15)
          //{
          //  schniederMotion.WriteUshortHoldingRegisters(1, 6918, new ushort[] { 6 });
          //  schniederMotion.WriteUshortHoldingRegisters(1, 6936, new ushort[] { 24 });
          //  schniederMotion.WriteUshortHoldingRegisters(1, 10250, new ushort[] { 400 });
          //  schniederMotion.WriteUshortHoldingRegisters(1, 10248, new ushort[] { 600 });

          //  schniederMotion.WriteIntHoldingRegisters(1, 10252, new int[] { (360*200000) });
          //  schniederMotion.WriteIntHoldingRegisters(1, 10262, new int[] { 0 });
          //  schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
          //}
          //schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
          var opmode = schniederMotion.ReadUshortHoldingRegisters(1, 6918, 1)[0];
          
          if (control == 15)
            schniederMotion.WriteUshortHoldingRegisters(1, 6918, new ushort[] { 1 });
          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
          opmode = schniederMotion.ReadUshortHoldingRegisters(1, 6918, 1)[0];
          if (opmode == 1)
          {
            schniederMotion.WriteUshortHoldingRegisters(1, 4612, new ushort[] { 3100 });
            var timespan = TimeSpan.FromSeconds(5);
            schniederMotion.Possition = 0;
            ushort speed = 15;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 720;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 45;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 135;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 180;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 225;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 315;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Possition = 360;
            schniederMotion.Speed = speed;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });
            System.Threading.Thread.Sleep(timespan);
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 15 });
            schniederMotion.Speed = speed;
            schniederMotion.Possition = 0;
            schniederMotion.WriteUshortHoldingRegisters(1, 6914, new ushort[] { 31 });



          }
          enhancedModbus.WriteUshortHoldingRegisters(1, 282, new ushort[] { 0 });


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
