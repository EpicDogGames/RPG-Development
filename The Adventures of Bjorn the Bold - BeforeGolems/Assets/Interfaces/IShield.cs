using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShield  {

	List<BaseStat> Stats { get; set; }
	CharacterStats CharacterStats { get; set; }

	void RaiseShield ();
}
