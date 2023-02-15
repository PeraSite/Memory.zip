using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MemoryZip.FlirtingGame {
	public class FlirtingGameManager : SerializedMonoBehaviour {
		[Header("설정"), SuffixLabel("초", Overlay = true)]
		[SerializeField] private int _maxRound = 3;
		[SerializeField] private float _defaultPercent = 30;

		[SerializeField] private float _percentPerRival = 1;
		[SerializeField] private float _percentPerSpace = 5;
		[SerializeField] private float _gameOverDelay = 5f;

		[Header("UI")]
		[SerializeField] private TextMeshProUGUI _currentRoundText;
		[SerializeField] private Image _completePercentBar;
		[SerializeField] private GameObject _gameOverObject;


		[Header("주인공 애니메이션")]
		[SerializeField] private Animator _playerAnimator;
		[SerializeField] private GameObject _playerAttackAnimation;

		[Header("수컷 고양이 스프라이트")]
		[SerializeField] private Sprite[] _damagedMaleCatSprites;

		[Header("고양이 오브젝트")]
		[SerializeField] private GameObject[] _maleCats;
		[SerializeField] private GameObject[] _rivalCats;

		[Tooltip("현재 라운드 -> 보여질 수컷 고양이 오브젝트")]
		[OdinSerialize] private Dictionary<int, GameObject> _maleCatDictionary = new();

		[Tooltip("현재 라운드 -> 보여질 암컷 고양이 오브젝트 목록")]
		[OdinSerialize] private Dictionary<int, List<GameObject>> _rivalCatDictionary = new();

		[Header("효과음")]
		[SerializeField] private AudioSource _attackSound;
		[SerializeField] private AudioClip _clearSound;


		private bool _isGameOver;
		private int _currentRound = 1;
		private float _completePercent;
		private int CurrentRound {
			get => _currentRound;
			set {
				_currentRound = value;
				UpdateUI();
			}
		}
		private float CompletePercent {
			get => _completePercent;
			set {
				_completePercent = value;
				UpdateUI();
				CheckGameOver();
			}
		}
		private Sprite _maleCatOriginalSprite;

		private float _timer;
		private static readonly int IsAttack = Animator.StringToHash("IsAttack");
		private static readonly int IsDead = Animator.StringToHash("IsDead");

		private void Start() {
			CompletePercent = _defaultPercent;
			UpdateUI();
			UpdateCats();
		}

		private void Update() {
			if (_isGameOver) return;
			HandleAttackInput();
			HandlePercentDecrease();
		}

		private void HandleAttackInput() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				_attackSound.time = 0f;

				// 함락 게이지 상승
				CompletePercent += _percentPerSpace;

				// 피격 애니메이션 표기
				_maleCatDictionary[CurrentRound].GetComponent<SpriteRenderer>().sprite = _damagedMaleCatSprites[Random.Range(0, _damagedMaleCatSprites.Length)];
				
				// 다음 스테이지 체크
				if (CompletePercent >= 100) {
					NextRound();
				}
			}

			if (Input.GetKeyUp(KeyCode.Space)) {
				// 피격 애니메이션 초기화
				_maleCatDictionary[CurrentRound].GetComponent<SpriteRenderer>().sprite = _maleCatOriginalSprite;
			}


			var isAttacking = Input.GetKey(KeyCode.Space);
			_playerAnimator.SetBool(IsAttack, isAttacking);
			_playerAttackAnimation.SetActive(isAttacking);
			_attackSound.volume = isAttacking ? 1 : 0;
		}

		private void HandlePercentDecrease() {
			CompletePercent = Mathf.Max(CompletePercent - _rivalCatDictionary[CurrentRound].Count * _percentPerRival * Time.deltaTime, 0);
		}

		[Title("유틸")]
		[Button]
		private void NextRound() {
			AudioSource.PlayClipAtPoint(_clearSound, Vector3.zero);
			CurrentRound++;
			if (CurrentRound > _maxRound) {
				_isGameOver = true;
				Debug.Log("Game Clear!");
				return;
			}
			CompletePercent = _defaultPercent;
			UpdateCats();
		}

		[ButtonGroup("UI")]
		private void UpdateUI() {
			_currentRoundText.text = $"{CurrentRound}라운드";
			_completePercentBar.fillAmount = CompletePercent / 100;
		}

		[ButtonGroup("UI")]
		private void UpdateCats() {
			_maleCatOriginalSprite = _maleCatDictionary[CurrentRound].GetComponent<SpriteRenderer>().sprite;
			_maleCats.ForEach(x => x.SetActive(_maleCatDictionary[CurrentRound] == x));
			_rivalCats.ForEach(x => x.SetActive(_rivalCatDictionary[CurrentRound].Contains(x)));
		}

		private void CheckGameOver() {
			if (CompletePercent <= 0) {
				_isGameOver = true;
				StartCoroutine(GameOver());
			}
		}

		private IEnumerator GameOver() {
			_playerAnimator.SetBool(IsDead, true);
			_gameOverObject.SetActive(true);
			_maleCats.ForEach(x => x.SetActive(false));
			_rivalCats.ForEach(x => x.SetActive(false));

			yield return new WaitForSeconds(_gameOverDelay);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
