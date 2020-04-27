using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ListEntry : MonoBehaviour
{
    #region Public References
    [Header("UI References")]
    public Text PlayerNameText;
    public Image PlayerImage;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;
    #endregion

    #region Private variables
    private int playerID;
    private bool isPlayerReady;
    #endregion

    #region Unity Calls
    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            Hashtable initialProperties = new Hashtable() {{BREAKTHROUGH.PLAYER_READY, isPlayerReady}}; //MAYBE WE NEED TO SET OTHER PROPERTIES AS WELL.
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProperties);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                setPlayerReady(isPlayerReady);

                Hashtable properties = new Hashtable() {{BREAKTHROUGH.PLAYER_READY, isPlayerReady}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

                if(PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<LobbyManager>().LocalPlayerPropertiesUpdated();

                }
                
            });
        }
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }
    #endregion

    #region Public Methods
    public void Initialize(int playerId, string playerName)
    {
        playerID = playerId;
        PlayerNameText.text = playerName;
    }

    public void setPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Not Ready";
        PlayerReadyImage.enabled = playerReady;
    }

    #endregion

    #region Private Methods
    private void OnPlayerNumberingChanged() //Create the new SelectedCharManager here.
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if(p.ActorNumber == playerID)
            {
                // TODO: MODIFY PLAYER IMAGE COLOR SET
            }
        }
    }
    #endregion
}
