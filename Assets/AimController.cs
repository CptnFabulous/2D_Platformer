using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public float sensitivity = 120f;
    public Transform aimOrigin;
    float aimAngle = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aimAngle -= Input.GetAxis("RotateAim") * sensitivity * Time.deltaTime;
        Debug.DrawRay(aimOrigin.position, AimDirection * 5, Color.red);
    }

    public Vector2 AimDirection
    {
        get
        {
            float aimRadian = aimAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(aimRadian), Mathf.Sin(aimRadian));
        }
    }
}
