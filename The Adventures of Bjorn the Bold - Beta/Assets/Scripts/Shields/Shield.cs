using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IShield {

	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }

	public void RaiseShield()  {
		Debug.Log ("Raise Shield!!");
	}
}
