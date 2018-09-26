namespace SuperCoolNetwork {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    public enum OpCode {
        Error,
        Queue,
        FoundMatch,
        Register,
        Spawn,
        Move,
        SendPos
    }

    public static class NetCode {
        public const string SERVER_ADDR = "localhost";
        public const int MATCHMAKER_PORT = 50999;
        public static UdpClient socket = new UdpClient();
        public static Queue CmdQueue = Queue.Synchronized(new Queue());
        public static int clientPort;

        public static byte[] BufferOp(OpCode op, int size){
            byte[] buffer = new byte[size]; // By default filled with zeroes
            buffer[0] = (byte) op;
            return buffer;
        }

        public static void UdpListener() {
            IPEndPoint inConn = new IPEndPoint(IPAddress.Any, clientPort);
            while (true) {
                byte[] buffer = socket.Receive(ref inConn);
                CmdQueue.Enqueue(buffer);
            }
        }
    }
}
