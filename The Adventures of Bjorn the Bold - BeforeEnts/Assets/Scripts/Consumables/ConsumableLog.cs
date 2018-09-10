using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableLog : MonoBehaviour, IConsumable {

	public void Consume()  {
		Destroy (gameObject);
	}

	public void Consume(CharacterStats stats)  {
		Destroy (gameObject);
	}
}
