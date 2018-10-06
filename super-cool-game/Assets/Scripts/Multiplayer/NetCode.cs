namespace SuperCoolNetwork {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public enum OpCode {
        Error,
        Queue,
        FoundMatch,
        Register,
        Spawn,
        Move,
        Jump,
        SetPos
    }

    public static class NetCode {
        public const string SERVER_ADDR = "localhost";
        public const int MATCHMAKER_PORT = 50999;
        public static UdpClient socket = new UdpClient();
        public static bool IsConnected { get; private set; }
        public static Queue CmdQueue = Queue.Synchronized(new Queue());
        public static int ClientPort { get; private set; }

        public static byte[] BufferOp(OpCode op, int size){
            byte[] buffer = new byte[size]; // By default filled with zeroes
            buffer[0] = (byte) op;
            return buffer;
        }

        public static void UdpListener() {
            IPEndPoint inConn = new IPEndPoint(IPAddress.Any, ClientPort);
            while (true) {
                byte[] buffer = socket.Receive(ref inConn);
                lock (CmdQueue.SyncRoot) {
                    CmdQueue.Enqueue(buffer);
                }
            }
        }

        public static void Connect(int port) {
            socket.Connect(SERVER_ADDR, port);
            ClientPort = ((IPEndPoint)socket.Client.LocalEndPoint).Port;

            // Start listening
            var udpThread = new Thread(new ThreadStart(UdpListener));
            udpThread.Start();

            byte[] buffer = BufferOp(OpCode.Register, 4);
            socket.Send(buffer, buffer.Length);

            IsConnected = true;
        }
    }
}
