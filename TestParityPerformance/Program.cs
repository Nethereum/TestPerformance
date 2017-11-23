using System;
using System.Collections.Async;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.Web3;

namespace TestParityPerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            long totalProcessed = 0;
            var cb = new ConcurrentBag<decimal>();
            var listAccounts = new List<string>();
            for (int i = 0; i < 200000; i++)
            {
                listAccounts.Add("0x12890d2cce102216644c59dae5baed380d84830c");
            }
            var web3 = new Nethereum.Web3.Web3(new IpcClient("jsonrpc.ipc"));
            var start = DateTime.Now;
            Console.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            await listAccounts.ParallelForEachAsync(async s =>
            {
                var balance =
                    await web3.Eth.GetBalance.SendRequestAsync(s);
                cb.Add(Web3.Convert.FromWei(balance.Value));
                Interlocked.Add(ref totalProcessed, 1);
            });
            var finish = DateTime.Now; 
            Console.WriteLine(finish);
            Console.WriteLine((finish - start).ToString());
            Console.WriteLine(totalProcessed);
            Console.ReadLine();
        }
    }
}
