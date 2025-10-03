using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class LevelManager : AbstractSingleton<LevelManager>
{
    private List<PlayerController> playerList = new List<PlayerController>();
    private List<PlayerController> deathPlayers = new List<PlayerController>();
    [SerializeField] private List<GameObject> spawnPositions = new List<GameObject>();
    private List<SpeedPowerUp> speedPowerUps = new List<SpeedPowerUp>();
    [SerializeField] private Transform[] powerUpPositions = new Transform[2];

    private bool gameEnded;

    private Dictionary<PlayerController, int> playerPoints = new Dictionary<PlayerController, int>();
    private GameObject SpawnPositionsGO;
    private int currentRound;
    private int MaxRounds;
    [SerializeField] private int maxPoints;
    private PhotonView photonView;

    [SerializeField] private GameObject WinScreen, DefeatScreen;
    public PhotonView PhotonView => photonView;

    [SerializeField] private BallSpawner ballSpawner;

    public int team1Points = 0;
    public int team2Points = 0;

    public override void Awake()
    {
        Instance = this;
        //base.Awake();
    }

    private void Start()
    {
        StartMatch();
    }
    public void StartMatch()
    {
        ballSpawner.SpawnBallToTeam1(true);
    }

    public void AddPointToTeam1(bool toTeamOne)
    {
        if (toTeamOne)
        {
            team1Points++;

            if (team1Points >= maxPoints)
            {
                SetWinner();
            }
        }
        else
        {
            team2Points++;

            if (team2Points >= maxPoints)
            {
                SetWinner();
            }
        }

   
    }

    public void SetWinner()
    {

    }

    [PunRPC]
    public void StartNewRound()
    {
        if (!gameEnded)
        {
            foreach (var player in deathPlayers)
            {
                if (!playerList.Contains(player)) // Verificación adicional
                {
                    playerList.Add(player);
                }
                player.photonView.RPC("RPC_Revive", RpcTarget.AllBuffered, player.photonView.ViewID);
            }

            deathPlayers.Clear();

            if (PhotonNetwork.IsMasterClient)
            {
                DestroyAllPowerUps();
            }

            speedPowerUps.Clear();


            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("ResetSpawnPoints", RpcTarget.MasterClient);
                photonView.RPC("ChangeCurrentRound", RpcTarget.MasterClient);
                photonView.RPC("ResetPositions", RpcTarget.MasterClient);
                photonView.RPC("SpawnPowerUps", RpcTarget.MasterClient);
            }
        }

    }

    private void DestroyAllPowerUps()
    {
        SpeedPowerUp[] allPowerUps = FindObjectsOfType<SpeedPowerUp>();
        foreach (var powerUp in allPowerUps)
        {
            if (powerUp != null && powerUp.gameObject != null)
            {
                PhotonNetwork.Destroy(powerUp.gameObject);
            }
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
        PhotonView.Find(viewId);
        photonView.RPC("RegisterPlayerForAll", RpcTarget.All, viewId);

        foreach (var player in playerList)
        {
            if (!playerPoints.ContainsKey(player))
            {
                playerPoints.Add(player, 0);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.Find(viewId);
            photonView.RPC("RegisterPlayerForAll", RpcTarget.All, viewId);
            photonView.RPC("SpawnPowerUps", RpcTarget.MasterClient);
            photonView.RPC("ResetSpawnPoints", RpcTarget.MasterClient);
            photonView.RPC("ResetPositions", RpcTarget.MasterClient);
        }
    }


    [PunRPC]
    public void SpawnPowerUps()
    {
        if (speedPowerUps.Count > 0) return;
        foreach (var powerUp in powerUpPositions)
        {
            PhotonNetwork.Instantiate("SpeedBoost", powerUp.position, powerUp.rotation, 0);
            speedPowerUps.Add(powerUp.GetComponent<SpeedPowerUp>());
        }
    }

    [PunRPC]
    public void RemovePlayer(int playerViewID) 
    {
        // Buscar el PlayerController por ViewID
        PhotonView targetView = PhotonView.Find(playerViewID);

        PlayerController player = targetView.GetComponent<PlayerController>();

        playerList.Remove(player);
        deathPlayers.Add(player);
        Debug.Log($"{player.NickName} removido");
        CheckRemainingPlayers();
    }
    public void CheckRemainingPlayers()
    {
        if (!gameEnded)
        {
            if (playerList.Count == 1)
            {
                var player = playerList.First();
                playerPoints[player] += 1;
                photonView.RPC("StartNewRound", RpcTarget.MasterClient);
            }
        }

    }

    
    public PlayerController CheckWinner()
    {
        PlayerController temp = playerList[0];
        foreach (var player in playerPoints.Keys)
        {
            /*if (playerPoints[player] > playerPoints[temp])
            {
                temp = player;
            }*/
            if (playerPoints[player] >= maxPoints)
            {
                temp = player;
                return temp;
            }
        }
        return null;
        //Debug.Log(temp.NickName);
        //return temp;
    }

    [PunRPC]
    public void ChangeCurrentRound()
    {
        currentRound++;

        //if (currentRound > MaxRounds)
        //{
            //currentRound = MaxRounds;
        var winner = CheckWinner();
        if (winner != null)
        {
            GameEnded(winner);
        }
        //}
    }

    public void GameEnded(PlayerController winner)
    {
        gameEnded = true;
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ShowGameResult", RpcTarget.All, winner.photonView.ViewID);
        }
    }

    [PunRPC]
    public void ResetPositions()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            List<GameObject> temp = spawnPositions.ToList();

            if (temp.Count == 0)
            {
                return;
            }

            foreach (var player in playerList)
            {
                if (temp.Count == 0) break; // No hay más spawn points

                int randomNumber = Random.Range(0, temp.Count);
                GameObject targetSpawn = temp[randomNumber];
                player.photonView.RPC("ResetPos", RpcTarget.AllBuffered, targetSpawn.transform.position, targetSpawn.transform.rotation);
                temp.Remove(targetSpawn);
                Debug.Log($"Spawn points restantes: {temp.Count}");
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
    public void ShowGameResult(int winnerViewID)
    {
        //Check for local player on the list
        PlayerController localPlayer = playerList.Find(p => p.photonView.IsMine);
        if (localPlayer == null) return;

        WinScreen.SetActive(false);
        DefeatScreen.SetActive(false);

        if (localPlayer.photonView.ViewID == winnerViewID) //Compare winner ID with local player ID
        {
            WinScreen.SetActive(true);
        }
        else
        {
            DefeatScreen.SetActive(true);
        }
    }

    public void EndMatch()
    {
         PUNManager.Instance.LeaveRoom();
    }
}

