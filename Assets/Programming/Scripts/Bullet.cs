using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    private float lifeSpanTimer;
    private bool canKillOwner = false;
    
    void Start()
    {
        lifeSpanTimer = lifeSpan;
    }

    // Update is called once per frame
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

    public void SetBulletData()
    {

    }
}
