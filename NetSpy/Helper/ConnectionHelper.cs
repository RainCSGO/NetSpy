using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetSpy.Helper
{
    public class ConnectionHelper
    {
        public const int Port = 6969;
        public const string Hostname = "127.0.0.1";
        private const int Buffer = 2048;

        private UdpClient MREServer;
        private UdpClient MREClient;
        private Thread listen;

        private void StartServer()
        {
            Console.WriteLine("Server creato.");
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, Port);
                var data = MREServer.Receive(ref remoteEP);
                byte[] recivedData = data.ToArray<byte>();
            }
        }

        public void StartAsyncListening()
        {
            listen = new Thread(StartServer);
            listen.Start();
        }

        public void StopAsyncListening()
        {
            listen.Abort();
        }

        public void SendData(byte[] data)
        {
            MREClient.Connect(IPAddress.Parse(Hostname), Port);
            MREClient.Send(data, data.Length);
        }

        public bool isFloat(float number)
        {
            if (number != (int) number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[][] PreparePackets(byte[] data)
        {
            float packetsFloat = data.Length / Buffer;
            int packetsNumber = 0;

            if (isFloat(packetsFloat))
            {
                packetsNumber = (int)packetsFloat + 1;
            }
            else
            {
                packetsNumber = (int)packetsFloat;
            }
            
            byte[][] packets = new byte[packetsNumber][];

            if (data.Length < Buffer)
            {
                packets[0] = data;
                return packets;
            }
            else
            {
                int dataPos = 0;
                for(int i = 0; i < packets.Length; i++)
                {
                    int remaningBytes = data.Length - dataPos;
                    byte[] packet;
                    if (remaningBytes < Buffer)
                    {
                        packet = new byte[remaningBytes];
                    }
                    else
                    {
                        packet = new byte[Buffer];
                    }

                    //BUILD PACKET
                    for (int j = 0; j < Buffer; j++)
                    {
                        packet[dataPos] = data[dataPos];
                        dataPos++;
                    }

                    packets[i] = packet;
                }

                return packets;
            }
        }

        private void printMatrix(object[][] matrix)
        {
            Console.Write("[" + Environment.NewLine);
            foreach (var rows in matrix)
            {
                foreach (var cell in rows)
                {
                    Console.Write(cell + " ");
                }
            }
            Console.Write(Environment.NewLine + "]" + Environment.NewLine);
        }

        private byte[][] buildRandomPackets()
        {

        }
    }
}
