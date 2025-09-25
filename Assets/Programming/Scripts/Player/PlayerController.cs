using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField][Min(0)] float movementSpeed;
    [SerializeField][Min(0)] float speedBoostSpeed = 3f;
    [SerializeField][Min(0)] float initialSpeed;


    [SerializeField][Min(0)] float rotationSpeed;

    private bool hasWon;

    public bool isOnPowerUp { get; private set; }

    public bool HasWon {  get { return hasWon; }  set { hasWon = value; } }
    private float forwardAxis = 0;
    private float rotationAxis = 0;

    private PlayerInput inputs;
    private PlayerActions actions;
    public PhotonView PhotonView => photonView ?? GetComponent<PhotonView>();

    [SerializeField] TextMeshProUGUI textName;

    [SerializeField] private float bulletCooldown;
    private float bulletCooldownTimer = 0;
    private int playerID;

    private LifeController lifeController;
    private GameObject nickNameCanvas;
    private Collider2D playerCollider;

    [SerializeField] private string nickName;

    private bool isDead;

    public bool IsDead { get { return isDead; } set { isDead = value; } }
    public string NickName => nickName;
    public GameObject Canvas
    {
        get { return nickNameCanvas; }
        set { nickNameCanvas = value; }
    }

    private void Awake()
    {
        if (PhotonView.IsMine)
        {
            playerID = PhotonNetwork.LocalPlayer.ActorNumber;
            inputs = GetComponent<PlayerInput>();
            actions = new PlayerActions(); //Instancia las actions
            actions.Enable();
            photonView.RPC("OnSpawned", RpcTarget.All, playerID);
            actions.Gameplay.Shoot.performed += Shoot;
            initialSpeed = movementSpeed;
        }

        playerCollider = GetComponent<Collider2D>();
        lifeController = GetComponent<LifeController>();
        nickNameCanvas = transform.Find("Canvas").gameObject;
    }

    [PunRPC]
    public void OnSpawned(int playerID)
    {
        Debug.Log("Player ID" + playerID + " joined");
    }

    [PunRPC]
    public void RPC_ToggleCollision(bool action)
    {
        playerCollider.enabled = action;
    }

    [PunRPC]
    public void RPC_SetPlayerName(string playerName)
    {
        textName.text = playerName;
        nickName = playerName;
    }


    [PunRPC]
    public void ResetPos(Vector3 pos, Quaternion rotation)
    {
        if (photonView.IsMine)
        {
            gameObject.transform.position = pos;
            gameObject.transform.rotation = rotation;
        }
    }

    [PunRPC]
    public void RPC_ToggleNameTag(bool action)
    {
        if (nickNameCanvas != null)
            nickNameCanvas.SetActive(action);
    }

    public void SetNickname()
    {
        photonView.RPC("RPC_SetPlayerName", RpcTarget.AllBuffered, PlayerPrefs.GetString("playerName"));
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            CheckTimers();
            forwardAxis = actions.Gameplay.Move.ReadValue<float>();
            rotationAxis = actions.Gameplay.Rotate.ReadValue<float>();

            // Movimiento
            transform.Translate(Vector3.right * forwardAxis * movementSpeed * Time.deltaTime);

            // Rotación
            transform.Rotate(Vector3.forward * -rotationAxis * rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckTimers()
    {
        //Bullet Cooldown
        if (bulletCooldownTimer > 0)
        {
            bulletCooldownTimer -= Time.deltaTime;
            if (bulletCooldownTimer <= 0)
            {
                bulletCooldownTimer = 0;
            }
        }
    }
    private void Shoot(InputAction.CallbackContext callback)
    {
        if (photonView.IsMine && !lifeController.isDead)
        {
            if (bulletCooldownTimer <= 0)
            {
                GameObject newBulletGO = PhotonNetwork.Instantiate("bullet", transform.position, transform.rotation, 0);
                newBulletGO.GetComponent<Bullet>().SetOwner(photonView.Owner);
                bulletCooldownTimer = bulletCooldown;
            }
        }
    }

    [PunRPC]
    public void ApplyEffect(float lifeSpan)
    {
        isOnPowerUp = true;
        movementSpeed = speedBoostSpeed;
        StartCoroutine(RemoveEffectAfterTime(lifeSpan));
    }

    private IEnumerator RemoveEffectAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        movementSpeed = initialSpeed;
        isOnPowerUp = false;
    }
}
