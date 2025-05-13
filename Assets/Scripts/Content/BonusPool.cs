using System;
using UnityEngine;

namespace WheelGame.Content {
	[CreateAssetMenu(fileName = "WheelWeight", menuName = "Scriptable Objects/WheelWeight")]
	public class BonusPool : ScriptableObject {
		[Serializable]
		public class BonusPoolOutcome {
			public string id;
			public ItemType item;
			public int quantity = 1;
			public float weight = 1f;
		}

		[SerializeField]
		private BonusPoolOutcome[] _outcomes;
		public BonusPoolOutcome[] outcomes => _outcomes;

		private float _totalWeight = 0f;

		public int GetRandomIndex() {
			if (_outcomes.Length == 0)
				return -1;

			//Collect weights (first run)
			if (_totalWeight == 0f) {
				foreach (BonusPoolOutcome outcome in _outcomes) {
					_totalWeight += outcome.weight;
				}
			}

			float target = UnityEngine.Random.Range(0f, _totalWeight);

			//Linear search (every run)
			for (int i = _outcomes.Length - 1; i > 0; --i) {
				if (target <= _outcomes[i].weight) {
					return i;
				}

				target -= _outcomes[i].weight;
			}

			return 0;
		}
	}
}
