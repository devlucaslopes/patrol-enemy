using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(8 * direction, rb.velocity.y);
    }
}
