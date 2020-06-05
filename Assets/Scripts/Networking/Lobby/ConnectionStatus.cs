using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Networking.Lobby
{
    public class ConnectionStatus : MonoBehaviour
    {
        //Connection status message. For debugging mainly, mostly adapted from PUN demos
        private readonly string statusMessage = "   Connection Status: ";
        //making them readonly we don't want to accidentally change this part of the string.

        [Header("UI object")]
        public Text StatusText;

        // Update is called once per frame
        void Update()
        {
            StatusText.text = statusMessage + PhotonNetwork.NetworkClientState;
        }
    }
}
