using UnityEngine;

namespace MemoryZip.RamyeonGame {
	public class Pickable : MonoBehaviour, IInteractable{
		public void Interact() {
			Debug.Log($"pickedup {name}");
		}
	}
}
