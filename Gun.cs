using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType
    {
        Manual, Semiautomatic, Automatic
    }
    public GameObject selfPrefab;
    public GameObject projectilePrefab;
    public string weaponName;
    public GunType type;
    public float damage;
    public float recoilTime;
    public float projectileSpeed;
    public float maxProjectileDistance = 2500;
    [HideInInspector] public bool active;
    // [HideInInspector] public bool 
    
    void Update()
    {
        if(active)
            if(Input.GetKeyDown(KeyCode.F))
                StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectileInstance.damage = this.damage;
        projectileInstance.speed = this.projectileSpeed;
        projectileInstance.maxDistance = this.maxProjectileDistance;
        yield return new WaitForSeconds(recoilTime);
    }
}
