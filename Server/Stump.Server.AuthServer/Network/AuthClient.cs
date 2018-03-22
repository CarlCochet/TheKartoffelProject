using System;
using System.Net.Sockets;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Messages.Custom;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Handlers.Connection;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.Network;
using Stump.Core.Reflection;
using Stump.Core.IO;
using System.Net;

namespace Stump.Server.AuthServer.Network
{
    public sealed class AuthClient : BaseClient
    {
        private string m_login;

        public AuthClient(Socket socket) : base(socket)
        {
            if (AuthServer.Instance.m_maintenanceMode)
            {
                CanReceive = true;

                Send(new SystemMessageDisplayMessage(true, 13, new string[0]));
                Disconnect();

                return;
            }

            Send(new ProtocolRequired(VersionExtension.ProtocolRequired, VersionExtension.ActualProtocol));

            Send(new HelloConnectMessage(CredentialManager.Instance.GetSalt(), CredentialManager.Instance.GetRSAPublicKey()));

            CanReceive = true;
            lock (ConnectionHandler.ConnectionQueue.SyncRoot)
                ConnectionHandler.ConnectionQueue.Add(this);
            InQueueUntil = DateTime.Now;
        }
        
        public string Login
        {
            get { return m_login; }
            set { m_login = value.ToLower(); }
        }

        public string Password
        {
            get;
            set;
        }

        public Account Account
        {
            get;
            set;
        }

        public UserGroupRecord UserGroup
        {
            get;
            set;
        }

        /// <summary>
        ///   True when the client is choising a server
        /// </summary>
        public bool LookingOfServers
        {
            get;
            set;
        }

        public DateTime InQueueUntil
        {
            get;
            set;
        }

        public bool QueueShowed
        {
            get;
            set;
        }



        public void Save()
        {
            AuthServer.Instance.IOTaskPool.AddMessage(SaveNow);
        }

        public void SaveNow()
        {
            AuthServer.Instance.IOTaskPool.EnsureContext();
            AuthServer.Instance.DBAccessor.Database.Save(Account);
        }

        #region CallBack
        public override void DisconectedCallBack(IAsyncResult asyncResult)
        {
            try
            {
                base.Runing = false;
                Socket client = (Socket)asyncResult.AsyncState;
                client.EndDisconnect(asyncResult);
                OnDisconnected(new DisconnectedEventArgs(Socket));
            }
            catch (System.Exception ex)
            {
                OnError(new ErrorEventArgs(ex));
            }
        }
        public override void ReceiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                Socket client = (Socket)asyncResult.AsyncState;
                if (client.Connected == false)
                {
                    Runing = false;
                    return;
                }
                if (Runing)
                {
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = client.EndReceive(asyncResult);


                        if (bytesRead == 0)
                        {
                            Runing = false;
                            //OnDisconnected(new DisconnectedEventArgs(Socket));
                            this.Disconnect();
                            return;
                        }
                        byte[] data = new byte[bytesRead];
                        Array.Copy(receiveBuffer, data, bytesRead);
                        buffer.Add(data, 0, data.Length);
                        ThreatBuffer();
                        var messagePart = DataReceivedEventArgs.Data;
                        this.currentMessage = null;
                        if (messagePart == null)
                        {
                            Disconnect();
                            return;
                        }
                        BigEndianReader Reader = new BigEndianReader(messagePart.Data);
                        Message message = MessageReceiver.BuildMessage((ushort)messagePart.MessageId, Reader);

                        Console.WriteLine(string.Format("[RCV] {0} -> {1}", this.IP, message));
                        if (message is BasicPingMessage)
                        {
                            Send(new BasicPongMessage((message as BasicPingMessage).quiet));
                        }
                        else
                        {
                            // AuthPacketManager.ParseHandler(this, message);
                            Singleton<AuthPacketHandler>.Instance.Dispatch(this, message);

                        }

                        client.BeginReceive(receiveBuffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallBack), client);

                    }
                    catch (System.Exception ex)
                    {
                        logger.Error(ex.ToString());
                        //OnError(new ErrorEventArgs(ex));
                        Disconnect();
                    }
                }
                else
                    Console.WriteLine("Receive data but not running");
            }
            catch { }
            
        }
        public override void SendCallBack(IAsyncResult asyncResult)
        {
            try
            {
                Socket client = (Socket)asyncResult.AsyncState;
                client.EndSend(asyncResult);
                OnDataSended(new DataSendedEventArgs());

            }
            catch (System.Exception ex)
            {
                OnError(new ErrorEventArgs(ex));
            }
        }
        #endregion
        public new void Disconnect()
        {
            if(Account != null)
            {
                this.Save();
            }
            AuthServer.Clients.Remove(this);
            base.Stop();
        }
        public new void DisconnectLater(int duration = 0)
        {
            AuthServer.Instance.IOTaskPool.CallDelayed(duration, Disconnect);
        }
        public new void Send(Message message)
        {
            try
            {
                var writer = new BigEndianWriter();
                message.Pack(writer);
                base.Send(writer.Data);
                Console.WriteLine(string.Format("[SND] {0} -> {1}", IP, message));
            }
            catch(Exception e)
            {
                logger.Error("Methode Send error: " + e);
            }
        }

        public override string ToString()
        {
            return base.ToString() + (Account != null ? " (" + Account.Login + ")" : "");
        }
    }
}