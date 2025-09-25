using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void ConnectToServer()
    {
        PUNManager.Instance.ConnectToPUN();
    }

    public void CreateRoom()
    {
        PUNManager.Instance.CreateRoom();
    }

   
    public void SetRoomName()
    {
        PUNManager.Instance.setRoomName();
    }

    public void JoinRoom()
    {
        PUNManager.Instance.JoinRoom();
    }

    public void LeaveRoom()
    {
        PUNManager.Instance.LeaveRoom();
    }
}
