using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Get the Rigidbody2D component attached to Volt
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input processing
        // Horizontal is A/D or Left/Right arrows, Vertical is W/S or Up/Down arrows
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Physics calculations
        // Move the Rigidbody based on movement vector and speed
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
