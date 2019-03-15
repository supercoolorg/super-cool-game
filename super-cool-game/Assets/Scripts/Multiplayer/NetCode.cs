namespace SuperCoolNetwork {
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Commands;


    public static class NetCode {
        public const string SERVER_ADDR = "localhost";
        public const int MATCHMAKER_PORT = 50999;
        public static UdpClient socket = new UdpClient();
        public static bool IsConnected { get; private set; }
        public static Queue CmdQueue = Queue.Synchronized(new Queue());
        public static int ClientPort { get; private set; }

        public static void UdpListener() {
            IPEndPoint inConn = new IPEndPoint(IPAddress.Any, ClientPort);
            while (true) {
                    byte[] buffer = socket.Receive(ref inConn);
                lock (CmdQueue.SyncRoot) {
                    CmdQueue.Enqueue(Command.From(buffer));
                }
            }
        }

        public static void Connect(int port) {
            socket.Connect(SERVER_ADDR, port);
            ClientPort = ((IPEndPoint)socket.Client.LocalEndPoint).Port;

            // Start listening
            var udpThread = new Thread(new ThreadStart(UdpListener));
            udpThread.Start();

            var cmd = new Command(OpCode.Register);
            Send(cmd);

            IsConnected = true;
        }

        public static void Send(Command cmd) {
            socket.Send(cmd.Buffer, cmd.Buffer.Length);
        }
    }
}
