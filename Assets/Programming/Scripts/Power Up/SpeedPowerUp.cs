using Photon.Pun;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviourPun
{
    [SerializeField] private float lifeSpan = 3f;
    [SerializeField] private float detectionRadius = 1f;


    private Collider2D[] resultsBuffer = new Collider2D[4];
    private bool hasBeenCollected = false; 

    void Start()
    {

    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            detectionRadius,
            resultsBuffer
        );

        for (int i = 0; i < hitCount; i++)
        {
            if (resultsBuffer[i].CompareTag("Player"))
            {
                Debug.Log("Power Up Collected");
                PhotonView hitView = resultsBuffer[i].GetComponent<PhotonView>();
                PlayerController localPlayer = hitView.GetComponent<PlayerController>();
                if (hitView != null && !localPlayer.isOnPowerUp)
                {
                    hitView.RPC("ApplyEffect", RpcTarget.All, lifeSpan);
                    PhotonNetwork.Destroy(gameObject);
                    return; 
                }
            }
        }
    }
}