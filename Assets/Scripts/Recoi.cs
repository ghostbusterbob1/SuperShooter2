using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoi : MonoBehaviour
{
    Vector3 currentRotation, targetRotation;
    Vector3 currentPosition, targetPosition;
    public Transform cam;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject ADSTarget;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;

    [SerializeField] float kickBackZ;

    public float snappiness, returnAmount;

    private Vector3 recoilPositionOffset;
    private Quaternion recoilRotationOffset;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Smoothly return to original rotation
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        recoilRotationOffset = Quaternion.Euler(currentRotation);

        // Smoothly return to original position
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        recoilPositionOffset = currentPosition;

        // Apply these values in WeaponSway script (combined with sway)
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            recoil();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            gun.transform.position = Vector3.Lerp(gun.transform.position, ADSTarget.transform.position, Time.deltaTime * 10f);
        }
    }

    public void recoil()
    {
        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    public Vector3 GetRecoilOffset()
    {
        return recoilPositionOffset;
    }

    public Quaternion GetRecoilRotation()
    {
        return recoilRotationOffset;
    }
}
