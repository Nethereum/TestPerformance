using System;
using System.Collections.Async;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.TransactionReceipts;
using Nethereum.StandardTokenEIP20;
using Nethereum.StandardTokenEIP20.ContractDefinition;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json.Linq;
using BigInteger = System.Numerics.BigInteger;

namespace TestPerformanceCore
{
    class Program
    {
        private static bool geth = true;
        private static string PlatformName => geth ? "Geth" : "Parity";
        private static string IPCPath => geth ? "geth.ipc" : "jsonrpc.ipc";
        static void Main(string[] args)
        {
            
            //RunReadTests().Wait();
            RunWriteTests().Wait();
        }

        public static async Task RunReadTests()
        {
            
            Console.WriteLine($"{PlatformName} IPC reading");
            var web3 = new Web3(new IpcClient(IPCPath));
            await RunReadTest((web3));

            Console.WriteLine($"{PlatformName} RPC reading");
            web3 = new Web3();
            await RunReadTest((web3));

            Console.WriteLine($"{PlatformName} RPC reading http client factory");
            await RunReadTest(GetWeb3UsingHttpClientFactory());

            Console.ReadLine();
        }

        public static Web3 GetWeb3UsingHttpClientFactory(IAccount account = null)
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            var client = httpClientFactory.CreateClient();

            var rpc = new SimpleRpcClient(new Uri("http://localhost:8545"), client);
            return account != null ? new Web3(account, rpc) : new Web3(rpc);
        }


        public static async Task RunWriteTests()
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var account = new Account(privateKey);

            Console.WriteLine($"{PlatformName} IPC writing");
            var web3 = new Web3(account, new IpcClient(IPCPath));
            await RunWriteTest((web3));

            Console.WriteLine($"{PlatformName} RPC writing");
            web3 = new Web3(account);
            await RunWriteTest((web3));

            Console.WriteLine($"{PlatformName} RPC writing http client factory");
            await RunWriteTest(GetWeb3UsingHttpClientFactory(account));

            Console.ReadLine();
        }

        public static async Task RunWriteTest(Web3 web3)
        {

            var listTasks = 500;

            var taskItems = new List<int>();

            for (var i = 0; i < listTasks; i++) taskItems.Add(i);

            var numProcs = Environment.ProcessorCount;
            var concurrencyLevel = numProcs * 2;
            var concurrentDictionary = new ConcurrentDictionary<int, string>(concurrencyLevel, listTasks * 2);

            var sw = Stopwatch.StartNew();
            Console.WriteLine("Deployment contracts:" + listTasks);
            await taskItems.ParallelForEachAsync(async (item, state) =>
            
            //foreach (var item in taskItems)
            {
                var txnx = await StandardTokenService.DeployContractAsync(web3,
                    new EIP20Deployment() {TokenName = "TST", InitialAmount = 1000, TokenSymbol = "TST"});

                concurrentDictionary.TryAdd(item, txnx); 
            }
             , maxDegreeOfParalellism: 10);

            sw.Stop();
            Console.WriteLine("Done sending transactions in " + sw.ElapsedMilliseconds + "ms");
            
            Console.WriteLine();
            Console.WriteLine("Polling receipts....");
            
            var pollService = new TransactionReceiptPollingService(web3.TransactionManager);
            for (var i = 0; i < listTasks; i++)
            {
                string txn = null;
                concurrentDictionary.TryGetValue(i, out txn);
                var receipt = await pollService.PollForReceiptAsync(txn);
                if (i == listTasks - 1)
                {
                    Console.WriteLine("Last contract address:");
                    Console.WriteLine(receipt.ContractAddress);
                }
            }

            Console.WriteLine();
        }


        public static async Task RunReadTest(Web3 web3)
        {
             long totalProcessed = 0;
             ConcurrentBag<BigInteger> cb = new ConcurrentBag<BigInteger>();

            var listAccounts = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                listAccounts.Add("0x12890d2cce102216644c59dae5baed380d84830c");
                listAccounts.Add("0x12890d2cce102216644c59dae5baed380d84830a");
            }

            var sw = Stopwatch.StartNew();
            await listAccounts.ParallelForEachAsync(async s =>
            {
                var balance = await web3.Eth.GetBalance.SendRequestAsync(s).ConfigureAwait(false);
                cb.Add(balance.Value);
                Interlocked.Add(ref totalProcessed, 1);
            });
            sw.Stop();

            Console.WriteLine("Done in " + sw.ElapsedMilliseconds + "ms");
            Console.WriteLine(totalProcessed);
            Console.WriteLine("Balance 1:" + cb.First());
            Console.WriteLine("Balance 2:" + cb.Last());

        }
    }
}

