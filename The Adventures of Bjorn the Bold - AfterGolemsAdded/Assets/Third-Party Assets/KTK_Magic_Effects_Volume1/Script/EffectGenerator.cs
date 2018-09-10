 using UnityEngine;
using System.Collections;

public class EffectGenerator : MonoBehaviour {
	
	public float hSliderValue = 0.5F;
	public float hSliderValue_speed = 10.0F;
	public float hSliderValue_scale = 1.0f;
	public Material FloorMat;

	private GameObject Eff_Point;

	public GameObject[] Effect_List;

	private Vector2 scrollViewVector = Vector2.zero;
	
	private Rect scrollViewRect = new Rect(0, 20, 170, 500);
	
	private Rect scrollViewAllRect = new Rect (40, 70, 100, 1000);


	void Awake() {
		Eff_Point = GameObject.Find("Eff_Point");
	}
		
	void OnGUI() {

		// SpeedControl Slider
		hSliderValue_speed = GUI.HorizontalSlider(new Rect(200, 30, 200, 30), hSliderValue_speed, 5.0F, 20.0F);
		GUI.Label(new Rect(410, 15,  200, 20), "BulletSpeed: " + hSliderValue_speed);

		// ScaleControl Slider
		hSliderValue_scale = GUI.HorizontalSlider(new Rect(200, 80, 200, 30), hSliderValue_scale, 0.1F, 3.0F);
		GUI.Label(new Rect(410, 75,  200, 20), "BulletScale: " + hSliderValue_scale);

		scrollViewVector = GUI.BeginScrollView(scrollViewRect , scrollViewVector, scrollViewAllRect);


		for(int i = 0; i < Effect_List.Length; i++)
		{
			if(Effect_List[i] != null){
				if (GUI.Button(new Rect(50, 70 + i * 40, 140, 30), Effect_List[i].name))
				{
					GameObject clone1 = Instantiate(Effect_List[i], Eff_Point.transform.position, Quaternion.identity) as GameObject;

					// Bullet Speed Set
					clone1.GetComponent<BulletController> ().bulletSpeed = hSliderValue_speed;

					// Effect Scale
					clone1.transform.localScale = new Vector3 (hSliderValue_scale, hSliderValue_scale, hSliderValue_scale);

					clone1.transform.rotation = Quaternion.Euler(0,  0 , 0 );
					clone1.transform.parent = Eff_Point.transform;



				}
			}
		}
		
		
		GUI.EndScrollView();

	}

}
