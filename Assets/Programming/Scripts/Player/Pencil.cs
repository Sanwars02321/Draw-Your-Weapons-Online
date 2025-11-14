using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pencil : Weapon
{
    [SerializeField] private GameObject LOCALDrawingPrefab;
    [SerializeField] private float spawnOffset;
    private bool isDrawing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Shoot()
    {
        base.Shoot();
        isDrawing = true;
        shootActionRef.canceled += CancelDrawing;
    }

    private void CancelDrawing(InputAction.CallbackContext callback)
    {
        CancelDrawing();

    }
    public void CancelDrawing()
    {
        isDrawing = false;
    }
    public override void FixedUpdateWeapon()
    {
        base.FixedUpdateWeapon();
        if (isDrawing)
        {
            
            GameObject circle = PUNManager.Instance.InstantiateWithPhoton("Drawing", transform.position, new Quaternion());

            circle.transform.position = transform.position +  (Vector3)( -1 * spawnOffset * MyMath.RotationToDirection(transform.eulerAngles.z)); 
        }
    }
}
