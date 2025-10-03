using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    [SerializeField] private bool scoresForTeam1;
    [SerializeField] private BallSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) // photonnetwork is client master
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                LevelManager.Instance.AddPointToTeam1(scoresForTeam1);
                Destroy(collision.gameObject);
                spawner.SpawnBallToTeam1(!scoresForTeam1);
            }
        }
    }
}
