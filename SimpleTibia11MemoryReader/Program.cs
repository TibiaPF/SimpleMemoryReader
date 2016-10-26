using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMemoryReader
{
    class Program
    {
        static void Main(string[] args)
        {
            //make sure to be logged in otherwise this example will fail.

            //get the tibia process
            Process tibia = Process.GetProcessesByName("client").First();

            //ty blackd 
            //adrMyHP="Qt5Core.dll" + 004555C8 > 8 > 1D8 > 60 > 8

            //get the correct module
            string dllName = "Qt5Core.dll";
            ProcessModule correctModule = null;
            for(int i =0;i < tibia.Modules.Count;i++)
            {
                if (tibia.Modules[i].ModuleName == dllName)
                {
                    correctModule = tibia.Modules[i];
                    break;
                }
            }

            IntPtr moduleHandle = new IntPtr(0);
            if (correctModule.BaseAddress != null)
                moduleHandle = correctModule.BaseAddress;

            var baseAddress = moduleHandle + 0x4555c8;       
            
            var offsets = new int[] { 0x8, 0x1d8, 0x60, 0x8 };

            MemoryReader reader = new MemoryReader();

            int lifeAddress = reader.ReadPointers(tibia.Handle, (int)baseAddress, offsets);

            int lifeValue = reader.ReadInt16(tibia.Handle, lifeAddress);

            Console.WriteLine($"Address gotten from reader = {lifeAddress.ToString("X")} .");
            Console.WriteLine($"Value in the Address = {lifeValue}.");
            Console.ReadLine();
        }
    }
}
