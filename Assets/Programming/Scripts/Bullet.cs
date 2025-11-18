using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    [SerializeField] private float bulletDamage;
    private float lifeSpanTimer;
    private Vector2 direction;
    private Photon.Realtime.Player owner;
    public PlayerController shotBy { get; private set; }

    private bool canKillOwner = false;

    public Vector2 Direction { get => direction; }

    void Start()
    {
        lifeSpanTimer = lifeSpan;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (lifeSpanTimer > 0)
            {
                lifeSpanTimer -= Time.deltaTime;
                if (lifeSpanTimer <= 0)
                {
                    lifeSpanTimer = 0;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void SetOwner(PhotonView newOwner)
    {
        owner = newOwner.Owner;
        shotBy = newOwner.GetComponent<PlayerController>();
    }
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView hitView = collision.gameObject.GetComponent<PhotonView>();         //Gets the PhotonView comp from the hit player
            PlayerController hitController = hitView.GetComponent<PlayerController>();

            if (hitView != null)
            {
                if (hitView.Owner == owner) return;                         // ignora al que dispar�, cambiarse para cuando rebote
                hitView.RPC("TakeDamage", RpcTarget.All, bulletDamage);     //Calls via RPC to the TakeDamage method from lifeController
                PhotonNetwork.Destroy(gameObject);
                hitController.playerStats.OnDeath();
                shotBy.playerStats.OnKill();
            }
        }
    }

    [PunRPC]
    public void Bounce(Vector2 newDir)
    {
        if (!photonView.IsMine) return;
        SetDirection(newDir);
        owner = null;
    }

}
