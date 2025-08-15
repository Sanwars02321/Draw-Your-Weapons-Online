using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    void Start()
    {
        PhotonNetwork.Instantiate("Player1", spawnPoint.position, spawnPoint.rotation);
    }

    
    void Update()
    {
        
    }
}
