using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class UseController : MonoBehaviour {

	public float range = 2;			//Players range
	public AudioClip useSound;		//The sound that plays when the player is using sth
	public GameObject cam;			//The players cam
	
	AudioSource audioSource;		//Audio player
	FalseTruth.KeyUse keyUse;					//The script that controls the UI

	void Start() {
		keyUse = GameObject.FindWithTag("GUI").GetComponentInChildren<FalseTruth.KeyUse>();	//Get the components
        audioSource = GetComponent<AudioSource>();									//
	}

	void Update () {
		keyUse.Deactivate();	//Deactivates the UI
		RaycastHit hit;			//a ray from the camera to where the camera is facing
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) {	//If the ray is hitting something
			GameObject hitObject = hit.collider.gameObject;		//The hit object
			if(hitObject.CompareTag("Useable")) {			//If the hit object is tagged as useable
				keyUse.Activate();								//Activates the UI
				if(Input.GetKeyDown(UserSettings.interact)) {			//If the player pressed the interact key
					audioSource.clip = useSound;				//Play the audio clip
					audioSource.Play();							//
					hitObject.GetComponent<FalseTruth.Useable>().Use();	//Use the object
				}
			} 
		}
	}

	public void Hide() {
		keyUse.Deactivate();
	}
}
}