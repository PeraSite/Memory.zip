using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaplePlayer : MonoBehaviour
{
	public float speed = 10f;
	public float jumpForce = 500f;
	public float knockBackForce;
	public int hp;
	public Animator anim;
	public SpriteRenderer sr;
	private Rigidbody2D rb;
	public float horizontal;
	public bool isMove;
	public bool isJump;

	void Start()
	{
		hp = 3;
		anim = GetComponent<Animator>();
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

        if (isJump)
        {
			anim.SetBool("isJump", true);
        }
        else
        {
			anim.SetBool("isJump", false);
		}
		if (isMove)
		{
			anim.SetBool("isMove", true);
		}
		else
		{
			anim.SetBool("isMove", false);
		}

	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		horizontal = Input.GetAxis("Horizontal");
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
		if (Input.GetButtonDown("Jump") && !isJump)
		{

			rb.AddForce(new Vector2(0, jumpForce));
		}
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
		if(collision.gameObject.layer == 8)
			isJump = false;

		if(collision.gameObject.tag == "Monster")
        {
			Debug.Log("monster attack");
			StartCoroutine(GameManager.Instance.Failure());
		}
		if (collision.gameObject.tag == "Goal")
		{
			Debug.Log("Success");
			StartCoroutine(GameManager.Instance.Success());
		}
	}
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.layer == 8)
			isJump = true;
	}
}
