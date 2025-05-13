using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using WheelGame.Content;

public class BonusPoolTests {
	private const string POOL_ASSET_PATH = "Assets/Content/BonusPools/DefaultBonusPool.asset";

	[Test]
	public void TestPoolRandomization() {
		BonusPool pool = AssetDatabase.LoadAssetAtPath<BonusPool>(POOL_ASSET_PATH);
		Assert.IsNotNull(pool, $"BonusPool asset is missing at {POOL_ASSET_PATH}");

		List<int> counts = new List<int>(pool.outcomes.Length);
		foreach (var outcome in pool.outcomes) {
			counts.Add(0);
		}

		for (int i = 0; i < 1000; ++i) {
			++counts[pool.GetRandomIndex()];
		}

		StringBuilder message = new StringBuilder("Randomization results for 1000 spins");
		for (int i = 0; i < counts.Count; ++i) {
			message.Append($"\n- {pool.outcomes[i].id}: {counts[i]}");
		}

		Debug.Log(message);
	}

	[Test]
	public void ValidatePoolContent() {
		BonusPool pool = AssetDatabase.LoadAssetAtPath<BonusPool>(POOL_ASSET_PATH);
		Assert.IsNotNull(pool, "BonusPool asset is missing at " + POOL_ASSET_PATH);
		Assert.Greater(pool.outcomes.Length, 0, "No outcomes are specified in the pool.");

		List<string> usedIDs = new List<string>();
		foreach (var outcome in pool.outcomes) {
			Assert.False(usedIDs.Contains(outcome.id), $"Multiple outcomes use the same ID: {outcome.id}");
			Assert.IsNotNull(outcome.item, $"ItemType is missing for outcome: {outcome.id}");
			Assert.NotZero(outcome.quantity, $"Quantity is zero for outcome: {outcome.id}");
			Assert.Greater(outcome.weight, 0, $"Weight is not greater than zero for outcome: {outcome.id}");

			usedIDs.Add(outcome.id);
		}
	}
}
