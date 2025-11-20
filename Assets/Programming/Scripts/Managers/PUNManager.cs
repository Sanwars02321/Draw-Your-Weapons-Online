using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;

public class PUNManager : MonoBehaviourPunCallbacks
{
    public static PUNManager Instance;
    private string roomName;
    public TMP_InputField roomNameInputField;
    public TMP_InputField playerNameInputField;

    public TextMeshProUGUI WarningText;

   

    private List<RoomInfo> cachedRooms = new List<RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRooms = roomList;
    }

    public bool RoomNameExists(string roomName)
    {
        foreach (var room in cachedRooms)
        {
            if (room.Name == roomName)
                return true;  
        }
        return false;          
    }

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
        if (RoomNameExists(roomName))
        {
            SetWarning(WarningText, "La sala que intenta crear ya existe. Por favor, cambie el nombre ingresado y vuelva a intentarlo.");
            return;
        }

        if (string.IsNullOrEmpty(roomName))
        {
            SetWarning(WarningText, "El nombre de la sala está vacío. Por favor, complételo y vuelva a intentarlo.");
            return;
        }

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
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
           
           SetWarning(WarningText, "El nombre de la sala está vacío. Por favor, complételo y vuelva a intentarlo.");
            return;
        }

        if (!RoomNameExists(roomName))
        {
            SetWarning(WarningText, "La sala a la que intenta unirse no existe. Por favor, corrobore el nombre ingresado y vuelva a intentarlo.");
            return;
        }
        
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
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        base.OnErrorInfo(errorInfo);
        Debug.Log(errorInfo);
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

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    OnJoinFail.gameObject.SetActive(true);
    //    OnJoinFail.SetText(message);
    //}

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    base.OnCreateRoomFailed(returnCode, message);
    //    OnCreateRoomFail.gameObject.SetActive(true);
    //    OnCreateRoomFail.SetText(message);
    //}

    public void SetWarning(TextMeshProUGUI warningText, string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
    }

    public void DisableWarning()
    {
        WarningText.gameObject.SetActive(false);
    }
}
