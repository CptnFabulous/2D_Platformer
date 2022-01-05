using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public Collider2D collider;
    public Rigidbody2D rigidbody;

    [Header("DPS")]
    public UnityEvent onShoot;
    public int projectileCount = 1;
    public float roundsPerMinute = 600;
    public float shotDelay
    {
        get
        {
            return 60 / roundsPerMinute;
        }
    }

    [Header("Accuracy")]
    public Transform muzzle;
    public float spread = 1;
    public float range = 100;
    public ContactFilter2D filter;

    [Header("Ammunition")]
    public Resource capacity;
    public int ammoPerShot = 1;
    public ResourceMeter meter;

    float lastTimeShot;

    public AimController PlayerUsing
    {
        get
        {
            return GetComponentInParent<AimController>();
        }
    }

    void Awake()
    {
        meter.Generate(capacity);

        AimController p = PlayerUsing;
        if (p != null)
        {
            Pickup(p);
        }
        else
        {
            Drop();
        }
    }

    public void Shoot()
    {
        // If firing cycle has not finished or there is no remaining ammo, don't do anything
        if (Time.time - lastTimeShot < shotDelay || capacity.current < ammoPerShot)
        {
            return;
        }

        lastTimeShot = Time.time;
        capacity.current -= ammoPerShot;
        meter.Refresh(capacity);
        onShoot.Invoke();
        
        for (int i = 0; i < projectileCount; i++)
        {
            AimController player = PlayerUsing;
            float angle = player.AimAngle + Random.Range(-spread, spread);
            Vector3 origin = player.weaponAxis.position;
            Vector3 direction = player.AngleToAimDirection(angle);


            RaycastHit2D[] results = new RaycastHit2D[2];
            int rn = Physics2D.Raycast(origin, direction, filter, results, range);

            //RaycastHit2D r = Physics2D.Raycast(origin, direction, range, filter.layerMask);

            Vector3 target;
            if (results[1].collider != null)
            {
                target = results[1].point;
            }
            else
            {
                target = origin + direction * range;
            }

            Debug.DrawLine(muzzle.position, target, Color.red, shotDelay);
        }
        
    }

    public void Pickup(AimController newPlayer)
    {
        rigidbody.bodyType = RigidbodyType2D.Static;
        collider.enabled = false;
        transform.parent = newPlayer.weaponAxis;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        meter.gameObject.SetActive(true);
        meter.transform.parent = newPlayer.weaponHUDSocket;
        meter.transform.localPosition = Vector3.zero;
        meter.transform.localRotation = Quaternion.identity;
    }
    public void Drop()
    {
        meter.gameObject.SetActive(false);
        meter.transform.parent = transform;
        transform.parent = null;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        collider.enabled = true;
    }
}
