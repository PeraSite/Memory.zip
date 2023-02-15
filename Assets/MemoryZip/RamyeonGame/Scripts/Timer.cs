using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryZip.RamyeonGame {
	public class Timer : MonoBehaviour {
		[SerializeField] private float _timeForItem = 15f;
		[SerializeField] private Image _gauge;

		private float _leftTime;
		public float LeftTime {
			get => _leftTime;
			set {
				_leftTime = value;
				UpdateUI();
			}
		}

		private void Start() {
			ResetTimer();
		}

		private void Update() {
			LeftTime -= Time.deltaTime;

			if (LeftTime <= 0) {
				StartCoroutine(GameManager.Instance.Failure());
			}
		}

		public void ResetTimer() {
			LeftTime = _timeForItem;
		}

		private void UpdateUI() {
			_gauge.fillAmount = LeftTime / 15f;
		}
	}
}
