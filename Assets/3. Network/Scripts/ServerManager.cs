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
        public List<int> port = new List<int>();

        private int serverNum = 2;
        public int serverIdx = 0;

        // 20240805HW
        private bool[] isConnected = { false, false };
        private Thread[] serverThreads;

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
        }

        // 데이터 입출력 등 데이터의 전송을 책임지는 Input, Output 스트림이 필요함
        //private StreamReader reader;
        //private StreamWriter writer;

        private void ServerThread(int port)
        {
            StreamReader reader;
            StreamWriter writer;

        // 오늘 과제
        // 서버 스레드를 List로 관리하여
        // 다중 연결이 가능한 서버로 만들어보세요.

        TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start(); // tcp 서버를 가동시킨다.

            //Text logText = Instantiate(messagePrefab, textArea);
            //logText.text = "서버 시작";

            log.Enqueue("서버 시작");

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

            while(tcpClient.Connected) // 클라이언트와 연결되었을 때
            {
                string readString = reader.ReadLine();

                if(string.IsNullOrEmpty(readString)) // 메시지가 공백이라면
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
