using Photon.Pun;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviourPun
{
    [SerializeField] private float lifeSpan = 5f;
    [SerializeField] private float detectionRadius = 1f;


    private Collider2D[] resultsBuffer = new Collider2D[4];
    private bool hasBeenCollected = false; 

    void Start()
    {

    }

    void Update()
    {
        if (!photonView.IsMine || hasBeenCollected) return;

        int hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            detectionRadius,
            resultsBuffer
        );

        for (int i = 0; i < hitCount; i++)
        {
            if (resultsBuffer[i].CompareTag("Player"))
            {
                PhotonView hitView = resultsBuffer[i].GetComponent<PhotonView>();
                if (hitView != null)
                {
                    hasBeenCollected = true; 
                    hitView.RPC("ApplyEffect", RpcTarget.All, lifeSpan);
                    PhotonNetwork.Destroy(gameObject);
                    return; 
                }
            }
        }
    }
}