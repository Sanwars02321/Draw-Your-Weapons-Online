using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PUNManager : MonoBehaviourPunCallbacks
{
    public static PUNManager Instance;
    private string roomName;
    public TMP_InputField roomNameInputField;
    public TMP_InputField playerNameInputField;


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // De lo contrario, esta es la instancia y la conservamos.
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        
    }
    public void ConnectToPUN()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void setRoomName()
    {
        roomName = roomNameInputField.text;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Menu");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        ButtonManager.instance.LoadResult(1);
        
    }

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    base.OnDisconnected(cause);
    //}

    public override void OnCustomAuthenticationFailed(string error)
    {
        Debug.Log(error);
        ButtonManager.instance.LoadResult(2);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created. Room name: "
            + roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined to room. Room name: " + roomName);

        PlayerPrefs.SetString("playerName", playerNameInputField.text);

        PhotonNetwork.LoadLevel("Level1");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Room left. Room name: "
           + roomName);
    }
    public GameObject InstantiateWithPhoton(string obj, Vector3 pos, Quaternion rot)
    {
        return PhotonNetwork.Instantiate(obj, pos, rot);
    }

    public Photon.Realtime.Player[] RoundStartWithPhoton()
    {
        return PhotonNetwork.PlayerList;
    }
}
