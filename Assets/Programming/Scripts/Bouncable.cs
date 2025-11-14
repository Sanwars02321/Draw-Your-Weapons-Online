using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncable : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        //if(!PhotonNetwork.IsMasterClient) return;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Vector2 normal = collision.contacts[0].normal;
            Bullet bull = collision.gameObject.GetComponent<Bullet>();
            Vector2 newDirection = Vector2.Reflect(bull.Direction, normal);
            //CABMIA LA ROTACION DE LA BALA. PARA ONLINE PASARLO A BULLET
            bull.Bounce(newDirection);
        }
    }
}
