using Photon.Pun;
using UnityEngine;

public class CloudObject : MonoBehaviourPun, IPunObservable
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private Vector2 moveDirection = Vector2.right;

    [Header("Interpolation")]
    [SerializeField] private float interpolationSpeed = 10f;

    private Vector3 networkPosition;
    private float lifeTimer;

    void Start()
    {
        lifeTimer = lifeTime;
        networkPosition = transform.position;
        moveDirection = moveDirection.normalized;
        int random = Random.Range(0, 10);
        moveDirection = (random % 2 == 0) ? moveDirection : Vector2.left;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                networkPosition,
                Time.deltaTime * interpolationSpeed
            );
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
        }
    }
}