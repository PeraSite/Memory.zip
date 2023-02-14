using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CrazyArcade_Player : MonoBehaviour
{
    public static CrazyArcade_Player instance; 

    [SerializeField] GameObject _WaterBallPrefab;

    public float speed = 10.0f;
    [SerializeField] private int Hp = 3;

    public Animator anim;
    public SpriteRenderer CharRender;
    public float nodamTime;
    public bool isnodamTime;
    public bool AttackDelay;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CharRender = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(horizontal, vertical, 0).normalized * speed * Time.deltaTime;

        if(horizontal + vertical != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !AttackDelay)
        {
            AttackDelay = true;
            var ball = Instantiate(_WaterBallPrefab);
            ball.transform.position = transform.position;
        }
    }

    public void TakeDamage()
    {
        if (Hp != 0)
        {
            Hp -= 1;
            if(Hp == 0)
            {
                StartCoroutine(GameManager.Instance.Failure());
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            Boss boss = collision.transform.GetComponent<Boss>();
            if (!isnodamTime && !boss.isnodamTime)
            {
                StartCoroutine(UnBeatTime());
                TakeDamage();
            }
        }

        if(collision.gameObject.tag == "Monster")
        {
            if (!isnodamTime)
            {
                StartCoroutine(UnBeatTime());
                TakeDamage();
            }
        }
    }

    IEnumerator UnBeatTime()
    {
        isnodamTime = true;
        for (int i = 0; i < nodamTime * 10; ++i)
        {
            if (i % 2 == 0)
                CharRender.color = new Color32(255, 255, 255, 90);
            else
                CharRender.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.1f);
        }

        //Alpha Effect End
        CharRender.color = new Color32(255, 255, 255, 255);

        isnodamTime = false;

        yield return null;
    }
}
