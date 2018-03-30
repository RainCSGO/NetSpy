using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSpy.Model
{
    /**
     *Questa classe sarà utilizzata dall'host
     */

    public class ConnectionUtility
    {
        public const int _PORT = 6969;
        public const string _HOSTNAME = "127.0.0.1";
        private const int _BUFFER = 4096;
        private const int _HEADER_FOOTER_NUMBER = 2;

        private UdpClient MREServer;
        private UdpClient MREClient;

        private CommandsUtility commandsUtility;

        private int packetSent = 0;
        private int bytesSent = 0;
        private int bytesRecived = 0;
        


        public Thread listen;

        public ConnectionUtility()
        {
            try
            {
                MREServer = new UdpClient(_PORT);
                MREClient = new UdpClient();
                commandsUtility = new CommandsUtility();
            }
            catch (Exception)
            {
                Console.WriteLine("Server Already Started");
            }
            
        }

        public void StartListening()
        {
            listen = new Thread(UDPListener);
            listen.Start();
        }

        public void StopListening()
        {
            listen.Abort();
        }

        private void UDPListener()
        {
            
            Console.WriteLine("Server creato.");
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, _PORT);
                var data = MREServer.Receive(ref remoteEP); // listen on port 6969
                byte[] recivedData = data.ToArray<byte>();
                string response = DecodeBytes(recivedData, false);

                //Console.Write("receive data from " + remoteEP.ToString() + " " + response);
                char separator = '\n';
                Console.Write("Recived From -> " + remoteEP.ToString());
                Console.WriteLine("......PACKET BYTES......");
                Console.WriteLine(response);
                Console.WriteLine("......PACKET DECODED......");
                string decodedResponse = DecodeBytes(recivedData, true);
                Console.WriteLine(decodedResponse);
                //checkCommands(decodedResponse);

                bytesRecived += response.Length;

                //controllerManager.labelBytesRecived.Text = bytesRecived.ToString();
            }
        }

        public void UDPClient(string hostname, string port, byte[] data,bool notCustomSend)
        {
            MREClient.Connect(IPAddress.Parse(_HOSTNAME), _PORT);
            
            //PACKET TRACER - Data Length

            if (!notCustomSend)
            {
                byte[][] packets = PreparePackets(data, _BUFFER);
                SendAllPackets(packets, MREClient);
            }
            else
            {
                MREClient.Send(data, data.Length);
            }
            
        }

        public string DecodeBytes(byte[] data, bool text)
        {   
            //Console.WriteLine("----- Byte Array -----");
            string response = "";
            for (int i = 0; i < data.Length; i++)
            {
                //Console.Write(data[i]);
                if (text)
                    response += (char)data[i];
                else
                    response += data[i].ToString();
            }
            return response;
        }

        public bool isFloat(float number)
        {
            if (number == (int) number)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public byte[][] PreparePackets(byte[] data,int buffer)
        {
            //TODO: NON FUNZIONA CORRETTAMENTE, IL PRIMO BYTE È GIUSTO ED IL RESTO È A 0 -> USARE DEBUGGER
            int packetsNumber = isFloat((float)data.Length/buffer)? (int) data.Length / buffer + 1 :  data.Length / buffer;

            byte[][] packets = new byte[packetsNumber][];
            
            Console.WriteLine(Environment.NewLine +"Packets Needed with " + buffer + " bytes of buffer: " + packetsNumber);
            int pos = 0;
            bool created = false;
            byte[] packet = new byte[0];
            int packetLength;

            for (int i = 0; i < packets.Length; i++)
            {
                for (int j = 0; j < packets.Length; j++)
                {
                    if (!created)
                    {
                        //CREA IL PACCHETTO CON IL BUFFER ADATTO
                        //Creo pacchetto dati da inviare
                        
                        if (data.Length - pos > buffer)
                        {
                            packetLength = _BUFFER + _HEADER_FOOTER_NUMBER;
                            packet = new byte[packetLength];
                        }
                        else
                        {
                            packetLength = data.Length - pos;
                            packet = new byte[packetLength];
                        }

                        created = true;
                    }

                    //RIEMPIE IL PACCHETTO
                    packet[j] = data[pos];
                    pos++;
                }

                packets[i] = packet;
                created = false;
            }

            return packets;
        }

        public void SendAllPackets(byte[][] packets, UdpClient MREClient)
        {
            for (int i = 0; i < packets.Length; i++)
            {
                MREClient.Send(packets[i], packets[i].Length);
            }
        }



        public void checkCommands(string response)
        {
            string[] commandList =
            {
                "joinserver",
                "leaveserver"
            };

            commandsUtility.useCommand(commandsUtility.GetCommand(response));
        }

        public void sentDataToVictim(string victimIp, string victimPort)
        {
            UDPClient("127.0.0.1","80",new byte[1000],true);
        }
    }
}
