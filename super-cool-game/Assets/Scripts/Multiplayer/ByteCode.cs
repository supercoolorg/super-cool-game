using System.Collections.Generic;
using System;

public static class ByteCode
{
    public static int GetByteCount(Type type) {
        switch (Type.GetTypeCode(type)) {
            case TypeCode.Byte:
            case TypeCode.Char:
            case TypeCode.Boolean:
                return 1;
            case TypeCode.Int16:
            case TypeCode.UInt16:
                return 2;
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Single:
                return 4;
            case TypeCode.Double:
                return 8;
            default:
                throw new Exception($"[ByteCode]: [GetByteCount]: Unsupported data type '{type}'");
        }
    }

    public static int GetByteCount(List<Type> types) {
        int count = 0;
        for (int i = 0; i < types.Count; i++) {
            count += GetByteCount(types[i]);
        }

        return count;
    }

    public static int GetByteCount(object[] obj) {
        int count = 0;
        for (int i = 0; i < obj.Length; i++) {
            count += GetByteCount(obj[i].GetType());
        }

        return count;
    }
}
