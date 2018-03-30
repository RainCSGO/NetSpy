using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSpy.Model;
using NetSpy.Helper;

namespace NetSpy
{
    public partial class Connections : Form
    {
        public ConnectionHelper ConnectionHelper;
        public Random rnd;

        public Connections()
        {
            InitializeComponent();
            ConnectionHelper = new ConnectionHelper();
            rnd = new Random();
        }

        private void Connections_Load(object sender, EventArgs e)
        {
            ConnectionHelper.TryPreparePackets();
        }

        public void SendFakePacket(int length)
        {
            byte[] packet = new byte[length];
            foreach (var cell in packet)
            {
                packet[cell] = (byte) GenerateNumber(1, 100);
            }
        }

        private int GenerateNumber(int min, int max)
        {
            return rnd.Next(min, max);
        }



    }
}
