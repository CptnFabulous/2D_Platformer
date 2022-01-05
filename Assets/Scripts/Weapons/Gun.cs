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
    public GunFireEffect shotEffect;
    public float roundsPerMinute = 600;
    public UnityEvent onShoot;
    public float shotDelay
    {
        get
        {
            return 60 / roundsPerMinute;
        }
    }

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

        shotEffect.Shoot(PlayerUsing);
    }

    public void Pickup(AimController newPlayer)
    {
        if (newPlayer.gun != null) // Do not assign if player is already holding a gun
        {
            Drop();
            return;
        }
        rigidbody.simulated = false;
        collider.enabled = false;
        transform.parent = newPlayer.weaponAxis;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        newPlayer.gun = this;

        meter.gameObject.SetActive(true);
        meter.transform.parent = newPlayer.weaponHUDSocket;
        meter.transform.localPosition = Vector3.zero;
        meter.transform.localRotation = Quaternion.identity;
    }
    public void Drop()
    {
        if (PlayerUsing != null)
        {
            PlayerUsing.gun = null;
        }

        // Resets euler angles
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        meter.gameObject.SetActive(false);
        meter.transform.parent = transform;
        transform.parent = null;
        rigidbody.simulated = true;
        collider.enabled = true;
    }
}
