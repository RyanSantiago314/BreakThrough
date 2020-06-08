using System;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class NetworkInstantiate : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private CharacterLoader loader;

        public bool allPlayersInstantiated = false;
        // Start is called before the first frame update
        private void Start()
        {
            loader = GameObject.Find("CharacterManager").GetComponent<CharacterLoader>();
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {

            //Most of this is properly setting online values from CharacterLoader.cs setP#Properties()
            if (PhotonNetwork.IsMasterClient && !info.Sender.Equals(PhotonNetwork.LocalPlayer)) //If we're master and message was not sent by us
            {
                
                //Three gameobject find's in a single call. need to fix this.
                //Sets Character manager
                loader.P2Character = this.gameObject;
            
                //Sets Player2 Prefab
                loader.setFullP2Properties(loader.P2Character);
                loader.P2Character.transform.parent = GameObject.Find("Player2").transform;
            
                loader.P1Character.SetActive(true);
                loader.P2Character.SetActive(true);
                //Setting Latestart bools for scripts.
                SetBools();

            }
        
            if(!PhotonNetwork.IsMasterClient && !info.Sender.Equals(PhotonNetwork.LocalPlayer))//If we're not master and message was not sent by us.
            {
                
                //Character Manager field
                loader.P1Character = this.gameObject;
            
                //Sets Player1 Prefab
                loader.setFullP1Properties(loader.P1Character);
                loader.P1Character.transform.parent = GameObject.Find("Player1").transform;

                loader.P1Character.SetActive(true);
                loader.P2Character.SetActive(true);
                SetBools();
            }
        
        
        }

        private void SetBools()
        {
            allPlayersInstantiated = true;
            loader.P1Character.GetComponent<MovementHandler>().networkInit = true;
        }
    }
}
