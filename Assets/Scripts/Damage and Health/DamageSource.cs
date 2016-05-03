using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageSource : MonoBehaviour
{
    public List<DamageTag> tags;
    public List<AbstractHealthPoints> ignoreList = new List<AbstractHealthPoints>();
    
    public float damageAmount = 0;

    public bool isHot = true;
    public bool useDefaultHitDetection = true;  //If true, will deal damage when colliding with a vulnerable AbstractHealthPoints


    //Events
    void OnTriggerEnter(Collider other)
    {
        OnHealthCollisionEnter(other.GetComponent<AbstractHealthPoints>());
    }
    
    void OnCollisionEnter(Collision col)
    {
        OnHealthCollisionEnter(col.collider.GetComponent<AbstractHealthPoints>());
    }
    

    //Public methods
    public void CopyDataTo(DamageSource target)
    {
        //Copies the damage tags, ignoreList, and damage amount to the target DamageSource.
        //isHot and useDefaultHitDetection are NOT copied over.

        target.tags = new List<DamageTag>(tags);
        target.ignoreList = new List<AbstractHealthPoints>(ignoreList);
        target.damageAmount = damageAmount;
    }

    //Misc methods
    private void OnHealthCollisionEnter(AbstractHealthPoints hp)
    {
        //Attack any HealthPoints
        if (hp != null && useDefaultHitDetection && isHot)
        {
            hp.AttackFrom(this);
        }
    }
}
