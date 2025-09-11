using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    void Start()
    {
        GameObject player = PUNManager.Instance.InstantiateWithPhoton("Player1", spawnPoint.position, spawnPoint.rotation);

        player.GetComponent<PlayerController>().SetNickname();
            
    }

    
    void Update()
    {
        
    }
}
