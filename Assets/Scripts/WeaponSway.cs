using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float bobbingSpeed = 10f;
    public float bobbingAmount = 0.05f;
    public float horizontalBobbingAmount = 0.02f;
    public Vector3 midpoint;

    private float timer = 0.0f;
    private Vector3 originalPosition;
    private float verticalRandomOffset;
    private float horizontalRandomOffset;

    [SerializeField] private float swaysmooth = 6f;
    [SerializeField] private float swayamount = 2f;
    [SerializeField] private float fallSwayAmount = 5f; // Added fall sway amount

    private Recoi recoilScript;
    public Rigidbody playerRigidbody;

    void Start()
    {
        originalPosition = transform.localPosition;
        midpoint = originalPosition;

        verticalRandomOffset = Random.Range(0f, Mathf.PI * 2);
        horizontalRandomOffset = Random.Range(0f, Mathf.PI * 2);

        recoilScript = GetComponent<Recoi>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            bobbingSpeed = 23f;
        }
        else
        {
            bobbingSpeed = 18f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float swayX = Input.GetAxisRaw("Mouse X") * swayamount;
        float swayY = Input.GetAxisRaw("Mouse Y") * swayamount;

        Quaternion swayRotationX = Quaternion.AngleAxis(-swayY, Vector3.right);
        Quaternion swayRotationY = Quaternion.AngleAxis(swayX, Vector3.up);
        Quaternion swayRotation = swayRotationX * swayRotationY;

        timer += bobbingSpeed * Time.deltaTime;
        if (timer > Mathf.PI * 2) timer -= Mathf.PI * 2;

        float waveSlice = Mathf.Sin(timer + verticalRandomOffset);
        float verticalOffset = waveSlice * bobbingAmount * Random.Range(0.9f, 1.1f);
        float horizontalOffset = Mathf.Cos(timer + horizontalRandomOffset) * horizontalBobbingAmount * Random.Range(0.9f, 1.1f);

        float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        totalAxes = Mathf.Clamp01(totalAxes);

        Vector3 swayPosition = new Vector3(midpoint.x + (horizontalOffset * totalAxes), midpoint.y + (verticalOffset * totalAxes), midpoint.z);

        Vector3 recoilOffset = recoilScript != null ? recoilScript.GetRecoilOffset() : Vector3.zero;
        Quaternion recoilRotOffset = recoilScript != null ? recoilScript.GetRecoilRotation() : Quaternion.identity;

        Vector3 fallEffect = Vector3.zero;
        Quaternion fallRotation = Quaternion.identity; 
        if (playerRigidbody != null && playerRigidbody.velocity.y < -1f)
        {
            float fallIntensity = Mathf.Abs(playerRigidbody.velocity.y);
            fallEffect = new Vector3(0f, fallIntensity * 0.05f, 0f);
            
            float fallTilt = Mathf.Sin(Time.time * 5f) * fallSwayAmount * (fallIntensity * 0.02f);
            fallRotation = Quaternion.Euler(fallTilt, 0f, fallTilt * 0.5f);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition + recoilOffset + fallEffect, Time.deltaTime * 5);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, swayRotation * recoilRotOffset * fallRotation, swaysmooth * Time.deltaTime);
    }
}
