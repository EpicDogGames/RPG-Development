using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWellController : MonoBehaviour
{

	public static LightWellController Instance;

    public Renderer wellRend;
    public SpriteRenderer runesRend;
    public Light wellLight;
    public Transform runesTF, starsTF;
	public ParticleSystem starsParticles;
   // public AudioSource starsAudio;

    private bool inTransition, activateWell, transitionFinished, rotateEffects;
    private float transitionTimer, targetValueColor, rotationSpeed;
    private Material wellMat;
    private Color wellMatColor = new Color(0, 0, 0, 0), runesMatColor = new Color(0, 0, 0, 0);
    private Vector3 v3Up = new Vector3(0, 1, 0);

    // Use this for initialization
    void Start()
    {
		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else  {
			Instance = this;
		}
        wellMat = wellRend.material;
        runesMatColor = runesRend.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (inTransition)
        {
            transitionTimer += 0.5f * Time.deltaTime;
            if (activateWell)
            {
                //fades the Emission of the well material in.
                if (wellMatColor[3] < 1)
                {
                    float ra = Mathf.MoveTowards(wellMatColor[3], 1, 0.12f * Time.deltaTime);
                    wellMatColor[1] = ra * 0.753f;//G
                    wellMatColor[2] = ra * 0.753f;//B
                    wellMatColor[3] = ra;//A
                    wellMat.SetColor("_EmissionColor", wellMatColor);
                }

                if (transitionTimer > 0.6f)
                {
                    //fades in the well light Intencity
                    if (wellLight.intensity < 1.4f)
                        wellLight.intensity = Mathf.MoveTowards(wellLight.intensity, 1.4f, 0.14f * Time.deltaTime);
                    
                    if (transitionTimer > 1)
                    {
                        //fades in the Transperancy of the Runes material.
                        if (runesMatColor[3] < 0.2f)
                        {
                            runesMatColor[3] = Mathf.MoveTowards(runesMatColor[3], 0.2f, 0.03f * Time.deltaTime);//A
                            runesRend.color = runesMatColor;
                        }

                        if (transitionTimer > 1.2f)
                        {

                            if (!starsParticles.isPlaying)
                                starsParticles.Play();

                            if (transitionTimer > 3f)
                            {
                                //wait for all the effects to fully start
                                if (transitionTimer > 6f)
                                    inTransition = false;
                            }
                        }
                    }
                }
            }           
            else if (!activateWell)
            {
                //fades the Emission of the well material in.
                if (wellMatColor[3] > 0)
                {
                    float ra = Mathf.MoveTowards(wellMatColor[3], 0, 0.2f * Time.deltaTime);
                    wellMatColor[1] = ra * 0.753f;//G
                    wellMatColor[2] = ra * 0.753f;//B
                    wellMatColor[3] = ra;//A
                    wellMat.SetColor("_EmissionColor", wellMatColor);
                }

                //fades in the Transperancy of the Runes material.
                if (runesMatColor[3] > 0)
                {
                    runesMatColor[3] = Mathf.MoveTowards(runesMatColor[3], 0f, 0.05f * Time.deltaTime);//A
                    runesRend.color = runesMatColor;
                }

                //fades out the well light Intencity
                if (wellLight.intensity > 0)
                    wellLight.intensity = Mathf.MoveTowards(wellLight.intensity, 0f, 0.4f * Time.deltaTime);

                transitionTimer += 0.5f * Time.deltaTime;

                //wait for all the effects to fully shut down
                if (transitionTimer > 6f)
                    inTransition = false;
            }
        }

        if (rotateEffects)
        {
            //rotating stars and runes
            runesTF.Rotate(v3Up, 35 * Time.deltaTime);
            starsTF.Rotate(v3Up, 12 * Time.deltaTime);
        }
        else
        {
            if (rotationSpeed > 0)
            {
                //slowly stopping the rotating stars and runes 
                rotationSpeed = Mathf.MoveTowards(rotationSpeed, 0, 0.2f * Time.deltaTime);
                runesTF.Rotate(v3Up, (35 * rotationSpeed) * Time.deltaTime);
                starsTF.Rotate(v3Up, (12 * rotationSpeed) * Time.deltaTime);
            }
        }
    }

    public void ActivateDeactivateWell()
    {
        //wait for the effects to fully play out before being able to activate/Deactivate again.
        if (inTransition)
            return;

        activateWell = !activateWell;

        if (!activateWell)
        {
            starsParticles.Stop();

            rotationSpeed = 1;
        }

        rotateEffects = activateWell;

        transitionTimer = 0;
        inTransition = true;
    }
}
