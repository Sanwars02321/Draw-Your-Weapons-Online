using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

   [SerializeField] private Animator animator;

    public static ButtonManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void ConnectToServer()
    {
        PUNManager.Instance.ConnectToPUN();
        animator.SetBool("IsLoading", true);
        //animator.SetBool("Loaded", false);
        animator.SetBool("LoadFail", false);

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

    public void LoadResult(int result)
    {
        animator.SetBool("IsLoading", false);
        switch (result)
        {
            case 1:
                animator.SetBool("Loaded", true);
                break;
            case 2:
                animator.SetBool("LoadFail", true);
                break;
        }
    }
}
