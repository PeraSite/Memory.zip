using UnityEngine;
using UnityEngine.Tilemaps;

public class Minecraft_Player : MonoBehaviour 
{
	public float speed = 10f;
	public float jumpForce = 500f;
	public int hp;
	public int damage;
	public int damageCount;
	public float range;
	public SpriteRenderer Render;
	private Rigidbody2D rb;

	[Header("Ray")]
	public RaycastHit2D hit;
	public float rayLength = 100f;
	public LayerMask layerMask;
	private Transform tf;
	private Vector2 rayDir;

	[Header("Tile")]
	public Grid grid;
	public Tilemap tilemap;
	public Vector3 hitTile;
	public Vector3Int tilePos;

	[Header("Sound")]
	public AudioSource AS1;
	public AudioSource AS2;
	public AudioClip waterSound;
	public AudioClip lavaSound;
	public AudioClip digSound;
	
	

	void Start() {
		hp = 3;
		tf = transform;
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		Attack();

        if (hp <= 0)
        {
			GameManager.Instance.Failure();
        }
	}

	private void FixedUpdate() {
		Move();
	}

	private void Move() {
		float horizontal = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
	}




	private void Attack() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		rayDir = tf.right * h + tf.up * v;

		Debug.DrawRay(tf.position, rayDir.normalized * rayLength, Color.red);

		hit = Physics2D.Raycast(tf.position, rayDir.normalized, rayLength, layerMask);
		Collider2D hitcol2d = Physics2D.OverlapCircle(hit.point, 0.1f, layerMask);
		if (hitcol2d != null) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				AS2.clip = digSound;
				AS2.Play();
				hitTile = hit.point - hit.normal / 2;
				tilePos = grid.WorldToCell(hitTile); 
				tilemap.SetTile(tilePos, null);
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Lava")
		{
			Debug.Log("Lava");
			StartCoroutine(GameManager.Instance.Failure());
		}

		if(collision.gameObject.tag == "Goal")
        {
			Debug.Log("Success");
			StartCoroutine(GameManager.Instance.Success());
		}

		if (collision.gameObject.tag == "WaterSound")
		{
			AS1.clip = waterSound;
			AS1.Play();
		}

		if (collision.gameObject.tag == "LavaSound")
		{
			AS1.clip = lavaSound;
			AS1.Play();
		}
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
		//if (collision.gameObject.tag == "WaterSound")
		//{
		//	AS1.clip = waterSound;
		//	AS1.Play();
		//}

		//if (collision.gameObject.tag == "LavaSound")
		//{
		//	AS1.clip = lavaSound;
		//	AS1.Play();
		//}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "WaterSound")
		{
			AS1.Stop();
		}

		if (collision.gameObject.tag == "LavaSound")
		{
			AS1.Stop();
		}
	}

    private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(hit.point, 0.1f);
	}
}
