using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    [SerializeField] private float bulletDamage;
    [SerializeField] public Vector3 direction;
    private float lifeSpanTimer;
    private Photon.Realtime.Player owner;
    private bool canKillOwner = false;
    
    void Start()
    {
        lifeSpanTimer = lifeSpan;
    }

    void Update()
    {
        if (true)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void InvertDirY()
    {
        direction.y = -direction.y;
    }
    public void InvertDirX()
    {
        direction.x = -direction.x;
    }
}
