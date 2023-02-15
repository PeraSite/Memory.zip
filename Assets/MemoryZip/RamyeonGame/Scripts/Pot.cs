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

		[Header("아이템")]
		[SerializeField] private List<GameObject> _items;
		[OdinSerialize] private Dictionary<ItemType, Sprite> _putAnimation = new();
		[OdinSerialize] private Dictionary<ItemType, GameObject> _putIngredients;

		[Header("설정")]
		[SerializeField] private float _showTime = 1f;
		[SerializeField] private int _endingIndex = 5;

		private int _showItemIndex;

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
				yield return new WaitForSeconds(_showTime);
				_animationSprite.gameObject.SetActive(false);
			}
			
			// 라면 그릇 안의 재료 표시
			_putIngredients[type].SetActive(true);
			if (type == ItemType.스프) {
				_putIngredients[ItemType.물].SetActive(false);
			}

			// 게임 내 주울 수 있는 아이템 업데이트
			_showItemIndex++;
			UpdateItemActiveState();

			// 인벤토리 초기화
			_inventory.Holding = ItemType.없음;

			// 게임 종료
			if (_showItemIndex >= _endingIndex) {
				StartCoroutine(GameManager.Instance.Success());
			}
		}

		private void UpdateItemActiveState() {
			_items.ForEach((item, i) => item.SetActive(i == _showItemIndex));
		}
	}
}
