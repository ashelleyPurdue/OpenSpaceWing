using UnityEngine;
using System.Collections;

public class HealthPoints : AbstractHealthPoints
{
    public float maxHealth;

    protected float currentHealth;


    //Events
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        //Cool down
        if (isCoolingDown)
        {
            timer += Time.deltaTime;

            if (timer >= cooldownTime)
            {
                timer = 0f;
                isCoolingDown = false;
            }
        }
    }

    //Interface
    public override void DealDamage(float amount)
    {
        //Get hurt
        currentHealth -= amount;

        //Start cooling down if the cooldownTime is greater than zero
        if (cooldownTime > 0)
        {
            isCoolingDown = true;
            timer = 0f;
        }

        //Broadcast the OnTakeDamage method
        BroadcastMessage("OnTakeDamage", SendMessageOptions.DontRequireReceiver);

        //Die if out of health
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
