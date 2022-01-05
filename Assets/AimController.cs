using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public Transform weaponAxis;
    public Transform weaponHUDSocket;
    public Camera playerCamera;


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
    

    


    void Update()
    {
        
        
        aimDirection = (Input.mousePosition - playerCamera.WorldToScreenPoint(weaponAxis.position)).normalized;
        //Debug.DrawRay(weaponAxis.position, aimDirection * 5, Color.cyan);
        //Debug.DrawRay(weaponAxis.position, AngleToAimDirection(AimAngle) * 5, Color.blue);




        //aimAngle -= Input.GetAxis("RotateAim") * sensitivity * Time.deltaTime;
        
        weaponAxis.LookAt(weaponAxis.position + aimDirection3D, transform.up);

        {
            float aimRadian = aimAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(aimRadian), Mathf.Sin(aimRadian));
        }


        //Debug.Log(direction + ", " + AimDirection(aimAngle));
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
