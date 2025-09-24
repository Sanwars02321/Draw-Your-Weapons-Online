using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpeedPowerUp : MonoBehaviourPun
{
    [SerializeField] protected float lifeSpan { get; set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("Player"))
        {
            PhotonView hitView = other.GetComponent<PhotonView>();   
            if (hitView != null)
            {
                hitView.RPC(nameof(ApplyEffect), RpcTarget.All);     
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    public abstract virtual void ApplyEffect();

}
