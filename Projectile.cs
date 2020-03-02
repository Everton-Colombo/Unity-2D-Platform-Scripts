using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Those are hidden because those values will be received from a Gun.cs instance
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public float maxDistance;
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine("checkForValues");
    }

    private void Update()
    {
        // There's room for optimization here: calling transform.position every frame will be very expensive.
        if(Vector2.Distance(initialPosition, transform.position) >= maxDistance)
            Destroy(gameObject);
        transform.Translate(speed * Time.deltaTime,0, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HealthManager victimHealthManager = other.transform.GetComponent<HealthManager>();
        if (victimHealthManager != null)
        {
            victimHealthManager.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    IEnumerator checkForValues()
    {
        while (damage == null || speed == null || maxDistance == null)
        {
            yield return null;
        }
    }
}

// TODO: add reassurance raycast ray so that projectiles don't miss potential targets.
