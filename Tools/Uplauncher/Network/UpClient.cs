using Shadow.Sound.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uplauncher.Network
{
    public class UpClient
    {
        #region Déclaration
        private SimpleClient m_client;

        public event EventHandler<DisconnectedArgs> Disconnected;
        #endregion

        public UpClient(SimpleClient client)
        {
            m_client = client;

            if (client != null)
            {
                m_client.DataReceived += this.ClientDataReceive;
                m_client.Disconnected += this.ClientDisconnected;
            }
        }

        /// <summary>
        /// Permet de déconnecter le client
        /// </summary>
        public void Dipose()
        {
            m_client.DataReceived -= ClientDataReceive;
            m_client.Disconnected -= this.ClientDisconnected;

            m_client.Stop();
        }

        #region Events
        private void ClientDataReceive(object sender, SimpleClient.DataReceivedEventArgs e)
        {
        }

        private void ClientDisconnected(object sender, SimpleClient.DisconnectedEventArgs e)
        {
            OnDisconnected(new DisconnectedArgs(this));
        }
        private void OnDisconnected(DisconnectedArgs e)
        {
            if (Disconnected != null)
                Disconnected(this, e);
        }
        #endregion
        public class DisconnectedArgs : EventArgs
        {
            public UpClient Host { get; private set; }

            public DisconnectedArgs(UpClient host)
            {
                Host = host;
            }
        }

        public void Send(byte[] data)
        {
            if (m_client.Runing)
                m_client.Send(data);
        }
    }
}
