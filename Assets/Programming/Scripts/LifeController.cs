using Photon.Pun;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private PhotonView photonView;
    private SpriteRenderer sr;
    private PlayerController playerController => GetComponent<PlayerController>();
    [SerializeField] float MaxHealth;
    private float currentHealth;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();


        LevelManager.Instance.PhotonView.RPC("RoundStarted", RpcTarget.MasterClient, playerController);
        currentHealth = MaxHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log(photonView.Owner.ActorNumber);

        if (!photonView.IsMine) return;
  
        //Solo hago daño a quien debe, no a cualquier cliente
        Debug.Log(photonView.name);
        Die();
    }

    public void Die()
    {
        LevelManager.Instance.PhotonView.RPC("RemovePlayer", RpcTarget.MasterClient, playerController);
        photonView.RPC("SetInactive", RpcTarget.All);
    }
}
