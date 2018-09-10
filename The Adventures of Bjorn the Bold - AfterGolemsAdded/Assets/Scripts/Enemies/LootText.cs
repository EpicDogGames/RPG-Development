using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootText : MonoBehaviour {

	public Animator lootAnimator;
	private Text lootText;

	void Awake()  {
		// get how long the animation plays and use it to determine how long to display the text until destroying it
		AnimatorClipInfo[] clipInfo = lootAnimator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);
		lootText = lootAnimator.GetComponent<Text> ();
	}

	public void SetLootText(string text)  {
		// update text of the UI
		lootText.text = text;
	}
}
