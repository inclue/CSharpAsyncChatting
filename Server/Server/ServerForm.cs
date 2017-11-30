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


namespace Server
{
    public partial class ServerForm : Form
    {
        delegate void AppendTextDelegate(Control c, string s);
        AppendTextDelegate textAppender;
        Socket serverSocket;
        IPAddress thisAddress;
        List<Socket> connectClientList;

        public ServerForm() { InitializeComponent(); }
        
        private void ServerForm_Load(object sender, EventArgs e)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            textAppender = new AppendTextDelegate(AppendText);
            connectClientList = new List<Socket>();

            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress addr in hostEntry.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    thisAddress = addr;
                    break;
                }
            }

            if (thisAddress == null) thisAddress = IPAddress.Loopback;
            textAddress.Text = thisAddress.ToString();
        }

        void AppendText(Control control, string s) {
            if (control.InvokeRequired) control.Invoke(textAppender, control, s);
            else control.Text += "\r\n" + s;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(textPort.Text, out port))
            {
                MessageBox.Show("포트 번호가 잘못 입력되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPort.Focus();
                textPort.SelectAll();
            }
            else
            {
                IPEndPoint endPoint = new IPEndPoint(thisAddress, port);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10);

                serverSocket.BeginAccept(AcceptCallback, null);
                AppendText(textStatus, "서버 시작이 완료되었습니다.");
            }
        }

        void AcceptCallback(IAsyncResult asyncResult) {
            Socket client = serverSocket.EndAccept(asyncResult);
            serverSocket.BeginAccept(AcceptCallback, null);

            AsyncObject asyncObject = new AsyncObject(4096);
            asyncObject.WorkingSocket = client;
            connectClientList.Add(client);

            AppendText(textStatus, string.Format("클라이언트 (@ {0})가 연결되었습니다.", client.RemoteEndPoint));
            client.BeginReceive(asyncObject.Buffer, 0, 4096, 0, ReceiveData, asyncObject);
        }

        void ReceiveData(IAsyncResult asyncResult)
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

            for (int i = connectClientList.Count - 1; i >= 0; i--)
            {
                Socket socket = connectClientList[i];
                if (socket != asyncObject.WorkingSocket)
                {
                    try { socket.Send(asyncObject.Buffer); }
                    catch
                    {
                        socket.Dispose();
                        connectClientList.RemoveAt(i);
                    }
                }
            }

            asyncObject.ClearBuffer();
            asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, 4096, 0, ReceiveData, asyncObject);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string send = textSend.Text.Trim();
            if (!serverSocket.IsBound) MessageBox.Show("서버가 실행되고 있지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrEmpty(send))
            {
                MessageBox.Show("텍스트가 입력되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textSend.Focus();
            }
            else
            {
                byte[] byteData = Encoding.UTF8.GetBytes("관리자\x01" + send);
                for (int i = connectClientList.Count - 1; i >= 0; i--)
                {
                    Socket tempSocket = connectClientList[i];
                    try { tempSocket.Send(byteData); }
                    catch
                    {
                        tempSocket.Dispose();
                        connectClientList.RemoveAt(i);
                    }
                }

                AppendText(textStatus, string.Format("[보냄] 관리자 : " + send));
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
