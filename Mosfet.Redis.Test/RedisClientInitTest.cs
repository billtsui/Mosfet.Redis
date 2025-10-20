// --------------------------------------------------------------------------
// Copyright (c) 2025 Bill Tsui. All rights reserved.
// Licensed under the GPLv3 License.
// 
// File: RedisClient_InitTest.cs
// Author: Bill Tsui
// Date: 10 20, 2025 19:10
// Description: 192.168.31.162:6379 是内网运行在Linux上的Redis服务
// --------------------------------------------------------------------------

using Mosfet.Redis.Setting;
using StackExchange.Redis;

namespace Mosfet.Redis.Test;

[TestFixture]
public class RedisClientInitTest
{
    private RedisConfiguration configuration;

    [SetUp]
    public void SetUp()
    {
        configuration = new RedisConfiguration
        {
            ConnectionType = RedisConnectionType.SingleNode, EndPoints =
            [
                new RedisEndPoint
                {
                    Host = "192.168.31.162",
                    Port = 6379
                }
            ],
            AuthInfo = new RedisAuthInfo { Password = "admin1928" }
        };
    }

    [Test]
    public void Singleton_Test()
    {
        var client1 = RedisClient.GetInstance(configuration);
        var client2 = RedisClient.GetInstance(null);
        var client3 = RedisClient.GetInstance(configuration);
        Assert.That(client2, Is.SameAs(client1));
        Assert.That(client3, Is.SameAs(client1));
    }

    [Test]
    public void Multithread_Test()
    {
        var client1 = RedisClient.GetInstance(configuration);

        Task task1 = new Task(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                var client = RedisClient.GetInstance(null);
                Assert.That(client, Is.SameAs(client1));
            }
        });

        Task task2 = new Task(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                var client = RedisClient.GetInstance(null);
                Assert.That(client, Is.SameAs(client1));
            }
        });
        
        task1.Start();
        task2.Start();
    }
}