using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class ClientForm : Form
    {
        delegate void AppendTextDelegate(Control c, string s);
        AppendTextDelegate textAppender;
        Socket serverSocket;
        public ClientForm() { InitializeComponent(); }
        private void ClientForm_Load(object sender, EventArgs e)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            textAppender = new AppendTextDelegate(AppendText);

            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress defaultAddress = null;
            foreach (IPAddress addr in hostEntry.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    defaultAddress = addr;
                    break;
                }
            }

            if (defaultAddress == null) defaultAddress = IPAddress.Loopback;
            textAddress.Text = defaultAddress.ToString();
        }

        private void AppendText(Control control, string s)
        {
            if (control.InvokeRequired) control.Invoke(textAppender, control, s);
            else control.Text += "\r\n" + s;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            int port;
            if (serverSocket.Connected)
                MessageBox.Show("이미 연결되어 있습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (!int.TryParse(textPort.Text, out port))
            {
                MessageBox.Show("포트 번호가 잘못 입력되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPort.Focus();
                textPort.SelectAll();
            }
            else
            {
                try { serverSocket.Connect(textAddress.Text, port); }
                catch (SocketException ex)
                {
                    MessageBox.Show("연결에 실패하였습니다.\n오류 내용 : "+ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AppendText(textStatus, "서버와 연결되었습니다.");

                AsyncObject asyncObject = new AsyncObject(4096, serverSocket);
                serverSocket.BeginReceive(asyncObject.Buffer, 0, asyncObject.BufferSize, 0, ReceiveData, asyncObject);
            }
        }

        private void ReceiveData(IAsyncResult asyncResult)
        {
            AsyncObject asyncObject = (AsyncObject)asyncResult.AsyncState;
            int receive = asyncObject.WorkingSocket.EndReceive(asyncResult);
            if (receive <= 0)
            {
                asyncObject.WorkingSocket.Close();
                return;
            }

            string text = Encoding.UTF8.GetString(asyncObject.Buffer);
            string[] tokens = text.Split('\x01');
            AppendText(textStatus, "[받음] " + tokens[0] + " : " + tokens[1]);

            asyncObject.ClearBuffer();
            asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, 4096, 0, ReceiveData, asyncObject);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string send = textSend.Text.Trim();
            if (!serverSocket.IsBound)
                MessageBox.Show("서버가 실행되고 있지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrEmpty(send))
            {
                MessageBox.Show("텍스트가 입력되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textSend.Focus();
            }
            else
            {
                string address = (serverSocket.LocalEndPoint as IPEndPoint).Address.ToString();
                string nickName = (textNickName.Text=="" ? address : textNickName.Text);
                byte[] byteData = Encoding.UTF8.GetBytes(nickName + '\x01' + send);
                serverSocket.Send(byteData);

                AppendText(textStatus, string.Format("[보냄] " + nickName + " : " + send));
                textSend.Clear();
            }
        }

        private void textSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) buttonSend_Click(sender, e);
        }
    }

    public class AsyncObject
    {
        public byte[] Buffer;
        public Socket WorkingSocket;
        public readonly int BufferSize;
        
        public AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }

        public AsyncObject(int buffersize, Socket tempSocket)
            : this(buffersize) { WorkingSocket = tempSocket; }

        public void ClearBuffer() { Array.Clear(Buffer, 0, BufferSize); }
    }
}
