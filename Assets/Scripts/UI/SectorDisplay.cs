using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelGame.Content;

namespace WheelGame.UI {
	public class SectorDisplay : MonoBehaviour {
		[SerializeField]
		private Image _icon;
		[SerializeField]
		private TextMeshProUGUI _quantityText;

		public void SetOutcome(BonusPool.BonusPoolOutcome outcome) {
			_icon.sprite = outcome.item.icon;

			if (outcome.quantity < 0) {
				_quantityText.text = outcome.quantity.ToString();
			} else {
				_quantityText.text = "+" + outcome.quantity.ToString();
			}
		}
	}
}
