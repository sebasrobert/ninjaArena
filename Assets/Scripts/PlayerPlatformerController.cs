﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    [SerializeField]
	private float maxSpeed = 7;

    [SerializeField]
	private float jumpTakeOffSpeed = 7;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

    private bool doubleJump = true;

	// Use this for initialization
	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	protected override void ComputeCustomAction()
	{
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("throw");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("attack");
        }

        if (Input.GetButtonDown("Fire3"))
        {
            animator.SetBool("crouch", true);
        } 
        else if (Input.GetButtonUp("Fire3"))
        {
            animator.SetBool("crouch", false);
        }
	}

	protected override void ComputeVelocity()
	{
        if (grounded) {
            doubleJump = true;
        }

		Vector2 move = Vector2.zero;

		move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && (grounded || doubleJump))
		{
            if (!grounded) {
                doubleJump = false;
            }

			velocity.y = jumpTakeOffSpeed;
		}
		else if (Input.GetButtonUp("Jump"))
		{
			if (velocity.y > 0)
			{
				velocity.y = velocity.y * 0.5f;
			}
		}

		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
		if (flipSprite)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);

		targetVelocity = move * maxSpeed;
	}
}