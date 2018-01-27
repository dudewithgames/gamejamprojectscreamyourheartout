using UnityEngine;
using System.Collections;

public class MicrophoneInput : MonoBehaviour 
{
	public float sensitivity = 100;
	public float loudness = 0;
	AudioSource _audio;
	
	private Rigidbody playerRBody;
	
	void Start()
	{
		_audio = GetComponent<AudioSource>();
		_audio.clip = Microphone.Start(null, true, 10, 44100);
		_audio.loop = true;
		_audio.mute = false;
		while(!(Microphone.GetPosition(null) > 0)){}
		_audio.Play();
		
		playerRBody = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		loudness = GetAveragedVolume() * sensitivity;
	}
	
	void FixedUpdate()
	{
		if(loudness > 8.4f && loudness < 9f)
		{
			playerRBody.AddForce(Vector3.up, ForceMode.Impulse);
		}else if(loudness > 9.2f && loudness < 10f)
		{
			playerRBody.AddForce(Vector3.forward, ForceMode.Impulse);
		}
	}
	
	float GetAveragedVolume()
	{
		float [] data = new float[256];
		float a = 0;
		_audio.GetOutputData(data, 0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}
}