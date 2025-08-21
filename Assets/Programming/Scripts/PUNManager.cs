using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PUNManager : MonoBehaviourPunCallbacks
{
    private static PUNManager instance;

    private void Awake()
    {

    }

    public string roomName;
    public TMP_InputField roomNameInputField;
    public void ConnectToPUN()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("Room1");
    }

    public void setRoomName()
    {
        roomName = roomNameInputField.textComponent.text;
    }

    public void JoinRoom()
    {
        //if (string.IsNullOrEmpty(roomName))
        //{
        //    Debug.Log("El nombre de la sala està vacio.");
        //    return;
        //}
        //roomName = roomNameInputField.text;

        PhotonNetwork.JoinRoom("Room1");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        //PhotonNetwork.JoinRandomRoom();
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
        Debug.Log("Joined to room. Room name: "+ roomName);

        PlayerPrefs.SetString("playerName", roomNameInputField.text);

        PhotonNetwork.LoadLevel("Level1");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Room left. Room name: "
           + roomName);
    }
}
