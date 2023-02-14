using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    public float range;
    public LayerMask hitLayers;
    public GameObject water;
    public GameObject ball;
    [Header("Sound")]
    public AudioSource SoundPlayer;
    public AudioClip spawnSound;
    public AudioClip boomSound;

    private void Start()
    {
        SoundPlayer = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
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
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, range, hitLayers);

        if (hit.Length != 0)
        {
            foreach (RaycastHit2D hits in hit)
            {
                Debug.Log("Hit " + hits.collider.gameObject.name);

                if (hits.collider.gameObject.tag == "Boss")
                {
                    Boss hitEnemy = hits.transform.GetComponent<Boss>();
                    if (hitEnemy != null && !hitEnemy.isnodamTime)
                    {
                        hitEnemy.TakeDamage();
                    }
                }
                if (hits.collider.gameObject.tag == "Monster")
                {
                    Monster hitEnemy = hits.transform.GetComponent<Monster>();
                    if (hitEnemy != null)
                    {
                        hitEnemy.TakeDamage();
                    }
                }
            }
            

        }
    }

    IEnumerator BoomDelay()
    {
        SoundPlayer.clip = spawnSound;
        SoundPlayer.Play();
        yield return new WaitForSeconds(1f);
        SoundPlayer.clip = boomSound;
        SoundPlayer.Play();
        yield return new WaitForSeconds(0.7f);
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
