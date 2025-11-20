using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun//, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    [SerializeField] private float bulletDamage;
    private float lifeSpanTimer;
    private Vector2 direction;
    private Photon.Realtime.Player owner;
    public PlayerController shotBy { get; private set; }

    private bool canKillOwner = false;

    public Vector2 Direction { get => direction; }

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private double currentPacketTime;
    private double currentTime;
    private double timeToReachGoal;
    private double lastPacketTime;
    private Vector3 positionAtLastPacket;
    private Quaternion rotationAtLastPacket;

    void Start()
    {
        lifeSpanTimer = lifeSpan;

        if (PhotonNetwork.IsMasterClient)
        {
            LevelManager.Instance.AddBulletToList(gameObject);
        }
    }

    void Update()
    {
        /*if (!photonView.IsMine)
        {
            UpdateWithNetworkInfo();
        }*/

        if (photonView.IsMine)
        {
            if (lifeSpanTimer > 0)
            {
                lifeSpanTimer -= Time.deltaTime;
                if (lifeSpanTimer <= 0)
                {
                    lifeSpanTimer = 0;
                    PUNManager.Instance.DestroyWithPhoton(gameObject);
                }
            }
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void UpdateWithNetworkInfo()
    {
        timeToReachGoal = currentPacketTime - lastPacketTime;
        currentTime += Time.deltaTime;
        transform.position = networkPosition; //Vector3.Lerp(transform.position, networkPosition, (float)timeToReachGoal);
    }

    public void SetOwner(PhotonView newOwner)
    {
        owner = newOwner.Owner;
        shotBy = newOwner.GetComponent<PlayerController>();
    }
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Bullet")) return; // Por si acaso, aunque la colision es ignorada desde la matrix de project settings

        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView hitView = collision.gameObject.GetComponent<PhotonView>();
            if (hitView == null) return;
            if (hitView.Owner == owner) return; // Ignora al que disparó


            hitView.RPC("TakeDamage", RpcTarget.All, bulletDamage);

           if (hitView != shotBy.photonView)
            {
                shotBy.playerStats.OnKill();
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void Bounce(Vector2 newDir)
    {
        if (!photonView.IsMine) return;
        SetDirection(newDir);

        owner = null;
    }

    [PunRPC]
    public void RPC_DestroyBullet()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            /*currentTime = 0.0;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;*/
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            /*lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;*/
        }
    }


    private void OnDestroy()
    {
        LevelManager.Instance.RemoveBulletFromList(gameObject);
    }

}
