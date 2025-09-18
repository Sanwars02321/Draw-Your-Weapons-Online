using Photon.Pun;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private PhotonView photonView;
    private SpriteRenderer sr;
    [SerializeField] float MaxHealth;
    private float currentHealth;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();
        
        currentHealth = MaxHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;     //Solo hago daño a quien debe, no a cualquier cliente

        Die();
    }

    public void Die()
    {
        photonView.RPC("SetInactive", RpcTarget.All);
    }
}
