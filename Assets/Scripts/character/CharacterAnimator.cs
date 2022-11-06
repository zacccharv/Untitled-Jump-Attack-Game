using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    private Rigidbody2D body;
    private Animator animator;
    private Ground ground;
    private Jump jump;
    private Move move;

    [SerializeField] private bool buttonCheck;

    private Controller _controller;
    private SpriteRenderer sr;
    private Vector2 _direction;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _controller = GetComponent<Controller>();
        ground = GetComponent<Ground>();
        jump = GetComponent<Jump>();
        move = GetComponent<Move>();

        sr = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        _direction.x = _controller.input.RetrieveMoveInput();

        if (_direction.x == -1)
        {
            transform.localScale = new Vector3(-1.79f, 1.79f, 0);
            buttonCheck = true;
            sr.flipX = true;
        }
        if (_direction.x == 1)
        {
            buttonCheck = true;
            transform.localScale = new Vector3(1.79f, 1.79f, 0);
            sr.flipX = false;
        }
        if (_direction.x == 0)
        {
            buttonCheck = false;
        }

            if (animator != null)
        {
            // idling check
            if (ground.OnGround && Mathf.Abs(body.velocity.x) < 0.1f || (move._onPlatform && buttonCheck == false))
            {
                animator.SetBool("Moving", false);
                animator.SetBool("Falling", false);
                animator.SetBool("Idling", true);
            } 

            // running check
            if (ground.OnGround && Mathf.Abs(body.velocity.x) > 0.1 && buttonCheck)
            {
                animator.SetBool("Moving", true);
                animator.SetBool("Idling", false);
                animator.SetBool("Falling", false);
            } 
            // just jumped check
            if (_controller.input.RetrieveJumpInput() && jump._jumpPhase < jump._maxAirJumps)
            {
                animator.SetBool("Moving", false);
            }

            // Floating Check
            if (!ground.OnGround && body.velocity.y > 0)
            {
                animator.SetBool("Floating", true);
            }

            //Falling Check also works if fall of ledge
            if (!ground.OnGround && body.velocity.y < 0)
            {
                animator.SetBool("Floating", false);
                animator.SetBool("Falling", true);
            }


        }
    }
}
