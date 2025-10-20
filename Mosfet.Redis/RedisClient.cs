// --------------------------------------------------------------------------
// Copyright (c) 2025 Bill Tsui. All rights reserved.
// Licensed under the GPLv3 License.
// 
// File: RedisClient.cs
// Author: Bill Tsui
// Date: 10 20, 2025 17:10
// Description:
// --------------------------------------------------------------------------

using StackExchange.Redis;
using Mosfet.Redis.Setting;

namespace Mosfet.Redis;

public partial class RedisClient
{
    private static RedisClient? _instance;
    private static readonly object _locker = new object();
    private static RedisConfiguration _configuration;
    private static ConnectionMultiplexer _connectionMultiplexer;
    private static IDatabase _db;
    private static bool isInitialized = false;

    private RedisClient()
    {
    }

    private static void Initialize(RedisConfiguration? config)
    {
        ConfigurationOptions confOptions;
        int dbNumber;
        /*
         * 默认值初始化
         * 127.0.0.1:6379
         */
        if (null == config)
        {
            confOptions = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" }
            };
            dbNumber = 0;
        }
        else
        {
            confOptions = new ConfigurationOptions();
            switch (config.ConnectionType)
            {
                case RedisConnectionType.SingleNode:
                    {
                        confOptions.EndPoints.Add(config.EndPoints[0].Host, config.EndPoints[0].Port);
                    }
                    break;
                case RedisConnectionType.Cluster:
                    {
                        foreach (var endPoint in config.EndPoints)
                        {
                            confOptions.EndPoints.Add(endPoint.Host, endPoint.Port);
                        }
                    }
                    break;
                default:
                    confOptions.EndPoints.Add(config.EndPoints[0].Host, config.EndPoints[0].Port);
                    break;
            }


            if (null != config.AuthInfo)
            {
                confOptions.User = config.AuthInfo.User;
                confOptions.Password = config.AuthInfo.Password;
            }

            dbNumber = config.DbNumber;
        }

        _connectionMultiplexer = ConnectionMultiplexer.Connect(confOptions);
        _db = _connectionMultiplexer.GetDatabase(dbNumber);
        RedisKey testKey = new RedisKey("Hello");
        if (_connectionMultiplexer is { IsConnected: true } && _db.IsConnected(testKey, CommandFlags.FireAndForget))
        {
            isInitialized = true;
        }
    }

    public static RedisClient GetInstance(RedisConfiguration? configuration)
    {
        if (null == _instance)
        {
            lock (_locker)
            {
                if (null == _instance)
                {
                    Initialize(configuration);
                    _instance = new RedisClient();
                }
            }
        }

        return _instance;
    }

    public RedisConfiguration GetConfiguration()
    {
        return _configuration;
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}