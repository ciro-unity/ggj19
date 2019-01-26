using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private float speed = 10f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Called on FixedUpdate by the GameManager
    public void Move(Vector2 input)
    {
        rigidbody2D.AddForce(input * speed * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    public void Dash()
    {
        
    }
}
