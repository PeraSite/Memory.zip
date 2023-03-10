using Sirenix.Utilities;
using UnityEngine;

namespace MemoryZip.RamyeonGame {
	public class RamyeonGameCharacter : MonoBehaviour {
		[Header("설정")]
		[SerializeField] private float _moveSpeed;
		[SerializeField] private float _interactDistance;

		private Animator _animator;
		private Rigidbody2D _rigidbody;
		private SpriteRenderer _spriteRenderer;
		private Vector2 _input;
		private static readonly int IsMoving = Animator.StringToHash("isMoving");
		private static readonly int Put = Animator.StringToHash("put");

		private void Awake() {
			_animator = GetComponent<Animator>();
			_rigidbody = GetComponent<Rigidbody2D>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Update() {
			ReadInput();
		}

		private void ReadInput() {
			_input.x = Input.GetAxisRaw("Horizontal");
			_input.y = Input.GetAxisRaw("Vertical");

			if (Input.GetKeyDown(KeyCode.Space)) {
				Interact();
			}
		}

		private void FixedUpdate() {
			Move();
		}

		private void Move() {
			// 넣는 중이면 움직이지 못함
			if (_animator.GetBool(Put)) {
				_rigidbody.velocity = Vector2.zero;
				return;
			}
			
			_rigidbody.velocity = _input.normalized * _moveSpeed;
			_animator.SetBool(IsMoving, _input.magnitude > 0);

			if (_input.x != 0) {
				_spriteRenderer.flipX = _input.x > 0;
			}
		}

		private void Interact() {
			Collider2D col = Physics2D.OverlapCircle(transform.position, _interactDistance, LayerMask.GetMask("Interactable"));
			if (col.SafeIsUnityNull()) return;

			IInteractable interactable = col.GetComponent<IInteractable>();
			interactable?.Interact();
		}
	}
}
