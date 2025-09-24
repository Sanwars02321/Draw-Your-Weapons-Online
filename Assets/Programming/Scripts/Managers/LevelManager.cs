using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : AbstractSingleton<LevelManager>
{

    private List<PlayerController> playerList = new List<PlayerController>();
    private List<PlayerController> deathPlayers = new List<PlayerController>();
    [SerializeField] private List<GameObject> spawnPositions = new List<GameObject>();
    private Dictionary<PlayerController, int> playerPoints = new Dictionary<PlayerController, int>();
    private GameObject SpawnPositionsGO;
    [SerializeField] private int currentRound;
    [SerializeField] private int MaxRounds;
    private PhotonView photonView;
    public PhotonView PhotonView => photonView;

    public override void Awake()
    {

        photonView = GetComponent<PhotonView>();
        base.Awake();
    }



    [PunRPC]
    public void StartNewRound()
    {
        foreach (var player in deathPlayers)
        {
            playerList.Add(player);
        }
        ChangeCurrentRound();
        ResetPositions();
    }

    [PunRPC]
    public void RoundStarted(PlayerController playerController)
    {
        playerList.Add(playerController);
        Debug.Log(playerController.gameObject.name + " Joined");

        SpawnPositionsGO = GameObject.Find("SPAWNPOINTS");
        ResetSpawnPoints();

        photonView.RPC("ResetPositions", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RemovePlayer(PlayerController player)
    {

        playerList.Remove(player);
        deathPlayers.Add(player);
        Debug.Log(player + "Removido");
        CheckRemainingPlayers();
    }

    public void CheckRemainingPlayers()
    {
        if (playerList.Count == 1)
        {
            var player = playerList.First();
            //Ganó 
            Debug.Log(player + "ganó");
            playerPoints[player] += 1;
            LevelManager.Instance.PhotonView.RPC("StartNewRound", RpcTarget.AllBuffered);
        }

    }

    public PlayerController CheckWinner()
    {
        PlayerController temp = playerList[0];
        foreach (var player in playerPoints.Keys)
        {
            if (playerPoints[player] > playerPoints[temp])
            {
                temp = player;
            }
        }
        return temp;
    }

    [PunRPC]
    public void ChangeCurrentRound()
    {
        currentRound++;

        if (currentRound > MaxRounds)
        {
            currentRound = MaxRounds;
            GameEnded(CheckWinner());
        }
    }

    [PunRPC]
    public void GameEnded(PlayerController winner)
    {
        Debug.Log("Ganó el jugador: " + winner.nickName);
    }

    [PunRPC]
    public void ResetPositions()
    {
        foreach (var player in playerList)
        {
            int randomNumber = Random.Range(0, spawnPositions.Count);
            GameObject targetSpawn = spawnPositions[randomNumber];
            player.gameObject.transform.position = targetSpawn.transform.position;
            player.gameObject.transform.rotation = targetSpawn.transform.rotation;
            spawnPositions.Remove(targetSpawn);
        }
    }

    public void ResetSpawnPoints()
    {
        foreach (Transform child in SpawnPositionsGO.transform)
        {
            if (!spawnPositions.Contains(child.gameObject))
            {
                spawnPositions.Add(child.gameObject);
            }
        }
    }

    [PunRPC]
    public void StartCounting()
    {

    }
}

