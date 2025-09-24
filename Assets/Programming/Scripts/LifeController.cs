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
            photonView.RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Die()
    {
        if (photonView.IsMine)
        {
            playerController.enabled = false;   //Le saca el control al jugador eliminado
        }

        sr.enabled = false;                     // Pero apaga el sprite renderer para todos 
        isDead = true;
        photonView.RPC("RPC_ToggleCanvas", RpcTarget.AllBuffered, false);

        LevelManager.Instance.PhotonView.RPC("RemovePlayer", RpcTarget.MasterClient, playerController);
    }

    [PunRPC]
    public void Respawn(Vector3 pos)        //Agregar como parametro que player sería el respawneado
    {
        currentHealth = MaxHealth;
        transform.position = pos;

        if (photonView.IsMine)
        {
            playerController.enabled = true;
            photonView.RPC("RPC_ToggleCanvas", RpcTarget.AllBuffered, true);
        }

        sr.enabled = true;
        isDead = false;
    }
}
