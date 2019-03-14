using System;
using System.Collections.Generic;

public enum OpCode {
    Error,
    Queue,
    FoundMatch,
    Register,
    Spawn,
    Move,
    Jump,
    SetPos,
    Disconnect,
    Ping
}

namespace Commands {

    public static class Utils {
        public static Dictionary<OpCode, List<Type>> MODELS = new Dictionary<OpCode, List<Type>>{
            { OpCode.Error, new List<Type>() },
            { OpCode.Queue, new List<Type>() },
            { OpCode.FoundMatch, new List<Type>() { typeof(ushort) } },
            { OpCode.Register, new List<Type>() },
            { OpCode.Spawn, new List<Type>() { typeof(ushort), typeof(float), typeof(float) } },
            { OpCode.Move, new List<Type>() { typeof(float) } },
            { OpCode.Jump, new List<Type>() { typeof(float) } },
            { OpCode.SetPos, new List<Type>() { typeof(ushort), typeof(float), typeof(float), typeof(float), typeof(float) } },
            { OpCode.Disconnect, new List<Type>() { typeof(ushort) } },
            { OpCode.Ping, new List<Type>() }
        };
    }

    public class Command {
        public byte[] Buffer { get; private set; }
        private readonly List<Type> model;
        private readonly int modelByteCount;

        public Command(OpCode op, params object[] args) {
            model = Utils.MODELS[op];
            modelByteCount = ByteCode.GetByteCount(model);
            Buffer = new byte[1 + modelByteCount];
            Buffer[0] = (byte)op;

            for (int i = 0; i < args.Length; i++) {
                SetAt(i, args[i]);
            }
        }

        public OpCode GetOpCode() {
            return (OpCode)Buffer[0];
        }

        public void SetAt(int index, object obj) {
            Type type = obj.GetType();
            int offset = GetByteOffset(index);
            byte[] bytes = (byte[])typeof(BitConverter)
                    .GetMethod("GetBytes", new Type[] { type })
                    .Invoke(null, new object[] { obj });
            System.Buffer.BlockCopy(bytes, 0, Buffer, offset, ByteCode.GetByteCount(type));
        }

        public T GetAt<T>(int index) {
            T obj = (T)typeof(BitConverter)
                    .GetMethod("To" + typeof(T).Name)
                    .Invoke(null, new object[] { Buffer, GetByteOffset(index) });
            return obj;
        }

        private int GetByteOffset(int index) {
            int n = index / modelByteCount;

            int offset = n * modelByteCount;
            for (int i = 0; i < index % modelByteCount; i++) {
                offset += ByteCode.GetByteCount(model[i]);
            }
            offset += 1;

            return offset;
        }
    }
}