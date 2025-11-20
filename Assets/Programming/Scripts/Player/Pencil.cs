using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pencil : Weapon
{
    [SerializeField] private GameObject LOCALDrawingPrefab;
    [SerializeField] private float spawnOffset;
    [SerializeField][Min(1)] private int circlesPerSecond;
    private float spawnTimer = 0;
    private bool isDrawing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Shoot()
    {
        base.Shoot();
        spawnTimer = 0;//dibuja uno apenas comienza
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
        spawnTimer = 0;
    }
    public override void UpdateWeapon()
    {
        base.UpdateWeapon();
        if (isDrawing)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)//Si ya paso el tiempo para spawnear el siguiente
            {
                spawnTimer = (float)(1f / circlesPerSecond);
                GameObject circle = PUNManager.Instance.InstantiateWithPhoton("Drawing", transform.position + (Vector3)(-1 * spawnOffset * MyMath.RotationToDirection(transform.eulerAngles.z)), new Quaternion());
                
            }
        }
    }

}
