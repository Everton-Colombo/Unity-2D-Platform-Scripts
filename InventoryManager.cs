using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject item;

    public float interactionDistance;

    public Vector2 throwForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            if(item == null)
                GrabItem();
            else
            {
                DropItem();
            }
    }

    public void GrabItem()
    {
        Vector2 position = transform.position;
        Vector2 localScale = transform.localScale;
        Vector2 rayOrigin = new Vector2(position.x + (localScale.x / 2) + (localScale.x > 0 ? +0.1f : -0.1f), position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * localScale, interactionDistance);
        Debug.DrawRay(rayOrigin, Vector2.right * ((localScale.x / 2) * interactionDistance), Color.cyan, 1);
        if (hit.collider != null)
        {
            Debug.Log("Inventory Manager: Did Hit");
            if (hit.transform.tag.Equals("Weapon"))
            {
                Weapon weaponScript = hit.transform.GetComponent<Weapon>();
                weaponScript.active = true;
                item = Instantiate(weaponScript.selfPrefab, transform);
                item.transform.localPosition = Vector2.one;
                Destroy(hit.transform.gameObject);
                item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                item.GetComponent<BoxCollider2D>().enabled = false;
            } else if (hit.transform.tag.Equals("Gun"))
            {
                Gun gunScript = hit.transform.GetComponent<Gun>();
                gunScript.active = true;
                item = Instantiate(gunScript.selfPrefab, transform);
                item.transform.localPosition = Vector2.one;
                Destroy(hit.transform.gameObject);
                item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                item.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void DropItem()
    {
        if (item != null)
        {
            item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            item.GetComponent<BoxCollider2D>().enabled = true;
            Rigidbody2D itemRB = Instantiate(item, item.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>(); 
            itemRB.AddForce(new Vector2(transform.localScale.x * throwForce.x * itemRB.mass, throwForce.y * itemRB.mass), ForceMode2D.Impulse);
            Destroy(item);
            item = null;
        }
    }
}
