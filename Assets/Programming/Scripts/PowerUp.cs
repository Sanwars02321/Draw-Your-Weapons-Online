using System.Collections;
using UnityEngine;

public abstract class SpeedPowerUp : MonoBehaviourPun
{
    [SerializeField] protected float lifeSpan = 5f; // Campo normal, más claro
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        if (collision.CompareTag("Player"))
        {
            PhotonView hitView = collision.GetComponent<PhotonView>();
            if (hitView != null)
            {
                photonView.RPC(nameof(ApplyEffectRPC), RpcTarget.All, hitView.ViewID);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    private void ApplyEffectRPC(int playerViewID)
    {
        PhotonView targetView = PhotonView.Find(playerViewID);
        if (targetView != null)
        {
            PlayerController controller = targetView.GetComponent<PlayerController>();
            if (controller != null)
            {
                ApplyEffect(controller);
            }
        }
    }


    public abstract void ApplyEffect(PlayerController playerController);
    public abstract void DeactivateEffect(PlayerController playerController);
}