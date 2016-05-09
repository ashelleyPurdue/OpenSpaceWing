﻿using UnityEngine;
using System.Collections;

public class PlayerGunBehaviour : MonoBehaviour
{
    //Constants
    public const float MAX_DIST = 500;
    public const float SCREEN_SPHERECAST_RADIUS = 0.15f;
    public const float BULLET_SPHERECAST_RADIUS = 0.2f;

    public const float BULLET_SPEED = 200f;


    //Properties
    public Vector3 AimPoint
    {
        get { return aimPoint; }
    }

    public Vector3 AimPointNormal
    {
        get { return aimPointNormal; }
    }

    public Transform TargettedObject
    {
        get { return targettedObject; }
    }


    //Private fields
    private Vector3 gunPos = Vector3.zero;  //Position of the gun in local space
    private GameObject bulletOriginal;
    private DamageSource damageSrc;

    private Vector3 aimPoint = Vector3.zero;
    private Vector3 aimPointNormal = Vector3.forward;
    private Transform targettedObject = null;


    //Events
    void Awake()
    {
        bulletOriginal = Resources.Load<GameObject>("testBullet_prefab");
        damageSrc = GetComponent<DamageSource>();
    }

    void Update()
    {
        //Update the aim point
        UpdateAimPoint();

        //Look at aim point
        transform.LookAt(aimPoint);

        //Shoot a lazer where the player is aiming
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }


    //Misc methods

    private void Fire()
    {
        //Fires the bullet

        //Find the direction the bullet should travel in
        Vector3 bulletDir = (aimPoint - transform.TransformPoint(gunPos)).normalized;

        //Do a raycast so we can get the healthpoints of the object being fired at.
        HealthPoints targetHP = null;
        
        Ray ray = new Ray(transform.position, (aimPoint - transform.position).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, BULLET_SPHERECAST_RADIUS, Vector3.Distance(transform.position, aimPoint));

        float closestDist = float.MaxValue;
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closestDist)
            {
                targetHP = hit.transform.GetComponent<HealthPoints>();
                closestDist = hit.distance;
            }
        }

        //Create the bullet and fire it.
        TargettedBulletBehaviour bullet = TargettedBulletBehaviour.Create(transform.position, targetHP, damageSrc, closestDist / BULLET_SPEED);
        bullet.myRigidbody.AddForce(bulletDir * BULLET_SPEED, ForceMode.VelocityChange);
    }

    private void UpdateAimPoint()
    {
        //Returns the point in world space that the player is aiming at.

        //Transform the mouse position to a viewport point
        Vector3 mousePos = Vector3.zero;
        mousePos.x = Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x);
        mousePos.y = Mathf.InverseLerp(0, Screen.height, Input.mousePosition.y);

        //Do a spherecast.
        Ray ray = Camera.main.ViewportPointToRay(mousePos);
        RaycastHit[] hits = Physics.SphereCastAll(ray, SCREEN_SPHERECAST_RADIUS, MAX_DIST);

        //If no hits were found, just set it to the maximum distance
        if (hits.Length == 0)
        {
            aimPoint = ray.GetPoint(MAX_DIST);
            aimPointNormal = Vector3.forward;
            targettedObject = null;
            return;
        }

        //Choose the point closest to the camera
        RaycastHit closestHit = hits[0];

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].distance < closestHit.distance)
            {
                closestHit = hits[i];
            }
        }

        //Return the closest point
        aimPoint = closestHit.point;
        aimPointNormal = closestHit.normal;
        targettedObject = closestHit.transform;
    }
}
