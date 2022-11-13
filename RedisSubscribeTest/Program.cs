// See https://aka.ms/new-console-template for more information
using EntityDataContract;
using StackExchange.Redis;

Console.WriteLine("Hello, World!");
using ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
var sub = redis.GetSubscriber();
//first subscribe, until we publish
//subscribe to a test message
sub.Subscribe(Consts.RedisChanelForNewEntity, (channel, message) => {
    Console.WriteLine("Got notification: " + (string)message);

});
