using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource mainAudio;
	
	public AudioClip ambient;
	public AudioClip combat;
    // Start is called before the first frame update
    void Start()
    {
        mainAudio = GetComponent<AudioSource>();
		mainAudio.volume = 0.3f;
		mainAudio.loop = true;
		mainAudio.Play();
    }

    // Update is called once per frame
    /*void Update()
    {
		
    }*/
	
	private float pausedTime = 0.0f;
	public void swapCombatTrack() {
		pausedTime = mainAudio.time;
		mainAudio.Stop();
		mainAudio.clip = combat;
		mainAudio.Play();
	}
	
	public void swapExplorationTrack() {
		mainAudio.Stop();
		mainAudio.clip = ambient;
		if (pausedTime > 0.0f) {
			mainAudio.time = pausedTime;
			pausedTime = 0.0f;
		}
		mainAudio.Play();
		mainAudio.volume = 0.0f;
		StartCoroutine(increaseVolume());
	}
	IEnumerator increaseVolume() { //Coroutine to fade audio back
		for (int i = 0; i<6;i++) {
			mainAudio.volume += 0.05f;
			yield return new WaitForSeconds(1);
		}
	}
}
