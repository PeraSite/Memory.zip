using UnityEngine;

namespace MemoryZip.RamyeonGame {
	public class Pickable : MonoBehaviour, IInteractable {
		[SerializeField] private Inventory _inventory;
		[SerializeField] private ItemType _itemType;
		[SerializeField] private AudioClip _pickupSound;

		public void Interact() {
			if (_inventory.Holding != ItemType.없음) {
				Debug.LogWarning("Can't pickup if already has one");
				return;
			}

			_inventory.Holding = _itemType;
			gameObject.SetActive(false);
			AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
		}
	}
}
