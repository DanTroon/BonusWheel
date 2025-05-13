using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WheelGame.Content;

namespace WheelGame.UI {
	public class BonusScreen : MonoBehaviour {
		private const string SPIN_BUTTON_ACTIVE_PARAM = "Active";

		[Header("UI")]
		[SerializeField]
		private WheelDisplay _wheel;
		[SerializeField]
		private ResultDisplay _resultPanel;
		[SerializeField]
		private Animator _spinButtonAnim;
		[SerializeField]
		private Button _spinButton;

		[Header("Content")]
		[SerializeField]
		private BonusPool _bonusPool;

		private int _selectedOutcomeIndex = -1;

		public class ClaimEvent : UnityEvent<ItemType, int> { }
		public ClaimEvent onClaim { get; } = new ClaimEvent();

		private bool spinButtonEnabled {
			get { return _spinButton.interactable; }
			set {
				_spinButton.interactable = value;
				_spinButtonAnim.SetBool(SPIN_BUTTON_ACTIVE_PARAM, value);
			}
		}


		private void Start() {
			_wheel.SetData(_bonusPool);

			_spinButton.onClick.AddListener(SpinButton_OnClick);
			_wheel.onSpinComplete.AddListener(Wheel_OnSpinComplete);
			_resultPanel.claimButton.onClick.AddListener(ResultClaimButton_OnClick);

			spinButtonEnabled = true;
		}

		private void SpinWheel() {
			_selectedOutcomeIndex = _bonusPool.GetRandomIndex();

			if (_selectedOutcomeIndex != -1) {
				spinButtonEnabled = false;
				_wheel.StartSpinning(_selectedOutcomeIndex);
			}
		}

		private void ShowResult() {
			_resultPanel.SetOutcome(_bonusPool.outcomes[_selectedOutcomeIndex]);
			_resultPanel.isOpen = true;
		}

		private void SpinButton_OnClick() {
			SpinWheel();
		}

		private void Wheel_OnSpinComplete() {
			ShowResult();
		}

		private void ResultClaimButton_OnClick() {
			if (_selectedOutcomeIndex == -1)
				return;

			BonusPool.BonusPoolOutcome outcome = _bonusPool.outcomes[_selectedOutcomeIndex];
			_selectedOutcomeIndex = -1;
			_resultPanel.isOpen = false;
			spinButtonEnabled = true;

			onClaim.Invoke(outcome.item, outcome.quantity);
		}
	}
}
