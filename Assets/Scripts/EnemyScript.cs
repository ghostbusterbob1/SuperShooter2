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

    public float jumpForce = 10f; 
    public float jumpCooldown = 3f; 
    public float jumpChance = 0.3f;
    private bool canJump = true;
    public Rigidbody rb; 



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

        if (canJump && Vector3.Distance(transform.position, player.transform.position) < 10f)
        {
            if (Random.value < jumpChance) 
            {
                JumpTowardsPlayer();
            }
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

    void JumpTowardsPlayer()
    {
        canJump = false;
        agent.enabled = false;


        Vector3 jumpDirection = (player.transform.position - transform.position).normalized;
        jumpDirection.y = 1f; 

        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

        StartCoroutine(ResetJump());
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(jumpCooldown);
        agent.isStopped = false;
        canJump = true;
    }

  


}
