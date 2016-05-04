using UnityEngine;
using System.Collections;

public class TargettedBulletBehaviour : MonoBehaviour
{
    public const string PREFAB_NAME = "targettedBullet_prefab";

    public HealthPoints targetHP;
    public DamageSource damageSrc;
    public Rigidbody myRigidbody;

    public float destroyTime;
    private float timer = 0f;

    public static TargettedBulletBehaviour Create(Vector3 position, HealthPoints targetHP, DamageSource damageSrc, float destroyTime)
    {
        //Creates a targetted bullet at the given spot

        GameObject obj = Instantiate(Resources.Load<GameObject>(PREFAB_NAME));
        obj.transform.position = position;

        //Configure the bullet behaviour
        TargettedBulletBehaviour tbb = obj.GetComponent<TargettedBulletBehaviour>();
        tbb.targetHP = targetHP;
        tbb.damageSrc = damageSrc;
        tbb.myRigidbody = obj.GetComponent<Rigidbody>();
        tbb.destroyTime = destroyTime;

        //Return the bullet behaviour
        return tbb;
    }


    //Events

    void FixedUpdate()
    {
        //Destroy when time is up, and deal the damage.
        timer += Time.deltaTime;
        if (timer >= destroyTime)
        {
            if (targetHP != null)
            {
                targetHP.AttackFrom(damageSrc);
            }

            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Attempt to deal damage to the other object.
        HealthPoints otherHP = other.transform.GetComponent<HealthPoints>();
        if (otherHP != null)
        {
            otherHP.AttackFrom(damageSrc);
        }

        //Destroy self
        GameObject.Destroy(gameObject);
    }
}
