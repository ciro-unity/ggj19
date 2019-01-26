using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
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

    [Header("Movement configuration")]
    public float speed = 200f;
    [SerializeField]
    private float walkDecayTreshold = .7f, walkDecayAmount = .5f, dashSpeed = 200f, dodgeSpeed = 100f, dashDodgeSpeedTreshold = .2f;

    private Rigidbody2D rigidbody2D;
    private Vector2 inputVector;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        rigidbody2D.AddForce(inputVector * dashSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.3f);

        charState = CharacterState.Walking;
    }

    public void Dodge()
    {
        StartCoroutine(EndDodge());
    }

    private IEnumerator EndDodge()
    {
        charState = CharacterState.Dodging;
        rigidbody2D.AddForce(inputVector * dodgeSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.2f);

        charState = CharacterState.Walking;
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
