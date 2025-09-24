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
    [SerializeField][Min(0)] float rotationSpeed;

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
}
