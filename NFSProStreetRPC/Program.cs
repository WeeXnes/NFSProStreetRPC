using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Memory.Win32;
using Memory.Utils;
using System.Threading;
using DiscordRPC;

namespace NFSProStreetRPC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Need for Speed: ProStreet DiscordRPC";


            Process p = Process.GetProcessesByName("nfs").FirstOrDefault();
            if (p == null) return;
            MemoryHelper32 helper = new MemoryHelper32(p);
            uint baseAddr1 = helper.GetBaseAddress(0x6B0EA8);
            int[] offset1 = { 0x1E0 };
            uint targetAddr1 = MemoryUtils.OffsetCalculator(helper, baseAddr1, offset1);

            uint baseAddr2 = helper.GetBaseAddress(0x6B9FB8);
            int[] offset2 = { 0x3C, 0x1C };
            uint targetAddr2 = MemoryUtils.OffsetCalculator(helper, baseAddr2, offset2);

            DiscordRpcClient client = new DiscordRpcClient("656565390411038731");
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "Test",
                State = "N/A",
                Timestamps = Timestamps.Now,
                Buttons = new Button[]
                {
                    new Button() { Label = "Test", Url = "https://github.com/weexnes" }
                },

                Assets = new Assets()
                {
                    LargeImageKey = "whitecrown",
                    LargeImageText = "NFS ProStreet v1.1",
                }
            });
            while (true)
            {
                string test = helper.ReadMemory<ulong>(targetAddr1).ToString();
                string xp = helper.ReadMemory<ulong>(targetAddr2).ToString();

                Console.WriteLine(test + ":" + xp);
                int balance = Convert.ToInt32(test);
                int xpi = Convert.ToInt32(xp);

                string formatted_xp = xpi.ToString("#,##0");
                string formatted_balance = balance.ToString("#,##0");
                client.UpdateState(formatted_balance + "$");
                client.UpdateDetails("Raceday: " + formatted_xp + "xp");
                Thread.Sleep(200);
            }
        }
    }
}
