using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Character carrier;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PickedUp(Character newCarrier)
    {
        carrier = newCarrier;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.sortingOrder = 2;
    }

    public void DroppedDown()
    {
        carrier = null;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        spriteRenderer.sortingOrder = 0;
    }
}
