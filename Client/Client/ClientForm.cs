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
        delegate void AppendTextDelegate(string s);
        AppendTextDelegate textAppender;
        Socket serverSocket;
        
        public ClientForm() { InitializeComponent(); }
        
        //Form Load시 초기화
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

        //연결하기 버튼 클릭 시 연결 시작
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            int port;
            if (serverSocket == null) serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { port = Int32.Parse(textPort.Text); }
            catch
            {
                MessageBox.Show("포트 번호가 잘못 입력되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPort.Focus();
                textPort.SelectAll();
                return;
            }
            
            if (serverSocket.Connected)
                MessageBox.Show("이미 연결되어 있습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (port < 0 || port > 65535)
            {
                MessageBox.Show("포트 번호가 잘못 입력되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPort.Focus();
                textPort.SelectAll();
            }
            else if (textNickName.Text == "")
                MessageBox.Show("닉네임을 채워 넣어주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try { serverSocket.Connect(textAddress.Text, port); }
                catch (SocketException ex)
                {
                    MessageBox.Show("연결에 실패하였습니다.\n오류 내용 : " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                byte[] byteData = Encoding.UTF8.GetBytes(textNickName.Text + '\x01' + '\x02');
                serverSocket.Send(byteData);
                textAddress.ReadOnly = true; textPort.ReadOnly = true; textNickName.ReadOnly = true;

                AppendText("서버와 연결되었습니다.");
                AsyncObject asyncObject = new AsyncObject(4096, serverSocket);
                serverSocket.BeginReceive(asyncObject.Buffer, 0, asyncObject.BufferSize, 0, ReceiveData, asyncObject);
            }
        }

        //데이터 받기 Callback
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
                if (tokens[1][0] == '\x02') AppendText(tokens[0] + "님이 입장하셨습니다.");
                else if (tokens[1][0] == '\x03') AppendText(tokens[0] + "님이 퇴장하셨습니다.");
                else if (tokens[1][0] == '\x04')
                {
                    try
                    {
                        AppendText("서버 종료로 서버와의 연결이 해제되었습니다.");
                        serverSocket.Close();
                        serverSocket = null;
                        textAddress.ReadOnly = false; textPort.ReadOnly = false; textNickName.ReadOnly = false;
                    }
                    catch { }
                    return;
                }
                else AppendText("[받음] " + tokens[0] + " : " + tokens[1]);
            }
            catch { }

            asyncObject.ClearBuffer();
            try { asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, 4096, 0, ReceiveData, asyncObject); }
            catch { asyncObject.WorkingSocket.Close(); }
        }
        
        //텍스트 보내기
        private void SendText(string message)
        {
            textSend.Clear();
            if (!serverSocket.Connected)
                MessageBox.Show("서버가 실행되고 있지 않습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("텍스트가 입력되지 않았습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textSend.Focus();
            }
            else
            {
                string address = (serverSocket.LocalEndPoint as IPEndPoint).Address.ToString();
                byte[] byteData = Encoding.UTF8.GetBytes(textNickName.Text + '\x01' + message);
                serverSocket.Send(byteData);

                //Thread.Sleep(1000); //비동기 테스트를 위함
                AppendText("[보냄] " + textNickName.Text + " : " + message);
            }
        }

        //보내기 버튼 누를 때 텍스트 보내기
        private void buttonSend_Click(object sender, EventArgs e) { SendText(textSend.Text.Trim()); }

        //Enter 누를 때 텍스트 보내기
        private void textSend_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) SendText(textSend.Text.Trim()); }

        //연결 종료
        private void Disconnect()
        {
            if (serverSocket != null && serverSocket.Connected)
            {
                byte[] byteData = Encoding.UTF8.GetBytes(textNickName.Text + "\x01\x03");
                serverSocket.Send(byteData);

                AppendText("서버와 연결이 해제되었습니다.");
                serverSocket.Close();
                serverSocket = null;
            }
            textAddress.ReadOnly = false; textPort.ReadOnly = false; textNickName.ReadOnly = false;
        }

        //연결끊기 버튼 클릭 시 연결 종료
        private void buttonDisconnect_Click(object sender, EventArgs e) { Disconnect(); }

        //폼 종료 시 연결 종료
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e) { Disconnect(); }

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
