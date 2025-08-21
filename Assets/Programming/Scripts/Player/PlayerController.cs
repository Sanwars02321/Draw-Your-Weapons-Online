using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine)
        {
            inputs = GetComponent<PlayerInput>();
            actions = new PlayerActions();//Instancia las actions
            actions.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            forwardAxis = actions.Gameplay.Move.ReadValue<float>();
            rotationAxis = actions.Gameplay.Rotate.ReadValue<float>();
            //Debug.Log(forwardAxis);
            // Movimiento
            transform.Translate(Vector3.right * forwardAxis * movementSpeed * Time.deltaTime);

            // Rotación
            transform.Rotate(Vector3.forward * -rotationAxis * rotationSpeed * Time.deltaTime);
        }
    }
}
