using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    public float range;
    public LayerMask hitLayers;
    public GameObject water;
    public GameObject ball;


    private void Start()
    {
        Boom();
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right * range, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * range, Color.red);
        Debug.DrawRay(transform.position, Vector2.up * range, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * range, Color.red);
    }
    public void Boom()
    {
        StartCoroutine(BoomDelay());
    }

    public void ShootRay(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, hitLayers);
        
        if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            if(hit.collider.gameObject.tag == "Boss")
            {
                Boss hitEnemy = hit.transform.GetComponent<Boss>();
                if (hitEnemy != null && !hitEnemy.isnodamTime)
                {
                    hitEnemy.TakeDamage();
                }
            }
            if (hit.collider.gameObject.tag == "Monster")
            {
                Monster hitEnemy = hit.transform.GetComponent<Monster>();
                if (hitEnemy != null)
                {
                    hitEnemy.TakeDamage();
                }
            }

        }
    }

    IEnumerator BoomDelay()
    {
        yield return new WaitForSeconds(1.5f);
        ball.SetActive(false);
        water.SetActive(true);
        ShootRay(Vector2.right);
        ShootRay(Vector2.left);
        ShootRay(Vector2.up);
        ShootRay(Vector2.down);
        yield return new WaitForSeconds(1f);
        water.SetActive(false);
        Destroy(gameObject);
        CrazyArcade_Player.instance.AttackDelay = false;
    }
}
