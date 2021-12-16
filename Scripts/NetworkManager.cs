using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Instance 
    public static NetworkManager instance;

    private void Awake()
    {
        // If an instance already exists and its not this one - destroy us
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else {
            // Set the Instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public void CreateRoom (string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }


    // Attempts to join a room 
    public void JoinRoom (string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // Changes the scene using Photon's system
    [PunRPC]
    public void ChangeScene (string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
