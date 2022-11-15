using System;

namespace Pearl
{
    public enum Endianness { bigEndian, LittleEndiam }

    public static class ConvertExtend
    {
        public static float ToFloat(bool value)
        {
            return value ? 1 : 0;
        }

        public static bool ToBoolean(float value)
        {
            return value > 0.0f ? true : false;
        }

        public static short ToInt16(byte[] bytes, Endianness endianness)
        {
            if (endianness == Endianness.bigEndian)
            {
                byte aux = bytes[1];
                bytes[1] = bytes[0];
                bytes[0] = aux;
            }

            return BitConverter.ToInt16(bytes, 0);
        }

        public static int ToInt8(byte rawValue)
        {
            // If a positive value, return it
            if ((rawValue & 0x80) == 0)
            {
                return rawValue;
            }

            // Otherwise perform the 2's complement math on the value
            return (byte)(~(rawValue - 0x01)) * -1;
        }

        public static int ToInt8(byte[] rawValue)
        {
            // If a positive value, return it
            if ((rawValue[0] & 0x80) == 0)
            {
                return rawValue[0];
            }

            // Otherwise perform the 2's complement math on the value
            return (byte)(~(rawValue[0] - 0x01)) * -1;
        }

        public static bool TryConvertTo<T>(object input, out T result)
        {
            result = default;
            bool success = true;

            try
            {
                result = (T)input;
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
