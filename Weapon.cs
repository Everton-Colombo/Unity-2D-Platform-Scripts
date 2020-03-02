using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


public class Weapon : MonoBehaviour
{
    public GameObject selfPrefab;
    public string weaponName;
    public int range;
    public int damage;
    [HideInInspector] public bool active;

    private void Update()
    {
        if(active)
            if(Input.GetKeyDown(KeyCode.F))
                Attack();
            
    }

    public void Attack()
    {
        Vector2 parentLocalScale = transform.parent.localScale;
        Vector2 position = transform.position;
        Vector2 rayOrigin = new Vector2(position.x + (transform.localScale.x / 2 * parentLocalScale.x) + 0.1f, position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * parentLocalScale, range);
        Debug.DrawRay(rayOrigin, Vector2.right * parentLocalScale * range, Color.red, 1);
        Debug.Log("Weapon: Did Shot");
        if (hit.collider != null)
        {
            Debug.Log("Weapon: Did Hit");
            HealthManager victimHeathManager = hit.transform.GetComponent<HealthManager>();
            if (victimHeathManager != null)
                victimHeathManager.TakeDamage(damage);
        }
    }
        
}
