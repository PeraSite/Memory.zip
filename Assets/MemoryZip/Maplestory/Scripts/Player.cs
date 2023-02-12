using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 10f;
	public float jumpForce = 500f;
	public int hp;
	public float jumpCheckradius;
	public Transform jumpCheck;
	public SpriteRenderer sr;
	private Rigidbody2D rb;

	public bool isMove;
	public bool isJump;

	void Start()
	{
		hp = 3;
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		Jump();
		if (hp <= 0)
		{
			GameManager.Instance.Failure();
		}
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		float horizontal = Input.GetAxis("Horizontal");
		if(horizontal != 0)
        {
			isMove = true;
			if (horizontal < 0)
			{
				sr.flipX = false;
			}
            else if(horizontal > 0)
            {
				sr.flipX = true;
            }
		}
        else
        {
			isMove = false;
        }
		
		rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
	}

	private void Jump()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(jumpCheck.position, jumpCheckradius, 8);
		Debug.Log(colliders.Length);
		if (colliders.Length > 0)
        {
			isJump = false;
		}
        else
        {
			isJump = true;
		}
			

		if (Input.GetButtonDown("Jump") && !isJump)
		{

			rb.AddForce(new Vector2(0, jumpForce));
		}
	}

    private void OnDrawGizmos()
    {
		Gizmos.DrawSphere(jumpCheck.position, jumpCheckradius);
    }
}
