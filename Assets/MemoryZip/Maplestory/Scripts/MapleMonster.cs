using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleMonster : MonoBehaviour
{
    public float speed = 2f;
    public float changeDirectionTime = 2f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float direction = 1f;
    private float changeDirectionCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        changeDirectionCounter = changeDirectionTime;
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(speed * direction * Time.deltaTime, 0, 0);
        changeDirectionCounter -= Time.deltaTime;

        if (changeDirectionCounter < 0)
        {
            direction *= -1;
            changeDirectionCounter = changeDirectionTime;
        }
        if(direction > 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
