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
    [Header("anim")]
    public Animator anim;
    public bool isMove;
    public bool isBack;
    public bool isRight;
    public bool isLeft;

    public bool isnodamTime;

    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        BossRender = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Delay());
        B_Hp = 10;
    }

    void Update()
    {
        DeathCheck();
        if (isMove)
        {
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
        if (isRight)
        {
            anim.SetBool("isRight", true);
        }
        else
        {
            anim.SetBool("isRight", false);
        }
        if (isLeft)
        {
            anim.SetBool("isLeft", true);
        }
        else
        {
            anim.SetBool("isLeft", false);
        }
        if (isBack)
        {
            anim.SetBool("isBack", true);
        }
        else
        {
            anim.SetBool("isBack", false);
        }

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
            if(B_Hp == 0)
            {
                anim.SetTrigger("Die");
                GameManager.Instance.Success();
            }
        }
    }

    void Move()
    {
        if (moveDirection == 0)
        {
            isMove = false;
            isRight = false;
            isLeft = false;
            isBack = false;
            rb.velocity = Vector3.zero;
        }
        else if (moveDirection == 1)
        {
            isMove = true;
            isRight = false;
            isLeft = false;
            isBack = true;
            rb.velocity = new Vector2(0, moveSpeed);
        }
        else if (moveDirection == 2)
        {
            isMove = true;
            isRight = true;
            isLeft = false;
            isBack = false;
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else if (moveDirection == 3)
        {
            isMove = true;
            isRight = false;
            isLeft = false;
            isBack = false;
            rb.velocity = new Vector2(0, -moveSpeed);
        }
        else if (moveDirection == 4)
        {
            isMove = true;
            isRight = false;
            isLeft = true;
            isBack = false;
            rb.velocity = new Vector2(-moveSpeed, 0);
        }
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero;
        isMove = false;
        isRight = false;
        isLeft = false;
        isBack = false;
        yield return new WaitForSeconds(1f);
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
