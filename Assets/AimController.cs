using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
    public Transform weaponAxis;
    public Camera playerCamera;

    [Header("Cosmetics")]
    public Canvas headsUpDisplay;
    public Transform weaponHUDSocket;
    public Image reticle;
    public LineRenderer reticleLine;
    public float minimumDistanceForReticleLine = 1.5f;

    Vector3 reticlePosition;
    public Vector2 aimDirection { get; private set; }
    public Vector3 aimDirection3D
    {
        get
        {
            return new Vector3(aimDirection.x, aimDirection.y, 0);
        }
    }
    public float AimAngle
    {
        get
        {
            return Vector2.SignedAngle(Vector2.up, aimDirection) + 90;
        }
    }
    

    [Header("Gun")]
    public Gun gun;
    


    void Update()
    {
        reticlePosition = Input.mousePosition;
        aimDirection = (reticlePosition - playerCamera.WorldToScreenPoint(weaponAxis.position)).normalized;
        
        weaponAxis.LookAt(weaponAxis.position + aimDirection3D, transform.up);

        if (Input.GetKey(KeyCode.Mouse0) && gun != null)
        {
            gun.Shoot();
        }
    }
    private void LateUpdate()
    {
        Vector3 reticleImagePosition = reticlePosition;
        reticleImagePosition.z = Vector3.Distance(playerCamera.transform.position, headsUpDisplay.transform.position);
        reticle.transform.position = playerCamera.ScreenToWorldPoint(reticleImagePosition);

        if (Vector3.Distance(weaponAxis.position, reticle.transform.position) > minimumDistanceForReticleLine)
        {
            reticleLine.SetPosition(0, weaponAxis.position + (aimDirection3D * minimumDistanceForReticleLine));
            reticleLine.SetPosition(1, reticle.transform.position);
            reticleLine.enabled = true;
        }
        else
        {
            reticleLine.enabled = false;
        }

            
    }


    public Vector2 AngleToAimDirection(float angle)
    {
        float aimRadian = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(aimRadian), Mathf.Sin(aimRadian));
    }
    public Vector3 AngleToAimDirection3D(float angle)
    {
        float aimRadian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(aimRadian), Mathf.Sin(aimRadian), 0);
    }
    
}
