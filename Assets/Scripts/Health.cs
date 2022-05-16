using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public Resource data;

    public UnityEvent<Resource> onDamage;
    public UnityEvent<Resource> onHeal;
    public UnityEvent onDeath;

    public void Damage(int amount)
    {
        if (data.depleted)
        {
            return;
        }

        data.Increment(-amount);
        if (amount < 0)
        {
            onHeal.Invoke(data);
        }
        else
        {
            onDamage.Invoke(data);
        }
        
        if (data.depleted)
        {
            onDeath.Invoke();
        }
    }
}