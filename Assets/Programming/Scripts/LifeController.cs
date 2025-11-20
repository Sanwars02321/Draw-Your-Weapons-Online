using Photon.Pun;
using UnityEngine;

public class LifeController : MonoBehaviourPun
{
    private SpriteRenderer sr;
    private PlayerController playerController => GetComponent<PlayerController>();
    [SerializeField] float MaxHealth;
    private float currentHealth;
    public bool isDead { get; private set; } = false;

    private void Start()
    {
        sr = transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();

        LevelManager.Instance.PhotonView.RPC("RoundStarted", RpcTarget.All, photonView.ViewID);
        currentHealth = MaxHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            playerController.playerStats.OnDeath();
            photonView.RPC("Die", RpcTarget.All);
        }


    }

    [PunRPC]
    public void Die()
    {
        if (isDead) return;

        if (photonView.IsMine)
        {
            playerController.enabled = false;
        }

        sr.enabled = false;
        isDead = true;
        photonView.RPC("RPC_ToggleCollision", RpcTarget.All, false);
        photonView.RPC("RPC_ToggleNameTag", RpcTarget.All, false);

        if (photonView.IsMine)
        {
            LevelManager.Instance.PhotonView.RPC("RemovePlayer", RpcTarget.MasterClient, playerController.photonView.ViewID);
        }
    }

    [PunRPC]
    public void RPC_Revive(int revivedPlayerID)
    {
        PhotonView targetView = PhotonView.Find(revivedPlayerID);
        if (targetView == null) return;

        PlayerController targetPlayer = targetView.GetComponent<PlayerController>();
        LifeController lifeControllerTargetPlayer = targetView.GetComponent<LifeController>();

        if (targetPlayer != null && lifeControllerTargetPlayer.isDead)
        {
            lifeControllerTargetPlayer.isDead = false;

            if (targetPlayer.photonView.IsMine)             //Devuelve Inputs
            {
                targetPlayer.enabled = true;
            }

            if (lifeControllerTargetPlayer.sr != null)      //Devuelve Visuales
            {
                lifeControllerTargetPlayer.sr.enabled = true;
            }

            photonView.RPC("RPC_ToggleCollision", RpcTarget.All, true);
            photonView.RPC("RPC_ToggleNameTag", RpcTarget.All, true);
        }
    }
}
