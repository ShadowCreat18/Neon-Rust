using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonRust.Weapons; // Add reference to weapons namespace

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    [Header("Combat Settings")]
    public WeaponBase currentWeapon;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;
    private Camera cam;

    void Start()
    {
        // Get the Rigidbody2D component attached to Volt
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; // Cache the main camera for mouse position calculations
    }

    void Update()
    {
        // Input processing for movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Input processing for mouse aiming
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Shooting input
        if (Input.GetButton("Fire1") && currentWeapon != null)
        {
            currentWeapon.TryShoot();
        }
    }

    void FixedUpdate()
    {
        // Physics calculations for movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        // Physics calculations for rotation (look at mouse)
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // -90 offset assuming sprite faces up
        rb.rotation = angle;
    }
}
