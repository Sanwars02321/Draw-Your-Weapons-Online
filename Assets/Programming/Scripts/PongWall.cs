using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)//&& PhotonNetwork.IsMasterClient
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                collision.gameObject.GetComponent<Bullet>().InvertDirY();
            }
        }
    }
}
