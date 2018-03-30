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

        public bool IsFloat(float number)
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

            if (IsFloat(packetsFloat))
            {
                packetsNumber = (int)packetsFloat + 1;
            }
            else
            {
                packetsNumber = (int)packetsFloat;
            }
            Console.WriteLine("Packets needed: " + packetsNumber);
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
                        packet[j] = data[dataPos];
                        dataPos++;
                    }

                    packets[i] = packet;
                }

                return packets;
            }
        }

        private void PrintMatrix(byte[][] matrix)
        {
            Console.Write("[" + Environment.NewLine);

            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write(i + ") ");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine("");
            }


            Console.Write("]" + Environment.NewLine);
        }

        private byte[][] BuildRandomPackets(int length)
        {
            byte[] data = new byte[length];
            FillRandomPacket(data);
            return PreparePackets(data);
        }


        private void FillRandomPacket(byte[] packet)
        {
            for (int i = 0; i < packet.Length; i++)
            {
                packet[i] = (byte)NumbersHelper.GenerateNumber(0, 100);
            }
        }

        public void TryPreparePackets()
        {
            byte[][] matrix = BuildRandomPackets(300 * 100);
            PrintMatrix(matrix);
        }
    }
}
