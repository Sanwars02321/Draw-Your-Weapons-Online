using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

   [SerializeField] private Animator animator;
    [SerializeField] Button Leaderboard;

    public static ButtonManager instance;

    public Button leaderboardButton;

    private void Awake()
    {
        instance = this;
        leaderboardButton.onClick.AddListener(() => SceneManager.LoadScene("Leaderboard"));
    }
    public void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PUNManager.Instance.ConnectToPUN();
            animator.SetBool("IsLoading", true);
            //animator.SetBool("Loaded", false);
            animator.SetBool("LoadFail", false);
        }
        else
        {
            animator.SetBool("Loaded", true);
        }
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
