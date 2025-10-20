// --------------------------------------------------------------------------
// Copyright (c) 2025 Bill Tsui. All rights reserved.
// Licensed under the GPLv3 License.
// 
// File: Strings.cs
// Author: Bill Tsui
// Date: 10 20, 2025 20:10
// Description:
// --------------------------------------------------------------------------

using StackExchange.Redis;

namespace Mosfet.Redis;

public partial class RedisClient
{
    public bool SetString(string key, string value)
    {
        return _db.StringSet(key, value);
    }

    public bool SetStringEx(string key, string value, int expireSeconds)
    {
        return _db.StringSet(key, value, TimeSpan.FromSeconds(expireSeconds));
    }
}