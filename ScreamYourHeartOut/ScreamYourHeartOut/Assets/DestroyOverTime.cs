using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

    public float lifetime = 4;

	// Use this for initialization
	void Start () {
        DestroyGameObject();
	}

    public void DestroyGameObject()
    {
        Destroy(gameObject, lifetime);
    }
}
