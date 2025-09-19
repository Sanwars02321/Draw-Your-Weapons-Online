using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    [SerializeField] private float bulletDamage;
    private float lifeSpanTimer;
    private Photon.Realtime.Player owner;
    private bool canKillOwner = false;

    private PhotonView photonView;
    
    void Start()
    {
        lifeSpanTimer = lifeSpan;
        photonView = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (lifeSpanTimer > 0)
        {
            lifeSpanTimer -= Time.deltaTime;
            if (lifeSpanTimer <= 0)
            {
                lifeSpanTimer = 0;
                Destroy(gameObject);
            }
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void SetOwner(Photon.Realtime.Player newOwner)
    {
        owner = newOwner;
    }

    public Photon.Realtime.Player GetOwner()
    {
        return owner;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return; 

        if (other.CompareTag("Player"))        
        {
            PhotonView hitView = other.GetComponent<PhotonView>();         //Gets the PhotonView comp from the hit player

            if (hitView != null)
            {
                if (hitView.Owner == owner) return;                         // ignora al que disparó, cambiarse para cuando rebote
                hitView.RPC("TakeDamage", hitView.Owner, bulletDamage);     //Calls via RPC to the TakeDamage method from lifeController
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
