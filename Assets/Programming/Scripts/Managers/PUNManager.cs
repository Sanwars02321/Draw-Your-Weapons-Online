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
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            // La sala se destruye apenas queda vacía
            EmptyRoomTtl = 0,
        };
        var room = PhotonNetwork.CreateRoom(roomName, roomOptions);
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

    public void CloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        ButtonManager.instance.LoadResult(1);
    }

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


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        LevelManager.Instance.OnPlayerLeft.Invoke(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log(newMasterClient.ActorNumber + " is the new Master Client");
    }

    public GameObject InstantiateWithPhoton(string obj, Vector3 pos, Quaternion rot)
    {
        return PhotonNetwork.Instantiate(obj, pos, rot);
    }

    public GameObject InstantiateRoomObjectWithPhoton(string obj, Vector3 pos, Quaternion rot)
    {
        return PhotonNetwork.InstantiateRoomObject(obj, pos, rot);
    }

    public void DestroyWithPhoton(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
    }
    public Photon.Realtime.Player[] RoundStartWithPhoton()
    {
        return PhotonNetwork.PlayerList;
    }
}
