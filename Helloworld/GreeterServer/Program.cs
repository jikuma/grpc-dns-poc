// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;

namespace GreeterServer
{
    class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
          var name = Environment.GetEnvironmentVariable("instancename");
          Console.WriteLine("Received request ");
          return Task.FromResult(new HelloReply { Message = "Hello " + request.Name + " from "+ name });
        }
    }

    class Program
    {
        const int Port = 30051;

        public static void Main(string[] args)
        {
            IEnumerable<ChannelOption> options = new List<ChannelOption>() { new ChannelOption("grpc.max_connection_age_ms", 500), new ChannelOption("grpc.max_connection_age_grace_ms", 500) }; //Maximum connection age of 40 sec
            Server server = new Server(options)
            {
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            //Console.ReadKey();
            Thread.Sleep(Timeout.Infinite);

            server.ShutdownAsync().Wait();
        }
    }
}
