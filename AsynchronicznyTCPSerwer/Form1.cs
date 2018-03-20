using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsynchronicznyTCPSerwer
{
    public partial class Form1 : Form
    {
        //pola prywatne
        private TcpListener serwer;
        private TcpClient klient;
        public Form1()
        {
            InitializeComponent();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            lbLogs.Items.Add("Oczekiwanie na połączenie ...");
            IPAddress adresIP;
            try
            {
                adresIP = IPAddress.Parse(tbHostAddress.Text);
            }
            catch
            {
                MessageBox.Show("Błędny format adresu IP!", "Błąd");
                lbLogs.Text = String.Empty;
                return;
            }
            int port = System.Convert.ToInt16(numPort.Value);
            try
            {
                serwer = new TcpListener(adresIP, port);
                serwer.Start();
                serwer.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), serwer);
            }
            catch (Exception ex)
            {
                lbLogs.Items.Add("Błąd: " + ex.Message);
            }
        }
        private void AcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            TcpListener s = (TcpListener)asyncResult.AsyncState;
            klient = s.EndAcceptTcpClient(asyncResult);
            SetListBoxText("Połączenie się powiodło!");
            klient.Close();
            serwer.Stop();
        }
        private delegate void SetTextCallBack(string tekst);
        private void SetListBoxText(string tekst)
        {
            if (lbLogs.InvokeRequired)
            {
                SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                this.Invoke(f, new object[] { tekst });
            }
            else
            {
                lbLogs.Items.Add(tekst);
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            if (serwer != null) serwer.Stop();
        }
    }
}
