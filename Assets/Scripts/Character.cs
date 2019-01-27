﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public UnityAction<Vector2, Character> DashLanded;

    [Header("Character data")]
    public int playerId;
    public PlayMode playMode = PlayMode.Action;
    public enum PlayMode
    {
        Action,
        Tetris,
    }

    public CharacterState charState = CharacterState.Walking;
    public enum CharacterState
    {
        Walking,
        Dashing,
        Dodging,
        Dizzy
    }

    [Header("Movement speeds")]
    public float speed = 200f;
    public float dashSpeed = 200f;
    public float dodgeSpeed = 200f;

    [Tooltip("Can't dash or dodge if below this movement treshold")]
    public float dashDodgeSpeedTreshold = .2f;
    public float dashPushStrength = 200f;

    [Header("Recovery delays")]
    [Tooltip("When using the dash")]
    public float endDashDelay = 1f;
    [Tooltip("When using the dodge")]
    public float endDodgeDelay = 1f;
    [Tooltip("When getting hit by a dash")]
    public float endDizzyDelay = 1f;
    
    //public float walkDecayTreshold = .7f, walkDecayAmount = .5f;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private Vector2 inputVector; //cached input
    private PickupObject intersectedPickup = null, carriedPickup = null;
    private float originalSpeed;
    private float speedReductionWhenCarrying = .8f;

    private int hashedWalking, hashedDash, hashedEndSpecial, hashedHorizontal, hashedVertical;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        hashedWalking = Animator.StringToHash("Walking");
        hashedDash = Animator.StringToHash("Dash");
        hashedEndSpecial = Animator.StringToHash("EndSpecial");
        hashedHorizontal = Animator.StringToHash("Horizontal");
        hashedVertical = Animator.StringToHash("Vertical");

        originalSpeed = speed;
    }

    //Called on FixedUpdate by the GameManager
    public void Move(Vector2 input)
    {
        if(charState == CharacterState.Walking)
        {
            inputVector = input;
            rigidbody2D.MovePosition(rigidbody2D.position + inputVector * speed * Time.fixedDeltaTime);

            //animator.SetBool(hashedWalking, inputVector.sqrMagnitude > .05f);
            animator.SetFloat(hashedHorizontal, inputVector.x);
            animator.SetFloat(hashedVertical, inputVector.y);
        }
    }


    //--------------------- Primary action: pickup and release
    public void PerformAction()
    {
        if(carriedPickup == null
            && intersectedPickup != null)
        {
            PickupObject();
        }
        else if(carriedPickup != null)
        {
            DropPickup();
        }
    }

    private void PickupObject()
    {
        //pick up what's on the ground
        intersectedPickup.PickedUp(this);
        carriedPickup = intersectedPickup;
        intersectedPickup = null;

        //parent and carry animation
        carriedPickup.transform.SetParent(this.transform, true);
        carriedPickup.transform.localPosition = new Vector2(0f, 2f);

        speed = originalSpeed * speedReductionWhenCarrying;
    }

    private void DropPickup()
    {
        //drop what it has in its hands
        carriedPickup.DroppedDown();

        //release the gameobject
        carriedPickup.transform.SetParent(null, true);

        speed = originalSpeed; //restore full speed
        carriedPickup = null;
    }


    //--------------------- Secondary actions: dashing and dodging

    public void PerformSecondary()
    {
        if(inputVector.sqrMagnitude >= dashDodgeSpeedTreshold)
        {
            switch(charState)
            {
                case CharacterState.Walking:
                    if(carriedPickup == null)
                    {
                        Dash();
                    }
                    else
                    {
                        Dodge();
                    }
                    break;
            }
        }
    }
    
    public void Dash()
    {
        StartCoroutine(EndDash());
    }

    private IEnumerator EndDash()
    {
        charState = CharacterState.Dashing;
        animator.SetTrigger(hashedDash);
        rigidbody2D.AddForce(inputVector.normalized * dashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(endDashDelay);

        GoToWalkingState();
    }

    public void Dodge()
    {
        StartCoroutine(EndDodge());
    }

    private IEnumerator EndDodge()
    {
        charState = CharacterState.Dodging;
        //animator.SetTrigger(hashedDash); //TODO: needs to be dodge
        rigidbody2D.AddForce(inputVector.normalized * dodgeSpeed, ForceMode2D.Impulse);
        gameObject.layer = LayerMask.NameToLayer("DodgingCharacters");

        yield return new WaitForSeconds(endDodgeDelay);

        gameObject.layer = LayerMask.NameToLayer("Characters");
        GoToWalkingState();
    }

    private void GoToWalkingState()
    {
        if(charState == CharacterState.Dodging || charState == CharacterState.Dashing)
            animator.SetTrigger(hashedEndSpecial);

        charState = CharacterState.Walking;
        rigidbody2D.velocity = Vector2.zero;
    }

    //--------------------- Collision detection
    private void OnCollisionEnter2D(Collision2D coll)
    {
        //Dashing into other players
        if(charState == CharacterState.Dashing)
        {
            if(coll.gameObject.CompareTag("Player"))
            {
                //Collision happened
                Character otherCharacter = coll.gameObject.GetComponent<Character>();
                rigidbody2D.velocity = Vector2.zero;
                DashLanded.Invoke(inputVector * dashPushStrength, otherCharacter); //notifies the ActionController
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //Any pickup is in the vicinity
        if(coll.gameObject.CompareTag("Pickup"))
        {
            intersectedPickup = coll.transform.parent.GetComponent<PickupObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        //A pickup left on the ground
        if(coll.gameObject.CompareTag("Pickup"))
        {
            if(coll.transform.parent.GetComponent<PickupObject>() == intersectedPickup)
            {
                intersectedPickup = null;
            }
        }
    }

     public void OnHit(Vector2 push)
    {
        charState = CharacterState.Dizzy;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(push, ForceMode2D.Impulse);

        //Drop object if carrying one
        if(carriedPickup != null)
        {
            DropPickup();
        }
        
        StartCoroutine(EndDizzy());
    }

    private IEnumerator EndDizzy()
    {
        yield return new WaitForSeconds(endDizzyDelay);

        GoToWalkingState();
    }

    //--------------------- 

    private void FixedUpdate()
    {
        switch(charState)
        {
            case CharacterState.Walking:
                //Normal reduction of speed
                // if(inputVector.sqrMagnitude <= walkDecayTreshold)
                // {
                //     rigidbody2D.velocity *= walkDecayAmount;
                // }
                break;
        }
    }

}
