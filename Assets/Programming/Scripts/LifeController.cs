using Photon.Pun;
using UnityEngine;

public class LifeController : MonoBehaviourPun
{
    private SpriteRenderer sr;
    private PlayerController playerController => GetComponent<PlayerController>();
    [SerializeField] float MaxHealth;
    private float currentHealth;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        LevelManager.Instance.PhotonView.RPC("RoundStarted", RpcTarget.MasterClient, playerController);
        currentHealth = MaxHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        LevelManager.Instance.PhotonView.RPC("RemovePlayer", RpcTarget.MasterClient, playerController);
    }
}
