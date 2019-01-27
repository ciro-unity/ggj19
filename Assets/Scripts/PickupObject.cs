using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupObject : MonoBehaviour
{
    public Character carrier;
    public GameObject tetrisPiecePrefab;

    public UnityAction<PickupObject> OnPickedUp;

    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Collider2D trigger, collider;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        trigger = GetComponentInChildren<Collider2D>();
    }

    public void PickedUp(Character newCarrier)
    {
        carrier = newCarrier;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.sortingOrder = 2;
        trigger.enabled = false;
        collider.enabled = false;

        if(OnPickedUp != null) OnPickedUp.Invoke(this); //notifies the spawn manager
    }

    public void DroppedDown()
    {
        carrier = null;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        spriteRenderer.sortingOrder = 0;
        trigger.enabled = true;
        collider.enabled = true;
    }
}
