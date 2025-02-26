using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;
    GameObject player;
    public GameObject projectilePrefab; 
    public float fireRate = 2f; 
    public float projectileSpeed = 50f;
    public float lifeTime = 5f;
    Transform firePoint;


    private float timer;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        
        firePoint = transform.Find("firePoint"); // Finds child named "FirePoint"
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position); 

        transform.LookAt(player.transform.position);
        if (Time.time > timer) 
        {
            Shoot();
            timer = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        Destroy(projectile, lifeTime);


    }

}
