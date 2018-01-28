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

    private GameObject elevator;
    private RaycastHit hit;

    private PlayerMovement pmove;
    private GameObject elevatorFloor;

    // Use this for initialization
    void Start () {
		moveSpeed = 1f;
		up = false;
		down = false;
		m_Recognizer = new KeywordRecognizer(m_Keywords);
		m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
		m_Recognizer.Start();

        pmove = GetComponent<PlayerMovement>();
	}

    void FixedUpdate()
    {
        if (Physics.Raycast(this.transform.position - new Vector3(0, -0.1f, 0), Vector3.down, out hit, 5.0f))
        {
            elevator = hit.transform.gameObject;
        }
    }

    // Update is called once per frame
    void Update () {
        if (up == true && pmove.isTagElevator) {
            elevator.transform.GetComponent<ElevatorInfo>().Raise();
		} else if (down == true && pmove.isTagElevator) {
            elevator.transform.GetComponent<ElevatorInfo>().Lower();
		}

        if (!pmove.isTagElevator)
        {
            up = false;
            down = false;
        }
	}
    
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
        if (pmove.isTagElevator)
        {
            if (args.text == "Raise")
            {
                Debug.Log("Up");
                up = true;
                down = false;
            }
            else if (args.text == "Lower")
            {
                Debug.Log("Down");
                down = true;
                up = false;
            }
        }
	}
}
