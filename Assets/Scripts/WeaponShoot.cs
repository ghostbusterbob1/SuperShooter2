using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Camera camera;
    [SerializeField] bool isAutomatic = false;
    [SerializeField] float fireRate = 0.1f;

    public GameObject bullet;
    public GameObject bulletPoint;
    public float bulletSpeed = 80f;

    private bool isShooting = false;

    // Optional LineRenderer to visualize the ray/bullet trail
    [SerializeField] LineRenderer lineRenderer;

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
    }

    IEnumerator AutomaticFire()
    {
        isShooting = true;
        while (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
        isShooting = false;
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

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
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
