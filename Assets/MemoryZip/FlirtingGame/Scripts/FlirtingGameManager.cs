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

		[Header("UI")]
		[SerializeField] private TextMeshProUGUI _currentRoundText;
		[SerializeField] private TextMeshProUGUI _completePercentText;
		[SerializeField] private Image _completePercentBar;

		[Header("주인공 애니메이션")]
		[SerializeField] private Animator _playerAnimator;
		[SerializeField] private GameObject _playerAttackAnimation;

		[Header("고양이 오브젝트")]
		[SerializeField] private GameObject[] _maleCats;
		[SerializeField] private GameObject[] _rivalCats;

		[Tooltip("현재 라운드 -> 보여질 수컷 고양이 오브젝트")]
		[OdinSerialize] private Dictionary<int, GameObject> _maleCatDictionary = new();

		[Tooltip("현재 라운드 -> 보여질 암컷 고양이 오브젝트 목록")]
		[OdinSerialize] private Dictionary<int, List<GameObject>> _rivalCatDictionary = new();

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
		private float _timer;
		private static readonly int IsAttack = Animator.StringToHash("IsAttack");

		private void Start() {
			CompletePercent = _defaultPercent;
			UpdateUI();
			UpdateCats();
		}

		private void Update() {
			HandleAttackInput();
			HandlePercentDecrease();
		}

		private void HandleAttackInput() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				// TODO: 공격 레이저 표시
				CompletePercent += _percentPerSpace;
				if (CompletePercent >= 100) {
					NextRound();
				}
			}

			var isAttacking = Input.GetKey(KeyCode.Space);
			_playerAnimator.SetBool(IsAttack, isAttacking);
			_playerAttackAnimation.SetActive(isAttacking);
		}

		private void HandlePercentDecrease() {
			CompletePercent = Mathf.Max(CompletePercent - _rivalCatDictionary[CurrentRound].Count * _percentPerRival * Time.deltaTime, 0);
		}

		[Title("유틸")]
		[Button]
		private void NextRound() {
			CurrentRound++;
			if (CurrentRound > _maxRound) {
				Debug.Log("Game Clear!");
				return;
			}
			CompletePercent = _defaultPercent;
			UpdateCats();
		}

		[ButtonGroup("UI")]
		private void UpdateUI() {
			_currentRoundText.text = $"{CurrentRound}라운드";
			_completePercentText.text = $"{(int)CompletePercent}";
			_completePercentBar.fillAmount = CompletePercent / 100;
		}

		[ButtonGroup("UI")]
		private void UpdateCats() {
			_maleCats.ForEach(x => x.SetActive(_maleCatDictionary[CurrentRound] == x));
			_rivalCats.ForEach(x => x.SetActive(_rivalCatDictionary[CurrentRound].Contains(x)));
		}

		private void CheckGameOver() {
			if (CompletePercent <= 0) {
				Debug.Log("Game Over!");
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
}
