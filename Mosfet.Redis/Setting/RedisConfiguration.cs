// --------------------------------------------------------------------------
// Copyright (c) 2025 Bill Tsui. All rights reserved.
// Licensed under the GPLv3 License.
// 
// File: RedisConfiguration.cs
// Author: Bill Tsui
// Date: 10 20, 2025 01:10
// Description:
// --------------------------------------------------------------------------

namespace Mosfet.Redis.Setting;

public class RedisConfiguration
{
    public required RedisConnectionType ConnectionType { get; set; } = RedisConnectionType.SingleNode;
    public required List<RedisEndPoint> EndPoints { get; set; }
    public RedisAuthInfo? AuthInfo { get; set; }
    public int DbNumber { get; set; } = 0;
}