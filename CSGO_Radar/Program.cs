using System;
using System.Diagnostics;
using System.Threading;

namespace CSGO_Radar
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = Process.GetProcessesByName("csgo")[0];

            var processHandle = Win32API.OpenProcess(Win32API.PROCESS_VM_READ | Win32API.PROCESS_VM_WRITE, false, process.Id);

            int clientAddress = 0;

            foreach (ProcessModule module in process.Modules)
            {
                if (module.ModuleName == "client.dll")
                {
                    clientAddress = (int)module.BaseAddress;
                }
            }

            while(true)
            {
                for (int i = 0; i <= 64; i++)
                {
                    var playerAddress = Memory.ReadMemory<int>((int)processHandle, clientAddress + Signatures.dwEntityList + (i * 0x10));
                    Memory.WriteMemory<int>((int)processHandle, playerAddress + NetVars.m_bSpotted, 1);
                }

                Thread.Sleep(10);
            }
        }
    }
}
