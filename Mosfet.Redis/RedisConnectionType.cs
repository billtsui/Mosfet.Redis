namespace Mosfet.Redis;

/*
 * SingleNode    单机模式
 * MasterSlave   主从模式
 * Sentinel      哨兵模式
 * Cluster       集群模式
 */
public enum RedisConnectionType
{
    SingleNode = 0,
    MasterSlave = 1,
    Sentinel = 2,
    Cluster = 3
}