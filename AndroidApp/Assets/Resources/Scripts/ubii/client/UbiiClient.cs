using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Jobs;
using System.Collections;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using Ubii.Services;
using Ubii.TopicData;
using Ubii.UtilityFunctions.Parser;

public class UbiiClient : MonoBehaviour, IUbiiClient
{
    NetMQUbiiClient client;

    [Header("Network configuration")]
    [Tooltip("Host ip the client connects to. Default is localhost.")]
    public string ip = "localhost";
    [Tooltip("Port for the client connection to the server. Default is 8101.")]
    public int port = 8101;

    public async Task InitializeClient()
    {
        client = new NetMQUbiiClient(null, "client", ip, port);
        await client.Initialize();
    }

    public Task<ServiceReply> CallService(ServiceRequest request)
    {
        return client.CallService(request);
    }

    public void Publish(TopicData topicData)
    {
        client.Publish(topicData);
    }

    public Task<ServiceReply> Subscribe(string topic, Action<TopicDataRecord> callback)
    {
        return client.Subscribe(topic, callback);
    }

    public Task<ServiceReply> Unsubscribe(string topic)
    {
        return client.Unsubscribe(topic);
    }


    private void OnDisable()
    {
        client.ShutDown();
        Debug.Log("Shutting down UbiiClient");
    }
}

