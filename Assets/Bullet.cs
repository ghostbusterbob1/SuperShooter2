using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(collision.gameObject); 
            Destroy(gameObject); 
        }
        else if (collision.collider)
        {
            Destroy(gameObject);
        }
        
    }
}
