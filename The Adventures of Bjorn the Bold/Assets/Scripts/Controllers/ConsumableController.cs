using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : MonoBehaviour {

	private int healAmount_HealthDraft = 10;
	private int healAmount_HealthElixir = 20;
	private int manaAmount = 10;
	private CharacterStats stats;

	// Use this for initialization
	void Start () {
		stats = GetComponent<Player>().characterStats;
	}

	public void ConsumeItem(Item item)  {
		GameObject itemToConsume = Instantiate (Resources.Load<GameObject> ("Consumables/" + item.ObjectSlug));
		if (item.ItemModifier)  {
			itemToConsume.GetComponent<IConsumable> ().Consume(stats);
		}
		else  {
			itemToConsume.GetComponent<IConsumable> ().Consume ();
			Debug.Log ("Item Name: " + item.ItemName);
			// weak draft
			if (item.ItemName == "Healing Draft")  {
				Debug.Log ("You just selected the healing draft");
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().HealPlayer(healAmount_HealthDraft);
			}
			// health elixir (red potion)
			if (item.ItemName == "Health Elixir")  {
				Debug.Log ("You just selected the health elixir");
				GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HealPlayer(healAmount_HealthElixir);
			}
			// mana elixir (blue potion)
			if (item.ItemName == "Mana Elixir")  {
				Debug.Log ("Player gains more magic");
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().AddManaToPlayer(manaAmount);
			}
		}
		InventoryController.Instance.RemoveItem (item);
	}

}
