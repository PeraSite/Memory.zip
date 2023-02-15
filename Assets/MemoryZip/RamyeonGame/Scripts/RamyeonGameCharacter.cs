using System;
using UnityEngine;

namespace MemoryZip.RamyeonGame {
	public class RamyeonGameCharacter : MonoBehaviour {
		[Header("¼³Á¤")]
		[SerializeField] private float _moveSpeed;

		private Animator _animator;
		private Rigidbody2D _rigidbody;
		private SpriteRenderer _spriteRenderer;
		private Vector2 _input;
		private static readonly int IsMoving = Animator.StringToHash("isMoving");

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
		}

		private void FixedUpdate() {
			Move();
		}

		private void Move() {
			_rigidbody.velocity = _input.normalized * _moveSpeed;
			_animator.SetBool(IsMoving, _input.magnitude > 0);

			if (_input.magnitude > 0)
				_spriteRenderer.flipX = _input.x > 0;
		}
	}
}
