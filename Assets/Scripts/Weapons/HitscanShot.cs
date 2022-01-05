using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitscanShot : GunFireEffect
{
    public int damage = 1;
    public int projectileCount = 1;
    public float spread = 1;
    public float range = 100;
    public Transform muzzle;
    public ContactFilter2D filter;

    public UnityEvent<RaycastHit2D> onDamage;
    public UnityEvent<RaycastHit2D> onMiss;

    RaycastHit2D lastHit;

    public override void Shoot(AimController player)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = player.AimAngle + Random.Range(-spread, spread);
            Vector3 origin = player.weaponAxis.position;
            Vector3 direction = player.AngleToAimDirection(angle);


            RaycastHit2D[] results = new RaycastHit2D[2];
            int rn = Physics2D.Raycast(origin, direction, filter, results, range);

            //RaycastHit2D r = Physics2D.Raycast(origin, direction, range, filter.layerMask);

            Vector3 target;

            lastHit = results[1];
            if (lastHit.collider != null)
            {
                Health h = lastHit.collider.GetComponent<Health>();
                if (h != null)
                {
                    h.Damage(damage);
                    onDamage.Invoke(lastHit);
                }
                else
                {
                    onMiss.Invoke(lastHit);
                }

                target = results[1].point;
            }
            else
            {
                target = origin + direction * range;
            }

            Debug.DrawLine(muzzle.position, target, Color.red, 1);
        }
    }



    public void SpawnDebris(ParticleSystem debris)
    {
        ParticleSystem p = Instantiate(debris);
        p.gameObject.SetActive(true);
        p.transform.position = lastHit.point;
        p.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, lastHit.normal));
        p.Play();
    }
}
