using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WheelGame.Content;

namespace WheelGame.UI {
	public class WheelDisplay : MonoBehaviour {
		[Header("References")]
		[SerializeField]
		private Transform _rotatorRoot;
		[SerializeField]
		private Transform _sectorMount;
		[SerializeField]
		private SectorDisplay _sectorPrefab;

		[Header("Animation")]
		[Tooltip("How long the wheel will spin, in seconds")]
		public float spinTimeSec = 1f;
		[Tooltip("How many full rotations the wheel should do before stopping")]
		public int extraRotations = 0;
		[Range(0f, .99f), Tooltip("How far from the center of the sector the wheel is allowed to stop, where 1 is the edge of the sector")]
		public float endRotationVariance = .5f;
		[Tooltip("The easing curve for the spin animation, where 0 is the starting rotation and 1 is the ending rotation")]
		public AnimationCurve speedCurve;


		private List<SectorDisplay> _sectors = new List<SectorDisplay>();
		private BonusPool _bonusPool;
		private float _rotationIntervalDeg;
		private Coroutine _spinRoutine = null;

		public UnityEvent onSpinComplete { get; } = new UnityEvent();


		private void Start() {
			//Start the wheel in a random orientation
			_rotatorRoot.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f));
		}

		public void SetData(BonusPool data) {
			int i = 0;
			int existingCount = _sectors.Count;
			int finalCount = data.outcomes.Length;

			_rotationIntervalDeg = 360f / finalCount;

			//Reuse existing sectors if possible
			while (i < existingCount && i < finalCount) {
				SectorDisplay existing = _sectors[i];
				existing.transform.localRotation = Quaternion.Euler(0f, 0f, i * -_rotationIntervalDeg);
				existing.SetOutcome(data.outcomes[i]);
				++i;
			}

			if (existingCount < finalCount) {
				//Create new sectors as needed
				while (i < finalCount) {
					SectorDisplay added = Instantiate(_sectorPrefab, _sectorMount, false);
					added.transform.localRotation = Quaternion.Euler(0f, 0f, i * -_rotationIntervalDeg);
					added.SetOutcome(data.outcomes[i]);
					_sectors.Add(added);
					++i;
				}
			} else {
				//Remove excess sectors
				while (i < existingCount) {
					Destroy(_sectors[i].gameObject);
					++i;
				}
				_sectors.RemoveRange(finalCount, existingCount - finalCount);
			}
		}

		public void StartSpinning(int targetIndex) {
			StopSpinning();

			_spinRoutine = StartCoroutine(SpinRoutine(targetIndex));
		}

		public void StopSpinning() {
			if (_spinRoutine != null) {
				StopCoroutine(_spinRoutine);
				_spinRoutine = null;
			}
		}

		private IEnumerator SpinRoutine(int targetIndex) {
			Quaternion startRotation = _rotatorRoot.localRotation;

			//Normalize the start angle within 0 <= x < 360 for consistent comparison
			float startAngleDeg = startRotation.eulerAngles.z;
			while (startAngleDeg < 0f)
				startAngleDeg += 360f;
			while (startAngleDeg >= 360f)
				startAngleDeg -= 360f;

			float endAngleDeg = targetIndex * _rotationIntervalDeg;

			//Apply random variance
			if (endRotationVariance > 0f) {
				endAngleDeg += (Random.value - .5f) * endRotationVariance * _rotationIntervalDeg;
			}

			//Add extra rotations (clockwise)
			if (endAngleDeg >= startAngleDeg) {
				endAngleDeg -= 360f;
			}
			endAngleDeg -= 360f * extraRotations;

			//Animate
			float elapsed = 0f;
			Vector3 euler = Vector3.zero;
			do {
				yield return null;
				elapsed += Time.deltaTime;
				euler.z = Mathf.Lerp(startAngleDeg, endAngleDeg, speedCurve.Evaluate(elapsed / spinTimeSec));
				_rotatorRoot.localRotation = Quaternion.Euler(euler);
			} while (elapsed < spinTimeSec);

			_rotatorRoot.localRotation = Quaternion.Euler(0f, 0f, endAngleDeg);

			_spinRoutine = null;
			onSpinComplete.Invoke();
		}
	}
}
