using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

	string ID { get; set; }

	void TakeDamage (int amount);

	void PerformAttack();

}
