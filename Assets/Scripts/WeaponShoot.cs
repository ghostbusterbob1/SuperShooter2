using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Camera camera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }    
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
