using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int moveDirection = 0;

    public SpriteRenderer M_Renderer;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        M_Renderer = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(Delay());
    }

    public void TakeDamage()
    {
        gameObject.SetActive(false);
    }

    void Move()
    {
        if (moveDirection == 0)
        {
            rb.velocity = Vector3.zero;
        }
        else if (moveDirection == 1)
        {
            rb.velocity = new Vector2(0, moveSpeed);
        }
        else if (moveDirection == 2)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else if (moveDirection == 3)
        {
            rb.velocity = new Vector2(0, -moveSpeed);
        }
        else if (moveDirection == 4)
        {
            rb.velocity = new Vector2(-moveSpeed, 0);
        }
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        moveDirection = Random.Range(0, 4);
        Move();
    }
}
