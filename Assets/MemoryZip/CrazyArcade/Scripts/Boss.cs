using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private int B_Hp;
    public float moveSpeed = 3f;
    public int moveDirection = 0;

    public float nodamTime = 1.5f;
    public SpriteRenderer BossRender;
    public bool isnodamTime;

    private Rigidbody2D rb;

    void Start()
    {
        BossRender = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Delay());
        B_Hp = 10;
    }

    void Update()
    {
        DeathCheck();
    }

    private void DeathCheck()
    {
        if (B_Hp <= 0)
        {
            Debug.Log("Death");
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        Debug.Log("hit");
        if(B_Hp > 0)
        {
            StartCoroutine(UnBeatTime());
            Debug.Log("Damaged");
            rb.velocity = Vector3.zero;
            B_Hp -= 1;
            Debug.Log(B_Hp);
        }
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

    IEnumerator UnBeatTime()
    {
        isnodamTime = true;
        for (int i = 0; i < nodamTime * 10; ++i)
        {
            if (i % 2 == 0)
                BossRender.color = new Color32(255, 255, 255, 90);
            else
                BossRender.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.1f);
        }

        //Alpha Effect End
        BossRender.color = new Color32(255, 255, 255, 255);

        isnodamTime = false;

        yield return null;
    }
}
