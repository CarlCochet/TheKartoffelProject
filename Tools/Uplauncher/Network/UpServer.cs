using Shadow.Sound.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Uplauncher.Network
{
    public class UpServer
    {
        #region Déclaration
        private SimpleServer m_server;
        private UpClient m_client;
        #endregion


        public UpServer()
        {
            m_server = new SimpleServer();
        }

        public void StartAuthentificate()
        {
            m_server.Start(4242);
            m_server.ConnectionAccepted += AccepteClient;
        }

        #region Socket auth
        private void AccepteClient(Socket client)
        {
            var newClient = new SimpleClient(client);
            m_client = new UpClient(newClient);
        }

        private void ClientDisconnected(object sender, UpClient.DisconnectedArgs e)
        {
            m_client = null;
        }
        #endregion

        public UpClient Client
        {
            get { return m_client; }
        }

        public bool IsStart
        {
            get
            {
                return m_server.Connected;
            }
        }
    }
}
