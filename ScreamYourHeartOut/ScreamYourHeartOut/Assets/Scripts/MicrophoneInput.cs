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
	private bool left;
	private bool right;
    private bool respawn;
	AudioSource _audio;

	private Rigidbody playerRBody;
    private GameObject player;

	[SerializeField]
	private string[] m_Keywords;

	private KeywordRecognizer m_Recognizer;
    private float loudnessAvg;

    public GameObject Shout;
    public GameObject ShoutEffect;
    private SphereCollider sphereCollider;

    private Rigidbody rbody;
	private Animator anim;
	private float moveSpeed;

    void Start()
	{
        sphereCollider = this.gameObject.GetComponent<SphereCollider>();
        player = this.gameObject;
		moveSpeed = 0.31f;
		anim = this.gameObject.GetComponent<Animator>();
		anim.SetBool("IsIdle", true);
		//Grab Player Rigidbody
		rbody = this.gameObject.GetComponent<Rigidbody>();

		left = false;
		right = false;
        respawn = false;

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

		if (left == true) {
			this.gameObject.GetComponent<SpriteRenderer> ().flipX = true;
			anim.SetBool ("IsIdle", false);
			rbody.MovePosition (transform.position + new Vector3 (-moveSpeed, 0));

		} else if (right == true) {
			this.gameObject.GetComponent<SpriteRenderer> ().flipX = false;
			anim.SetBool ("IsIdle", false);
			rbody.MovePosition (transform.position + new Vector3 (moveSpeed, 0));
		}

		//loudness of last 30 frames

		if (i < qLen) {
			loudnessQ[i] = loudness;
			i++;
		} else 
		{
			i = 0;
			loudnessQ[i] = loudness;
            i++;
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

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
        /*StringBuilder builder = new StringBuilder();
		builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
		builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
		builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
		Debug.Log(builder.ToString());*/

        loudnessAvg = VoiceAveragePerFrame();
        Debug.Log(loudnessAvg);

        Shout = (GameObject)Instantiate(ShoutEffect, (this.transform.position + new Vector3(0, 3.3f, 0)), transform.rotation);

        if (args.text == "Jump" || args.text == "Roof")
        {
            Debug.Log("Jump");
            playerRBody.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
        else if (args.text == "Freeze")
        {
            Time.timeScale = 0;
        }
        else if (args.text == "Unfreeze")
        {
            StartCoroutine("Increase");
            Time.timeScale = 1;
        }
        else if (args.text == "Left")
        {
            left = true;
            right = false;
        }
        else if (args.text == "Right")
        {
            right = true;
            left = false;
        }
        else if (args.text == "Stop")
        {
            right = false;
            left = false;
            anim.SetBool("IsIdle", true);
        }
        else if (args.text == "Respawn")
        {
            right = false;
            left = false;
            player.transform.position = new Vector3(0, .5f, 0);
            playerRBody.velocity = new Vector3(0, 0, 0);
        }
        else if (args.text == "Quit")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

    }

    float VoiceAveragePerFrame()
    {
        float avg = 0f;
        foreach (int v in loudnessQ)
        {
            avg += v;
        }
        
        return avg / loudnessQ.Length;
    }
}