using UnityEngine;
using System.Collections;
using System;
using System.Text;
using UnityEngine.Windows.Speech;


public class Elevator : MonoBehaviour {

	private bool down;
	private bool up;
	public float moveSpeed;

	[SerializeField]
	public string[] m_Keywords;

	private KeywordRecognizer m_Recognizer;


	// Use this for initialization
	void Start () {
		moveSpeed = 1.5f;
		up = false;
		down = false;
		m_Recognizer = new KeywordRecognizer(m_Keywords);
		m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
		m_Recognizer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if (up == true) {
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime);
		} else if (down == true) {
			transform.Translate (Vector3.down * moveSpeed * Time.deltaTime);
		}
	}

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		if (args.text == "Up") {
			Debug.Log ("Up");
			up = true;
			down = false;
		} else if (args.text == "Down") {
			Debug.Log ("Down");
			down = true;
			up = false;
		}
	}
}
