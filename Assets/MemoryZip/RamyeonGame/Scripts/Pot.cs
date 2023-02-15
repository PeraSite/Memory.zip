using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace MemoryZip.RamyeonGame {
	public class Pot : SerializedMonoBehaviour, IInteractable {
		[Header("오브젝트")]
		[SerializeField] private Inventory _inventory;
		[SerializeField] private SpriteRenderer _animationSprite;
		[SerializeField] private Animator _playerAnimator;
		[SerializeField] private Timer _timer;

		[Header("아이템")]
		[SerializeField] private List<GameObject> _items;
		[OdinSerialize] private Dictionary<ItemType, Sprite> _putAnimation = new();
		[OdinSerialize] private Dictionary<ItemType, GameObject> _putIngredients;

		[SerializeField] private GameObject _boilingWater;
		[SerializeField] private GameObject _boilingSoup;


		[Header("설정")]
		[SerializeField] private float _showTime = 1f;
		[SerializeField] private int _endingIndex = 5;
		[SerializeField] private float _boilingTime = 5f;

		private bool _hasSoup;
		private bool _boiling;
		private int _showItemIndex;
		private static readonly int Put = Animator.StringToHash("put");

		private void Awake() {
			UpdateItemActiveState();
			_putIngredients.Values.ForEach(x => x.SetActive(false));
		}

		public void Interact() {
			if (_inventory.Holding == ItemType.없음) {
				Debug.LogWarning("Can't put item if have none");
				return;
			}
			var item = _inventory.Holding;

			// 아이템 넣기
			StartCoroutine(PutItem(item));
		}

		private IEnumerator PutItem(ItemType type) {
			var sprite = _putAnimation.GetValueOrDefault(type, null);

			// 애니메이션 표시
			if (!sprite.SafeIsUnityNull()) {
				_animationSprite.gameObject.SetActive(true);
				_animationSprite.sprite = sprite;
				_playerAnimator.SetBool(Put, true);
				yield return new WaitForSeconds(_showTime);
				_animationSprite.gameObject.SetActive(false);
				_playerAnimator.SetBool(Put, false);
			}

			// 라면 그릇 안의 재료 표시
			_putIngredients[type].SetActive(true);

			// 물을 넣으면 끓는 물 표시
			if (type == ItemType.스프) {
				_hasSoup = true;
				_putIngredients[ItemType.물].SetActive(false);
				_boilingWater.SetActive(false);
				if (_boiling) _boilingSoup.SetActive(true);
			}

			if (type == ItemType.물) {
				StartCoroutine(MakeBoil());
			}

			// 게임 내 주울 수 있는 아이템 업데이트
			_showItemIndex++;
			UpdateItemActiveState();

			// 아이템, 타이머 초기화
			_inventory.Holding = ItemType.없음;
			_timer.ResetTimer();

			// 게임 종료 체크
			if (_showItemIndex >= _endingIndex) {
				StartCoroutine(GameManager.Instance.Success());
			}
		}

		private IEnumerator MakeBoil() {
			yield return new WaitForSeconds(_boilingTime);
			(_hasSoup ? _boilingSoup : _boilingWater).SetActive(true);
			_boiling = true;
		}

		private void UpdateItemActiveState() {
			_items.ForEach((item, i) => item.SetActive(i == _showItemIndex));
		}
	}
}
