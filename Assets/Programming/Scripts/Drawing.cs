using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviourPun
{
    [SerializeField][Min(0.1f)] private float lifeSpan;
    private float aliveTimer = 0;
    public PhotonView PhotonView => photonView ?? GetComponent<PhotonView>();

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)//Chequear si soy el owner
        {
            aliveTimer += Time.deltaTime;
            if (aliveTimer >= lifeSpan)
            {
                PUNManager.Instance.DestroyWithPhoton(gameObject);
            } 
        }
    }
}
