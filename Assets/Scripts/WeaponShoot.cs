using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Camera camera;
    [SerializeField] bool isAutomatic = false;  
    [SerializeField] float fireRate = 0.1f;  

    private bool isShooting = false;  

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

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, 15f, layerMask))
        {
            Destroy(hit.transform.gameObject);
        }
    }

    IEnumerator WaitFlash(float waitime)
    {
        yield return new WaitForSeconds(waitime);
        muzzleFlash.SetActive(false);
    }
}
