using UnityEngine;
using System.Collections;
using System;
using System.Text;
using UnityEngine.Windows.Speech;


public class MicrophoneInput : MonoBehaviour 
{
	public float sensitivity = 100;
	public float loudness = 0;
	public float[] loudnessQ;
	public int qLen = 0;
	private int i;
	private int sum;
	AudioSource _audio;

	private Rigidbody playerRBody;

	[SerializeField]
	private string[] m_Keywords;

	private KeywordRecognizer m_Recognizer;

	void Start()
	{
		qLen = loudnessQ.Length;
		i = 0;
		_audio = GetComponent<AudioSource>();
		_audio.clip = Microphone.Start(null, true, 10, 44100);
		_audio.loop = true;
		_audio.mute = false;
		while(!(Microphone.GetPosition(null) > 0)){}
		_audio.Play();

		playerRBody = GetComponent<Rigidbody>();


		m_Recognizer = new KeywordRecognizer(m_Keywords);
		m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
		m_Recognizer.Start();
		Debug.Log (m_Recognizer.IsRunning);
	}

	void Update()
	{
		loudness = GetAveragedVolume() * sensitivity;

		//loudness of last 30 frames

		if (i < qLen) {
			loudnessQ [i] = loudness;
			i++;
		} else 
		{
			i = 0;
			loudnessQ [i] = loudness;
		}

	}

	void FixedUpdate()
	{

		/*if(loudness > 0f && loudness < 1f)
			Debug.Log (.5f);

		if(loudness > 1f && loudness < 2f)
			Debug.Log (1.5f);

		if(loudness > 2f && loudness < 3f)
			Debug.Log (2.5f);

		if(loudness > 3f && loudness < 4f)
			Debug.Log (3.5f);*/
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

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		/*StringBuilder builder = new StringBuilder();
		builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
		builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
		builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
		Debug.Log(builder.ToString());*/
		sum = 0;
		foreach (int item in loudnessQ) {
			sum += item;
		}

		if (args.text == "jump") {
			Debug.Log (sum/qLen);
		}
	}
}