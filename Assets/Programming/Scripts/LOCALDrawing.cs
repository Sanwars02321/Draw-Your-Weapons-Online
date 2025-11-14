using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOCALDrawing : MonoBehaviour
{
    [SerializeField][Min(0.1f)] private float lifeSpan;
    private float aliveTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if (true)//Chequear si soy el owner
        {
            aliveTimer += Time.deltaTime;
            if (aliveTimer >= lifeSpan)
            {
                Destroy(gameObject);//PASARLO A FOTON
            } 
        }
    }
}
