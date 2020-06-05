using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class NetworkInstantiate : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private CharacterLoader loader;
        // Start is called before the first frame update
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
        
            loader = GameObject.Find("CharacterManager").GetComponent<CharacterLoader>();
        
        
            //Most of this is properly setting online values from CharacterLoader.cs setP#Properties()
            if (PhotonNetwork.IsMasterClient && !info.Sender.Equals(PhotonNetwork.LocalPlayer)) //If we're master and message was not sent by us
            {
                loader.P1Character.GetComponent<MovementHandler>().networkInit = true;
                //Three gameobject find's in a single call. need to fix this.
                //Sets Character manager
                loader.P2Character = this.gameObject;
            
                //Sets Player2 Prefab
                loader.setFullP2Properties(loader.P2Character);
                loader.P2Character.transform.parent = GameObject.Find("Player2").transform;
            
                loader.P1Character.SetActive(true);
                loader.P2Character.SetActive(true);


            }
        
            if(!PhotonNetwork.IsMasterClient && !info.Sender.Equals(PhotonNetwork.LocalPlayer))//If we're not master and message was not sent by us.
            {
                loader.P2Character.GetComponent<MovementHandler>().networkInit = true;
                //Character Manager field
                loader.P1Character = this.gameObject;
            
                //Sets Player1 Prefab
                loader.setFullP1Properties(loader.P1Character);
                loader.P1Character.transform.parent = GameObject.Find("Player1").transform;

                loader.P1Character.SetActive(true);
                loader.P2Character.SetActive(true);
            }
        
        
        }
    }
}
