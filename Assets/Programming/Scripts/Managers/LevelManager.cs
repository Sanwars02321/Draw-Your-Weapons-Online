using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
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

    
    [SerializeField] private GameObject WinScreen, DefeatScreen;
    public PhotonView PhotonView => photonView;

    public override void Awake()
    {

        Instance = this;
        //base.Awake();
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        SpawnPositionsGO = GameObject.Find("SPAWNPOINTS");
    }

    [PunRPC]
    public void StartNewRound()
    {
        foreach (var player in deathPlayers)
        {
            playerList.Add(player);

            player.photonView.RPC("RPC_Revive", RpcTarget.AllBuffered, player.photonView.ViewID);
        }

        foreach (var player in playerList)
        {
            deathPlayers.Remove(player);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ResetSpawnPoints", RpcTarget.MasterClient);
            photonView.RPC("ChangeCurrentRound", RpcTarget.MasterClient);
            photonView.RPC("ResetPositions", RpcTarget.MasterClient);
        }

    }

    [PunRPC]
    public void RegisterPlayerForAll(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            PlayerController p_controller = pv.GetComponent<PlayerController>();
            if (p_controller != null && !playerList.Contains(p_controller))
            {
                playerList.Add(p_controller);
            }
        }
    }

    [PunRPC]
    public void UnregisterPlayerForAll(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            PlayerController p_controller = pv.GetComponent<PlayerController>();
            if (p_controller != null && !playerList.Contains(p_controller))
            {
                playerList.Add(p_controller);
            }
        }
    }

    [PunRPC]
    public void RoundStarted(int viewId)
    {
        

        foreach (var player in playerList)
        {
            if (!playerPoints.ContainsKey(player))
            {
                playerPoints.Add(player, 0);
            }
        }
        //Debug.Log(playerController.gameObject.name + " Joined");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.Find(viewId);
            photonView.RPC("RegisterPlayerForAll", RpcTarget.All, viewId);
            Debug.Log("PLAYERS: " + playerList.Count);
            photonView.RPC("ResetSpawnPoints", RpcTarget.MasterClient);
            photonView.RPC("ResetPositions", RpcTarget.MasterClient);
        }


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
            Debug.Log(player.NickName + "ganó");
            playerPoints[player] += 1;
            PhotonView.RPC("StartNewRound", RpcTarget.AllBuffered);
        }

    }

    [PunRPC]
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
        Debug.Log(temp.NickName);
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


    public void GameEnded(PlayerController winner)
    {
        Debug.Log("Ganó el jugador: " + winner.NickName);
        winner.HasWon = true;
        foreach (var player in playerList)
        {
            player.CheckState();
        }
    }

    [PunRPC]
    public void ResetPositions()
    {

        if (PhotonNetwork.IsMasterClient) {

            List<GameObject> temp = spawnPositions.ToList();

            foreach (var player in playerList)
            {
                int randomNumber = Random.Range(0, temp.Count);
                GameObject targetSpawn = temp[randomNumber];
                player.photonView.RPC("ResetPos", RpcTarget.AllBuffered, targetSpawn.transform.position, targetSpawn.transform.rotation);
                temp.Remove(targetSpawn);
                Debug.Log(spawnPositions.Count);
            }
        }
    }

    [PunRPC]
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

    public void AfterMatch(PlayerController player)
    {


        if (player.photonView.ViewID != CheckWinner().photonView.ViewID)
        {
            WinScreen.SetActive(false);
            DefeatScreen.SetActive(true);
            return;
        }

        else
        {
            DefeatScreen.SetActive(false);
            WinScreen.SetActive(true);
        }


    }
    public void EndMatch()
    {
        PUNManager.Instance.LeaveRoom();
    }
}

