using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] Sound[] sounds;

	private void Awake()
	{
		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.audioClip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
	}


	public void PlaySound(string ID)
	{
		Sound s = Array.Find(sounds, sound => sound.audioName == ID);
		s.source.Play();
	}
}
