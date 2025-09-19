using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField][Min(0)] float movementSpeed;
    [SerializeField][Min(0)] float rotationSpeed;

    private float forwardAxis = 0;
    private float rotationAxis = 0;

    private PlayerInput inputs;
    private PlayerActions actions;

    private PhotonView photonView;

    public PhotonView PhotonView => photonView ?? GetComponent<PhotonView>();

    [SerializeField] TextMeshProUGUI textName;

    [SerializeField] private float bulletCooldown;
    private float bulletCooldownTimer = 0;

    private int playerID;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine)
        {
            playerID = PhotonNetwork.LocalPlayer.ActorNumber;
            inputs = GetComponent<PlayerInput>();
            actions = new PlayerActions();//Instancia las actions
            actions.Enable();
            photonView.RPC("OnSpawned", RpcTarget.All, playerID);
            actions.Gameplay.Shoot.performed += Shoot;
        }
    }


    [PunRPC]
    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void OnSpawned(int playerID)
    {
        Debug.Log("Player ID" + playerID + " joined");
    }

    [PunRPC]
    public void RPC_SetPlayerName(string playerName)
    {
        textName.text = playerName;
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
        if (bulletCooldownTimer <= 0)
        {
            GameObject newBulletGO = PhotonNetwork.Instantiate("bullet", transform.position, transform.rotation);
            newBulletGO.GetComponent<Bullet>().SetOwner(photonView.Owner);
            bulletCooldownTimer = bulletCooldown;
        }
    }

 
}
