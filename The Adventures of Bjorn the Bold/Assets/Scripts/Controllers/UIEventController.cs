using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventController : MonoBehaviour {

	public delegate void ItemEventHandler(Item item);
	public static event ItemEventHandler OnItemAddedToInventory = delegate{};
	public static event ItemEventHandler OnItemAddedToQuest = delegate{};
	public static event ItemEventHandler OnItemEquipped = delegate{};
	public static event ItemEventHandler OnItemShielded = delegate{};
	public static event ItemEventHandler OnItemRemovedFromInventory = delegate{};
	public static event ItemEventHandler OnItemRemovedFromQuest = delegate{};

	public delegate void PlayerHealthEventHandler(int currentHealth, int maxHealth);
	public static event PlayerHealthEventHandler OnPlayerHealthChanged = delegate{};

	public delegate void PlayerManaEventHandler(int currentMana, int maxMana);
	public static event PlayerManaEventHandler OnPlayerManaChanged = delegate{};

	public delegate void PlayerStatsEventHandler ();
	public static event PlayerStatsEventHandler OnPlayerStatsChanged = delegate{};

	public static void ItemAddedToInventory(Item item)  {
		OnItemAddedToInventory (item);
	}

	public static void ItemAddedToQuest(Item item)  {
		OnItemAddedToQuest (item);
	}

	public static void ItemEquipped(Item item)  {
		OnItemEquipped (item);
	}

	public static void ItemShielded(Item item)  {
		OnItemShielded (item);
	}

	public static void ItemRemovedFromInventory(Item item)  {
		OnItemRemovedFromInventory (item);
	}

	public static void ItemRemovedFromQuest(Item item)  {
		OnItemRemovedFromQuest(item);
	}

	public static void PlayerHealthChanged(int currentHealth, int maxHealth)  {
		OnPlayerHealthChanged (currentHealth, maxHealth);
	}

	public static void PlayerManaChanged(int currentMana, int maxMana)  {
		OnPlayerManaChanged (currentMana, maxMana);
	}

	public static void PlayerStatsChanged()  {
		OnPlayerStatsChanged();
	}
}
