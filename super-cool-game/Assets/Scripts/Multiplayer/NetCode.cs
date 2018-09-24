namespace NetCode {
    using System;

    public enum OpCode {
        Error,
        Queue,
        FoundMatch,
        Spawn,
        Move
    }

    public static class NetHelpers {
        public const string SERVER_ADDR = "localhost";
        public const int MATCHMAKER_PORT = 50999;
        public const int MESSAGE_SIZE = 4;

        public static UInt16 ReadUInt16(byte[] buffer, int idx) {
            return (UInt16)((buffer[idx] << 8) | buffer[idx+1]);
        }

        public static void WriteUInt16(UInt16 n, ref byte[] buffer, int idx) {
            if (idx + sizeof(UInt16) > buffer.Length)
                throw new IndexOutOfRangeException("Buffer cannot contain 2 bytes from index " + idx);
            buffer[idx] = (byte) (n >> 8);
            buffer[idx + 1] = (byte) (n & 0x00FF);
        }

        public static byte[] BufferOp(OpCode op){
            byte[] buffer = new byte[MESSAGE_SIZE]; // By default filled with zeroes
            buffer[0] = (byte) op;
            return buffer;
        }
    }
}
