using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SimpleMemoryReader
{
    public class MemoryReader
    {
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
          [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);



        public int ReadPointers(IntPtr handle, int baseAddress, params int[] offsets)
        {
            IntPtr bytesRead;
            int currentAddress = baseAddress;
           
            byte[] buffer = new byte[4];
            foreach (int offset in offsets)
            {
                ReadProcessMemory(handle, (IntPtr)currentAddress, buffer, 4, out bytesRead);
                currentAddress = BitConverter.ToInt32(buffer, 0) + offset;
            }
            return currentAddress;
        }

        public short ReadInt16(IntPtr handle, int address)
        {
            IntPtr bytesRead;
            byte[] buffer = new byte[4];
            ReadProcessMemory(handle, (IntPtr)address, buffer, 4, out bytesRead);
            return BitConverter.ToInt16(buffer, 0);
        }

    }
}
