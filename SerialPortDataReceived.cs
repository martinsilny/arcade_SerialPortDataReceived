using System;
using System.IO.Ports;
using WindowsInput.Native;
using WindowsInput;

namespace SerialPortDataReceived
{
    class SerialPortDataReceived
    {                                            
        private InputSimulator              inputSimulator;
        private const String                portNum = "COM3";                
        private readonly VirtualKeyCode[]   keyToPress =
        {
            VirtualKeyCode.UP, VirtualKeyCode.DOWN, VirtualKeyCode.LEFT, VirtualKeyCode.RIGHT, VirtualKeyCode.RSHIFT, VirtualKeyCode.RETURN,
            VirtualKeyCode.VK_Z, VirtualKeyCode.VK_A, VirtualKeyCode.VK_X, VirtualKeyCode.VK_S, VirtualKeyCode.VK_Q, VirtualKeyCode.VK_W
        };

        public static void Main(string[] args)
        {
            SerialPortDataReceived cls = new SerialPortDataReceived();
            cls.Run();            
        }

        public void Run()
        {
            inputSimulator = new InputSimulator();

            SerialPort mySerialPort     = new SerialPort(portNum);
            mySerialPort.BaudRate       = 9600;
            mySerialPort.Parity         = Parity.None;
            mySerialPort.StopBits       = StopBits.One;
            mySerialPort.DataBits       = 8;
            mySerialPort.Handshake      = Handshake.None;
            mySerialPort.RtsEnable      = true;
            mySerialPort.DataReceived   += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.Open();

            Console.WriteLine("Listening on " + portNum + ", press escape to exit...");            

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {               
            }

            mySerialPort.Close();
            Console.WriteLine("Exit...");
        }

        private void DataReceivedHandler(object _sender, SerialDataReceivedEventArgs _e)
        {            
            SerialPort  sp          = (SerialPort)_sender;
            int         readValue   = sp.ReadByte();                        
            int         btnNum      = (readValue - 100) / 10;
            Console.Write(readValue);

            if (btnNum >= 0 && btnNum <= 11)
            {
                if (readValue % 10 == 1)
                {
                    inputSimulator.Keyboard.KeyDown(keyToPress[btnNum]);
                }
                else if (readValue % 10 == 0)
                {
                    inputSimulator.Keyboard.KeyUp(keyToPress[btnNum]);
                }
            }            
        }
    }
}
