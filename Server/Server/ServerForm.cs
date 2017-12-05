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
        delegate void AppendTextDelegate(string s);
        AppendTextDelegate textAppender;
        Socket serverSocket;
        IPAddress thisAddress;
        List<Socket> connectClientList;

        public ServerForm() { InitializeComponent(); }

        //Form Load시 초기화
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
            dataGridView.ReadOnly = true;
        }

        //시작 버튼 클릭 시 연결 시작
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            int port;
            if (serverSocket == null)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connectClientList = new List<Socket>();
            }
            try { port = Int32.Parse(textPort.Text); }
            catch
            {
                MessageBox.Show("포트 번호가 잘못 입력되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPort.Focus();
                textPort.SelectAll();
                return;
            }

            if (serverSocket.IsBound)
                MessageBox.Show("서버가 실행 중입니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (port < 0 || port > 65535)
            {
                MessageBox.Show("포트 번호가 잘못 입력되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPort.Focus();
                textPort.SelectAll();
            }
            else
            {
                IPEndPoint endPoint = new IPEndPoint(thisAddress, port);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(20);

                serverSocket.BeginAccept(AcceptCallback, null);
                AppendText("서버 시작이 완료되었습니다.");
            }
        }

        //Client에서 연결 신호가 들어오면 시작되는 Callback
        private void AcceptCallback(IAsyncResult asyncResult) {
            try
            {
                Socket client = serverSocket.EndAccept(asyncResult);

                serverSocket.BeginAccept(AcceptCallback, null);

                AsyncObject asyncObject = new AsyncObject(4096);
                asyncObject.WorkingSocket = client;
                connectClientList.Add(client);

                AppendText("IP : " + client.RemoteEndPoint);
                client.BeginReceive(asyncObject.Buffer, 0, 4096, 0, ReceiveData, asyncObject);
            }
            catch { }
        }

        //Data를 받았을 때 시작되는 Callback
        private void ReceiveData(IAsyncResult asyncResult)
        {
            AsyncObject asyncObject = asyncResult.AsyncState as AsyncObject;
            try { asyncObject.WorkingSocket.EndReceive(asyncResult); }
            catch
            {
                asyncObject.WorkingSocket.Close();
                return;
            }

            string text = Encoding.UTF8.GetString(asyncObject.Buffer);
            string[] tokens = text.Split('\x01');
            try
            {
                if (tokens[1][0] == '\x02')
                {
                    AppendText(tokens[0] + "님이 입장하셨습니다. (현재 인원 : " + connectClientList.Count + "명)");
                    try { dataGridView.Rows.Add(new string[] { tokens[0] }); }
                    catch { }
                }
                else if (tokens[1][0] == '\x03')
                {
                    AppendText(tokens[0] + "님이 퇴장하셨습니다. (현재 인원 : " + (connectClientList.Count - 1) + "명)");
                    try
                    {
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            if (tokens[0] == dataGridView.Rows[i].Cells[0].Value as string)
                            {
                                dataGridView.Rows.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    catch { }
                }
                else AppendText("[받음] " + tokens[0] + " : " + tokens[1]);
            }
            catch { }
            for (int i = connectClientList.Count - 1; i >= 0; i--)
            {
                Socket tempSocket = connectClientList[i];
                if (tempSocket != asyncObject.WorkingSocket)
                {
                    try { tempSocket.Send(asyncObject.Buffer); }
                    catch
                    {
                        tempSocket.Close();
                        connectClientList.RemoveAt(i);
                    }
                }
            }

            asyncObject.ClearBuffer();
            try { asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, 4096, 0, ReceiveData, asyncObject); }
            catch
            {
                asyncObject.WorkingSocket.Close();
                connectClientList.Remove(asyncObject.WorkingSocket);
            }
        }

        //텍스트 보내기
        private void SendText(string message)
        {
            if (!serverSocket.IsBound) MessageBox.Show("서버가 실행되고 있지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("텍스트가 입력되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textSend.Focus();
            }
            else
            {
                SendProcess(Encoding.UTF8.GetBytes("관리자\x01" + message));
                AppendText("[보냄] 관리자 : " + message);
                textSend.Clear();
            }
        }

        //각 Client들에게 텍스트 보내기
        private void SendProcess(byte[] byteData)
        {
            for (int i = connectClientList.Count - 1; i >= 0; i--)
            {
                Socket tempSocket = connectClientList[i];
                try { tempSocket.Send(byteData); }
                catch
                {
                    tempSocket.Close();
                    connectClientList.RemoveAt(i);
                }
            }
        }

        //보내기 버튼 누를 때 텍스트 보내기
        private void buttonSend_Click(object sender, EventArgs e) { SendText(textSend.Text.Trim()); }

        //Enter 누를 때 텍스트 보내기
        private void textSend_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) SendText(textSend.Text.Trim()); }

        //연결 종료
        private void Disconnect()
        {
            if (serverSocket != null && serverSocket.IsBound)
            {
                SendProcess(Encoding.UTF8.GetBytes("관리자\x01\x04"));
                serverSocket.Close();
                serverSocket = null;

                AppendText("서버 종료가 완료되었습니다.");
                while (dataGridView.Rows.Count > 0) dataGridView.Rows.RemoveAt(0);
            }
        }

        //연결끊기 버튼 클릭 시 연결 종료
        private void buttonDisconnect_Click(object sender, EventArgs e) { Disconnect(); }

        //폼 종료 시 연결 종료
        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e) { Disconnect(); }

        //메시지, 상태 등의 내역 쓰기
        private void AppendText(string message)
        {
            if (textStatus.InvokeRequired) textStatus.Invoke(textAppender, message);
            else textStatus.Text += "\r\n" + message;
        }
    }

    //Callback에 대한 내용 저장을 위한 Class
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