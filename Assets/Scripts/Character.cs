using System.Collections;
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
        Carrying,
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

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //Called on FixedUpdate by the GameManager
    public void Move(Vector2 input)
    {
        if(charState == CharacterState.Walking)
        {
            inputVector = input;
            //rigidbody2D.AddForce(inputVector * speed * Time.fixedDeltaTime, ForceMode2D.Force);
            rigidbody2D.MovePosition(rigidbody2D.position + inputVector * speed * Time.fixedDeltaTime);
        }
    }


    //--------------------- Primary action: pickup and release
    public void PerformAction()
    {

    }


    //--------------------- Secondary actions: dashing and dodging

    public void PerformSecondary()
    {
        if(inputVector.sqrMagnitude >= dashDodgeSpeedTreshold)
        {
            switch(charState)
            {
                case CharacterState.Walking:
                    Dash();
                    break;

                case CharacterState.Carrying:
                    Dodge();
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
        rigidbody2D.AddForce(inputVector.normalized * dodgeSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(endDodgeDelay);

        GoToWalkingState();
    }

    private void GoToWalkingState()
    {
        charState = CharacterState.Walking;
        rigidbody2D.velocity = Vector2.zero;
    }

    //--------------------- Collision detection
    private void OnCollisionEnter2D(Collision2D coll)
    {
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

     public void OnHit(Vector2 push)
    {
        charState = CharacterState.Dizzy;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(push, ForceMode2D.Impulse);
        
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
