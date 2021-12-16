using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    [Header("Main Screen")]
    public Button createRoomButton;
    public Button joinRoomButton;

    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText;
    public Button startGameButton;

    private void Start()
    {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }

    // Called when we connect to the master server
    // Enables the create room and join room buttons
    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }

    void SetScreen (GameObject screen)
    {
        // deactivate all screens
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);

        // enable the requested screen
        screen.SetActive(true);
    }

    // called when the create room button is pressed
    public void OnCreateRoomButton (TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    // called when the join room button is pressed
    public void OnJoinRoomButton (TMP_InputField roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    // called when the player name input field has been updated
    public void OnPlayerNameUpdate (TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    // called when we join a room
    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);

        // since theres now a new player in the lobby, tell everyone to update the lobby
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    // called when a player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // OnJoinedRoom only called for the client who just joined
        // OnPLayerLeftRoom gets called for all clients in the room
        UpdateLobbyUI();
    }

    // updates the lobby UI to show player list and host buttons
    [PunRPC]
    public void UpdateLobbyUI ()
    {
        playerListText.text = "";

        // display all players currently in the lobby
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
        }

        // only the host can start the game
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
        } else {
            startGameButton.interactable = false ;
        }
    }

    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    public void OnStartGameButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}
