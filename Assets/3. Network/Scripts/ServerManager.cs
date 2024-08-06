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

        public string ipAddress = "127.0.0.1"; // IPv6���� ȣȯ���� ���� string�� �ַ� ���

        // 0~65, 535 => usinged short �������� ���ڸ� ����� �� ������
        // port �ּҴ� 2����Ʈ�� ��ȣ ���� ������ ���
        // C#������ int ���
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

        // ��� �����尡 ������ �� �ִ� Data ������ Queue
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
                    // ���⼭�� ������ ����.
                    print(port[i]);
                    serverThreads[i] = new Thread(() => ServerThread(port[i]));
                    serverThreads[i].IsBackground = true;
                    serverThreads[i].Start();
                    isConnected[i] = true;
                    serverIdx++;
                }
                else
                {
                    // ���⼭�� ������ �ݴ´�.
                    serverThreads[i].Abort();
                    isConnected[i] = false;
                }
            }
            */
            if (isConnected == false)
            {
                // ���⼭�� ������ ����.
                serverMainThreads = new Thread(ServerThread);
                serverMainThreads.IsBackground = true;
                serverMainThreads.Start();
                isConnected = true;
                serverIdx++;
            }
            else
            {
                // ���⼭�� ������ �ݴ´�.
                serverMainThreads.Abort();
                isConnected = false;
            }
        }

        // ������ ����� �� �������� ������ å������ Input, Output ��Ʈ���� �ʿ���
        private StreamReader reader;
        private StreamWriter writer;
        private int clientId = 0;

        //private void ServerThread(int port)
        private void ServerThread()
        {
            // try / catch ���� �뵵 : ���ܻ��� �߻� ��, �޽����� �������� Ȱ���� �� �ֵ���
            // �� ����� if-else���� ����ϴ�.
            try
            {
                //StreamReader reader;
                //StreamWriter writer;

                // ���� ����
                // ���� �����带 List�� �����Ͽ�
                // ���� ������ ������ ������ ��������.

                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start(); // tcp ������ ������Ų��.

                //Text logText = Instantiate(messagePrefab, textArea);
                //logText.text = "���� ����";

                log.Enqueue("���� ����");

                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient(); // ��Ⱑ �ɸ���.

                    ClientHandler handler = new ClientHandler();

                    handler.Connect(clientId++, this, client);

                    clients.Add(handler);

                    Thread clientThread = new Thread(handler.Run);

                    clientThread.IsBackground = true;
                    clientThread.Start();

                    threads.Add(clientThread);

                    log.Enqueue($"{handler.id}�� Ŭ���̾�Ʈ�� ���ӵǾ����ϴ�.");
                }
                /*
                TcpClient tcpClient = tcpListener.AcceptTcpClient(); // Tcp�� ���� ������ �� ������ ��Ⱑ �ɸ���.
                                                                     // ���� ��Ƽ ������� ������ �Ǿ�� �Ѵ�.
                                                                     // �ٵ� ��Ƽ ������� �Ժη� ���� �� �ȴ�.

                //Text logText2 = Instantiate(messagePrefab, textArea);
                //logText2.text = "Ŭ���̾�Ʈ �����";

                log.Enqueue("Ŭ���̾�Ʈ �����");

                reader = new StreamReader(tcpClient.GetStream());
                writer = new StreamWriter(tcpClient.GetStream());

                // AutoFlush�� true�� �ڵ����� write.WriteLine �������� Flush�ȴ�.
                writer.AutoFlush = true;

                while (tcpClient.Connected) // Ŭ���̾�Ʈ�� ����Ǿ��� ��
                {
                    string readString = reader.ReadLine();

                    if (string.IsNullOrEmpty(readString)) // �޽����� �����̶��
                    {
                        continue;
                    }

                    //Text messageText = Instantiate(messagePrefab, textArea);
                    // messageText.text = readString;

                    //���� �޽����� �״�� writer�� ����.
                    writer.WriteLine($"����� �޽��� : {readString}");

                    log.Enqueue($"client message : {readString}");
                }

                log.Enqueue("Ŭ���̾�Ʈ ���� ����");
                */
            }
            catch (ArgumentException e) // try �� ���� ���� �߿� ������ �߻��� �� ȣ��
            {
                log.Enqueue("ArgumentException �߻�");
                log.Enqueue(e.Message);
            }
            catch (NullReferenceException e)
            {
                log.Enqueue("NullReferenceException �߻�");
                log.Enqueue(e.Message);
            }
            catch (Exception e)
            {
                log.Enqueue("Exception �߻�");
                log.Enqueue(e.Message);
            }
            finally // try�� ������ ������ �߻� �ص� ���� �ǰ�, �� �߻��ص� ���� �ȴ�.
            { // �ַ� �߰��� �帧�� ������ �ʰ� ������ ��ü�� �����ϴ� ���� �ݵ�� �ʿ��� ������ ���⼭ �����Ѵ�.

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

// Ŭ���̾�Ʈ�� TCP ���� ��û�� �� ������ �ش� Ŭ���̾�Ʈ�� �ٵ�� �ִ� ��ü�� �����Ѵ�.
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
        // ��ŵ� ����, ���� �̷� �ֵ���
        // GC�� �ڵ����� ���Ÿ� �� �ؼ� ���� Close�ؼ� �޸� ���� ������Ѵ�.
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

                // string�� class, ����Ÿ���̴� null üũ ���ִ� �� ��������
                if(string.IsNullOrEmpty(readString))
                {
                    continue;
                }

                // �о�� �޽����� ������ �������� ����
                server.BroadcastToClients($"{id} ���� �� : {readString}");
            }
        }
        catch(Exception e)
        {
            ServerManager.log.Enqueue($"{id}�� Ŭ���̾�Ʈ ���� �߻� : {e.Message}");
        }
        finally
        {
            Disconnect();
        }
    }
}