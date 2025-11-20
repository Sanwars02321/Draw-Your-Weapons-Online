using Photon.Pun;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10f, 5f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(-8f, 8f);

    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnRoundChanged.AddListener(OnRoundChanged);
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnCloud();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnCloud()
    {
        Vector3 spawnPos = new Vector3(
            UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0f
        );

        PhotonNetwork.Instantiate("Cloud", spawnPos, Quaternion.identity);
    }

    void OnRoundChanged()
    {
        spawnTimer = spawnInterval;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2f,
            (spawnAreaMin.y + spawnAreaMax.y) / 2f,
            0f
        );
        Vector3 size = new Vector3(
            spawnAreaMax.x - spawnAreaMin.x,
            spawnAreaMax.y - spawnAreaMin.y,
            0f
        );
        Gizmos.DrawWireCube(center, size);
    }

    void OnDestroy()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnRoundChanged.RemoveListener(OnRoundChanged);
        }
    }
}