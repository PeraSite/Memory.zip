using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryZip.FlirtingGame {
	public class FlirtingGameManager : MonoBehaviour {
		[Header("UI")]
		[SerializeField] private TextMeshProUGUI _currentRoundText;
		[SerializeField] private TextMeshProUGUI _healthText;
		[SerializeField] private Image _healthBar;

		private int _currentStage = 1;
		private int _health = 100;

		public int CurrentStage {
			get => _currentStage;
			set {
				_currentStage = value;
				UpdateUI();
			}
		}
		public int Health {
			get => _health;
			set {
				_health = value;
				UpdateUI();
			}
		}

		private void Start() {
			UpdateUI();
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				Health -= 5;
				if (Health <= 0) {
					NextStage();
				}
			}
		}

		private void NextStage() {
			CurrentStage++;
			Health = 100;
		}

		private void UpdateUI() {
			_currentRoundText.text = $"{CurrentStage}라운드";
			_healthText.text = $"{Health}";
			_healthBar.fillAmount = (float)Health / 100;
		}
	}
}
