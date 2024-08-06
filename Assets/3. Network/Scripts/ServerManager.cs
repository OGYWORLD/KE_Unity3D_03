using MyProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject{
    public class ServerManager : MonoBehaviour
    {
        public static ServerManager Instance { get; private set; }

        public Button connectButton;
        public Text messagePrefab;
        public RectTransform textArea;

        public string ipAddress = "127.0.0.1"; // IPv6와의 호환성을 위해 string을 주로 사용

        // 0~65, 535 => usinged short 사이즈의 숫자만 취급할 수 있으나
        // port 주소는 2바이트의 부호 없는 정수를 사용
        // C#에서는 int 사용
        //public List<int> port = new List<int>();
        public int port = 9999;

        private int serverNum = 2;
        public int serverIdx = 0;

        // 20240805HW
        //private bool[] isConnected = { false, false };
        //private Thread[] serverThreads;

        private bool isConnected = false;
        private Thread serverMainThreads;

        private List<ClientHandler> clients = new List<ClientHandler>();
        private List<Thread> threads = new List<Thread>();

        // 모든 스레드가 접근할 수 있는 Data 영역의 Queue
        public static Queue<string> log = new Queue<string>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            connectButton.onClick.AddListener(ServerConnectButtonClick);
        }

        /*
        private void Start()
        {
            // 20240805HW
            serverThreads = new Thread[serverNum];
            port.Add(9999);
            port.Add(9998);
            port.Add(9997);
            port.Add(9996);
            port.Add(9995);
            port.Add(9994);
            port.Add(9993);
            port.Add(9992);
            port.Add(9991);
            port.Add(9990);
        }
        */

        public void ServerConnectButtonClick()
        {
            /*
            for(int i = 0; i < serverNum; i++)
            {
                if (isConnected[i] == false)
                {
                    // 여기서는 서버를 연다.
                    print(port[i]);
                    serverThreads[i] = new Thread(() => ServerThread(port[i]));
                    serverThreads[i].IsBackground = true;
                    serverThreads[i].Start();
                    isConnected[i] = true;
                    serverIdx++;
                }
                else
                {
                    // 여기서는 서버를 닫는다.
                    serverThreads[i].Abort();
                    isConnected[i] = false;
                }
            }
            */
            if (isConnected == false)
            {
                // 여기서는 서버를 연다.
                serverMainThreads = new Thread(ServerThread);
                serverMainThreads.IsBackground = true;
                serverMainThreads.Start();
                isConnected = true;
                serverIdx++;
            }
            else
            {
                // 여기서는 서버를 닫는다.
                serverMainThreads.Abort();
                isConnected = false;
            }
        }

        // 데이터 입출력 등 데이터의 전송을 책임지는 Input, Output 스트림이 필요함
        private StreamReader reader;
        private StreamWriter writer;
        private int clientId = 0;

        //private void ServerThread(int port)
        private void ServerThread()
        {
            // try / catch 문의 용도 : 예외사항 발생 시, 메시지를 수동으로 활용할 수 있도록
            // 잘 제어된 if-else문과 비슷하다.
            try
            {
                //StreamReader reader;
                //StreamWriter writer;

                // 오늘 과제
                // 서버 스레드를 List로 관리하여
                // 다중 연결이 가능한 서버로 만들어보세요.

                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start(); // tcp 서버를 가동시킨다.

                //Text logText = Instantiate(messagePrefab, textArea);
                //logText.text = "서버 시작";

                log.Enqueue("서버 시작");

                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient(); // 대기가 걸린다.

                    ClientHandler handler = new ClientHandler();

                    handler.Connect(clientId++, this, client);

                    clients.Add(handler);

                    Thread clientThread = new Thread(handler.Run);

                    clientThread.IsBackground = true;
                    clientThread.Start();

                    threads.Add(clientThread);

                    log.Enqueue($"{handler.id}번 클라이언트가 접속되었습니다.");
                }
                /*
                TcpClient tcpClient = tcpListener.AcceptTcpClient(); // Tcp로 부터 리턴이 올 때까지 대기가 걸린다.
                                                                     // 따라서 멀티 스레드로 생성이 되어야 한다.
                                                                     // 근데 멀티 스레드는 함부로 쓰면 안 된다.

                //Text logText2 = Instantiate(messagePrefab, textArea);
                //logText2.text = "클라이언트 연결됨";

                log.Enqueue("클라이언트 연결됨");

                reader = new StreamReader(tcpClient.GetStream());
                writer = new StreamWriter(tcpClient.GetStream());

                // AutoFlush가 true면 자동으로 write.WriteLine 쓸때마다 Flush된다.
                writer.AutoFlush = true;

                while (tcpClient.Connected) // 클라이언트와 연결되었을 때
                {
                    string readString = reader.ReadLine();

                    if (string.IsNullOrEmpty(readString)) // 메시지가 공백이라면
                    {
                        continue;
                    }

                    //Text messageText = Instantiate(messagePrefab, textArea);
                    // messageText.text = readString;

                    //받은 메시지를 그대로 writer에 쓴다.
                    writer.WriteLine($"당신의 메시지 : {readString}");

                    log.Enqueue($"client message : {readString}");
                }

                log.Enqueue("클라이언트 연결 종료");
                */
            }
            catch (ArgumentException e) // try 문 내의 구문 중에 에러가 발생할 시 호출
            {
                log.Enqueue("ArgumentException 발생");
                log.Enqueue(e.Message);
            }
            catch (NullReferenceException e)
            {
                log.Enqueue("NullReferenceException 발생");
                log.Enqueue(e.Message);
            }
            catch (Exception e)
            {
                log.Enqueue("Exception 발생");
                log.Enqueue(e.Message);
            }
            finally // try문 내에서 에러가 발생 해도 실행 되고, 안 발생해도 실행 된다.
            { // 주로 중간에 흐름이 끊기지 않고 생성된 객체를 해제하는 등의 반드시 필요한 절차를 여기서 수행한다.

                foreach(var thread in threads)
                {
                    thread?.Abort();
                }
            }
        }

        public void Disconnect(ClientHandler client)
        {
            clients.Remove(client);
        }

        public void BroadcastToClients(string message)
        {
            log.Enqueue(message);

            foreach(ClientHandler client in clients)
            {
                client.MessageToClient(message);
            }
        }

        private void Update()
        {
            if(log.Count > 0)
            {
                Text logText = Instantiate(messagePrefab, textArea);
                logText.text = log.Dequeue();
            }
        }
    }
}

// 클라이언트가 TCP 접속 요청을 할 때마다 해당 클라이언트를 붙들고 있는 객체를 생성한다.
public class ClientHandler
{
    public int id;
    public ServerManager server;
    public TcpClient TcpClient;
    public StreamReader reader;
    public StreamWriter writer;

    public void Connect(int id, ServerManager server, TcpClient tcpClient)
    {
        this.id = id;
        this.server = server;
        this.TcpClient = tcpClient;

        reader = new StreamReader(tcpClient.GetStream());
        writer = new StreamWriter(tcpClient.GetStream());
        writer.AutoFlush = true;
    }

    public void Disconnect()
    {
        // 허거덩 버퍼, 서버 이런 애들은
        // GC가 자동으로 수거를 안 해서 직접 Close해서 메모리 수거 해줘야한다.
        writer.Close();
        reader.Close();

        TcpClient.Close();

        server.Disconnect(this);
    }

    public void MessageToClient(string message)
    {
        writer.WriteLine(message);
    }

    public void Run()
    {
        try
        {
            while(TcpClient.Connected)
            {
                string readString = reader.ReadLine();

                // string은 class, 참조타입이니 null 체크 해주는 거 잊지말자
                if(string.IsNullOrEmpty(readString))
                {
                    continue;
                }

                // 읽어온 메시지가 있으면 서버에게 전달
                server.BroadcastToClients($"{id} 님의 말 : {readString}");
            }
        }
        catch(Exception e)
        {
            ServerManager.log.Enqueue($"{id}번 클라이언트 오류 발생 : {e.Message}");
        }
        finally
        {
            Disconnect();
        }
    }
}