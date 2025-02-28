using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShoot : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] bool isAutomatic = false;
    [SerializeField] float fireRate = 0.1f;


    public GameObject bullet;
    public GameObject bulletPoint;
    public Text BulletCounter;
    public float bulletSpeed = 80f;
    public float bulletRange;
    public int bulletCount = 30;
    public int initialBullet;

    private bool isShooting = false;


    private void Start()
    {
       initialBullet = bulletCount;
    }

    void Update()
    {
        if (isAutomatic)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !isShooting)
            {
                StartCoroutine(AutomaticFire());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();   
        }


        
    }

    IEnumerator AutomaticFire()
    {
        isShooting = true;
        while (Input.GetKey(KeyCode.Mouse0) && bulletCount > 0)
        {
            Shoot();
            bulletCount--;
            BulletCounter.text = bulletCount.ToString();    

            Debug.Log(bulletCount.ToString());  
            yield return new WaitForSeconds(fireRate);


        }
        isShooting = false;
    }

    void Reload()
    {
       
            bulletCount = initialBullet;
            BulletCounter.text = bulletCount.ToString();
        
    }

    void Shoot()
    {
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        muzzleFlash.SetActive(true);
        StartCoroutine(WaitFlash(0.2f));

        float x = Screen.width / 2f;
        float y = Screen.height / 2f;
        var ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, bulletRange, layerMask))
        {
            Destroy(hit.transform.gameObject);
        }

        GameObject projectile = Instantiate(bullet, bulletPoint.transform.position, bulletPoint.transform.rotation);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        projectileRB.velocity = ray.direction * bulletSpeed;

        Destroy(projectile, 3f );
        
    }

    

    IEnumerator WaitFlash(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        muzzleFlash.SetActive(false);
    }
}
