using System;
using System.Collections.Generic;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir
{
	public class PlayerInventory : MonoBehaviour
	{
		private static PlayerInventory s_instance;

		private List<RuleData> purchasedRules = new();
		public List<LevelData> PlayedLevels { get; private set; } = new();

		public event Action OnRuleAdded;
		public IReadOnlyList<RuleData> PurchasedRules => purchasedRules;

		public static PlayerInventory Instance
		{
			get
			{
				if (s_instance == null)
				{
					Initialize();
				}

				return s_instance;
			}
		}

		public void AddRule(RuleData rule)
		{
			purchasedRules.Add(rule);
			OnRuleAdded?.Invoke();
		}

		private static void Initialize()
		{
			s_instance = FindFirstObjectByType<PlayerInventory>(FindObjectsInactive.Include);
			if (s_instance)
			{
				DontDestroyOnLoad(s_instance);
			}
		}

		private void Awake()
		{
			if (s_instance == null)
			{
				Initialize();
				return;
			}

			Destroy(this);
		}
	}
}