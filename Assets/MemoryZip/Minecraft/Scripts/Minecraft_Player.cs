using UnityEngine;
using UnityEngine.Tilemaps;

public class Minecraft_Player : MonoBehaviour 
{
	public enum Tool { hand, wood, stone, iron, gold, diamond }

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

	[Header("Inventory")]
	public Tool handTool = Tool.hand;
	
	

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
				hitTile = hit.point - hit.normal / 2;
				tilePos = grid.WorldToCell(hitTile); 
				tilemap.SetTile(tilePos, null);
			}
		}
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
			hp--;
        }
    }

    private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(hit.point, 0.1f);
	}
}
