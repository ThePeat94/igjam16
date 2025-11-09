using System;
using System.Collections.Generic;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir
{
	public class PlayerInventory : MonoBehaviour
	{
		private static PlayerInventory s_instance;

		public List<RuleData> PurchasedRules { get; private set; } = new();

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