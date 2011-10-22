﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading.Tasks;
using System.Threading;

namespace PloobsEngine.NetWorking
{
    public enum NetMessageType : short
    {
        PhysicSync = 0x01,
        UserDefined = 0x02
    }

    public class NetworkCliente
    {
        Dictionary<NetMessageType, List<Action<NetMessageType, NetIncomingMessage>>> messagehandler = new Dictionary<NetMessageType, List<Action<NetMessageType, NetIncomingMessage>>>();
        
        public void AddMessageHandler(NetMessageType messageType , Action<NetMessageType, NetIncomingMessage> handler)
        {
            if(messagehandler[messageType] == null)
            {
                messagehandler[messageType] = new List<Action<NetMessageType,NetIncomingMessage>>();
                messagehandler[messageType].Add(handler);
            }
            else
            {
                   messagehandler[messageType].Add(handler);
            }
        }

        public NetOutgoingMessage CreateMessage(NetMessageType messageType)
        {
            NetOutgoingMessage mes = client.CreateMessage();
            mes.Write((short)messageType);
            return mes;
        }

        public void SendMessage(NetOutgoingMessage mes,NetDeliveryMethod method = NetDeliveryMethod.Unreliable)
        {
            if (servercon == null)
                waitConnection();

            client.SendMessage(mes, servercon, method);
        }

        public void ProcessMessageSync()
        {
            if (servercon == null)
                waitConnection();

            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("recieved data message");
                        NetMessageType type =(NetMessageType) msg.ReadInt16();
                        if (messagehandler[type] != null)
                        {
                            foreach (var item in messagehandler[type])
                            {
                                item(type, msg);
                            }                            
                        }                        
                        break;
                }
            }
        }

        NetClient client;
        NetConnection servercon;
        public NetworkCliente(String serverName = "GameServer", int port = 14242,bool waitForConnect = true)
        {
            NetPeerConfiguration config = new NetPeerConfiguration(serverName);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();
            client.DiscoverLocalPeers(port);

            if (waitForConnect)
            {
                waitConnection();
            }
        }


        private void waitConnection()
        {
            NetIncomingMessage msg;            
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        servercon = client.Connect(msg.SenderEndpoint);
                        return;
                }
            }
        }
    }
}
