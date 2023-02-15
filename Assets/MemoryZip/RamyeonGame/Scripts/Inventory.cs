using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryZip.RamyeonGame {
	public class Inventory : SerializedMonoBehaviour {
		[OdinSerialize] private Dictionary<ItemType, Sprite> _icon;
		[SerializeField] private Image _iconImage;

		private ItemType _holding;

		public ItemType Holding {
			get => _holding;
			set {
				_holding = value;
				UpdateUI();
			}
		}

		private void Awake() {
			UpdateUI();
		}

		private void UpdateUI() {
			_iconImage.sprite = _icon.GetValueOrDefault(_holding, null);
		}
	}
}
