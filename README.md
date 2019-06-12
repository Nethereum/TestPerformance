# TestPerformance

Quick test of multi threading performance using .net core

NOTE: If you run this directly on Visual Studio DISABLE DIAGNOSTICS when running on .Net core. It is 100x slower.

Overall tests results, obviously environments will differ (clique and parity poa)

|Total: 20000 reads of Account balance|
|---|
|.Net core, Parity, HTTP / RPC, Windows = ~3.6 seconds|
|.Net core, Parity, HTTP / RPC, Http Factory = ~3.995 seconds|
|.Net core, Parity, IPC, Windows = ~3.6 seconds|
| |
|.Net core, Geth, HTTP / RPC, Windows = ~2.8 seconds|
|.Net core, Geth, HTTP / RPC, Http Factory = ~2.75 seconds|
|.Net core, Geth, IPC, Windows = ~4.3 seconds|
| |



|Total: sending 500 transactions deployment ERC20 smart contracts, for the same account including estimating, get price, nonce management, etc|
|---|
|.Net core, Parity, HTTP / RPC, Windows = ~6 seconds|
|.Net core, Parity, HTTP / RPC, Http Factory = ~6 seconds|
|.Net core, Parity, IPC, Windows = ~9 seconds|
| |
|.Net core, Geth, HTTP / RPC, Windows = ~5 seconds|
|.Net core, Geth, HTTP / RPC, Http Factory = ~6 seconds|
|.Net core, Geth, IPC, Windows = ~9 seconds|
| |



### Old results including Linux and net462
|Total: 6000 reads of Account balance|
|---|
|.Net core, Parity, HTTP / RPC, Windows = ~1.8 seconds|
|.Net core, Parity, HTTP / RPC, Linux Ubuntu = ~1.8 seconds|
|.Net core, Parity, IPC, Windows = ~1.5 seconds|
| |
|.Net core, Geth, HTTP / RPC, Windows = ~2 seconds|
|.Net core, Geth, HTTP / RPC, Linux Ubuntu = ~2 seconds|
|.Net core, Geth, IPC, Windows = ~1.8 seconds|
| |


|.Net 462, Geth, HTTP / RPC, Windows = ~1.5 seconds|
|.Net 462, Geth, IPC, Windows = ~1.8 seconds|
| |
|.Net 462, Parity, HTTP / RPC, Windows = ~2 seconds|
|.Net 462, Parity, IPC, Windows = ~1.3 seconds|




