using UnityEngine;
using UnityEngine.UI;
using WheelGame.Content;

namespace WheelGame.UI {
	[RequireComponent(typeof(Animator))]
	public class ResultDisplay : MonoBehaviour {
		private const string ANIM_PARAM_OPEN = "Active";

		[SerializeField]
		private SectorDisplay _itemDisplay;
		[SerializeField]
		private Button _claimButton;

		private Animator _animator;

		public Button claimButton => _claimButton;

		public bool isOpen {
			get { return _animator.GetBool(ANIM_PARAM_OPEN); }
			set { _animator.SetBool(ANIM_PARAM_OPEN, value); }
		}

		private void Awake() {
			_animator = GetComponent<Animator>();
		}

		public void SetOutcome(BonusPool.BonusPoolOutcome outcome) {
			_itemDisplay.SetOutcome(outcome);
		}
	}
}
