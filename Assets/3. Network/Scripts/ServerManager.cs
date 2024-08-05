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
        public List<int> port = new List<int>();

        private int serverNum = 2;
        public int serverIdx = 0;

        // 20240805HW
        private bool[] isConnected = { false, false };
        private Thread[] serverThreads;

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

        public void ServerConnectButtonClick()
        {
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
        }

        // ������ ����� �� �������� ������ å������ Input, Output ��Ʈ���� �ʿ���
        //private StreamReader reader;
        //private StreamWriter writer;

        private void ServerThread(int port)
        {
            StreamReader reader;
            StreamWriter writer;

        // ���� ����
        // ���� �����带 List�� �����Ͽ�
        // ���� ������ ������ ������ ��������.

        TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start(); // tcp ������ ������Ų��.

            //Text logText = Instantiate(messagePrefab, textArea);
            //logText.text = "���� ����";

            log.Enqueue("���� ����");

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

            while(tcpClient.Connected) // Ŭ���̾�Ʈ�� ����Ǿ��� ��
            {
                string readString = reader.ReadLine();

                if(string.IsNullOrEmpty(readString)) // �޽����� �����̶��
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
