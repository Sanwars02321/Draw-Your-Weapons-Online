using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnBallToTeam1(bool toTeam1)
    {
        var newBall = Instantiate(ball);
        if (toTeam1) newBall.GetComponent<Bullet>().direction.x = -1;
        if (!toTeam1) newBall.GetComponent<Bullet>().direction.x = 1;

        float num = Random.Range(-1f, 1f);
        newBall.GetComponent<Bullet>().direction.y = num;
        newBall.GetComponent<Bullet>().direction.Normalize();
    }
   
}
