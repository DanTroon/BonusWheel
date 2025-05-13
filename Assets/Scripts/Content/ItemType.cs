using UnityEngine;

namespace WheelGame.Content {
	[CreateAssetMenu(fileName = "ItemType", menuName = "Scriptable Objects/ItemType")]
	public class ItemType : ScriptableObject {
		[SerializeField] protected string _id;
		[SerializeField] protected Sprite _icon;

		public string id => _id;
		public Sprite icon => _icon;
	}
}
