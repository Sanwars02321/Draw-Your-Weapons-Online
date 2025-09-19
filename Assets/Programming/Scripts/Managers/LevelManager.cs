using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : AbstractSingleton<LevelManager>
{
   
    private List<PlayerController> playerList = new List<PlayerController>();
    private Dictionary<PlayerController, int> playerPoints = new Dictionary<PlayerController, int>();
   
    private PhotonView photonView;
    public PhotonView PhotonView => photonView;

    public override void Awake()
    {
        
        photonView = GetComponent<PhotonView>();
        base.Awake();
    }

    private void Start()
    {

    }

    [PunRPC]
    public void RoundStarted(PlayerController playerController)
    {
        playerList.Add(playerController);
        Debug.Log(playerController.gameObject.name + " Joined");

    }

    [PunRPC]
    public void RemovePlayer(PlayerController player)
    {
        
        playerList.Remove(player);
        Debug.Log(player + "Removido");
        CheckRemainingPlayers();
    }

    public void CheckRemainingPlayers()
    {
        if (playerList.Count == 1)
        {
           var player = playerList.First();
           //Ganó 
           Debug.Log(player + "ganò");
        }

    }
}
