using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        
        if(collision.collider.tag == "Player")
        {
            Destroy(gameObject);
        }else if (collision.collider)
        {
            Destroy(gameObject);
        }
        
    }
}
