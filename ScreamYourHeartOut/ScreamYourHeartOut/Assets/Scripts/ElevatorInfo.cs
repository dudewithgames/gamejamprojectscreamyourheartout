using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInfo : MonoBehaviour {
    public GameObject bottom;
    public GameObject top;

    private bool up, down;

    private void Start()
    {
        up = false;
        down = false;
    }

    private void Update()
    {
        if (up)
            this.transform.Translate(Vector3.up * 3f * Time.deltaTime);
        else if (down)
            this.transform.Translate(Vector3.down * 3f * Time.deltaTime);
    }
    private void LateUpdate()
    {
        if (IsAtBottom() && down)
            down = false;

        if (IsAtTop() && up)
            up = false;
    }

    public void Raise()
    {
        if(!IsAtTop())
        {
            up = true;
            down = false;
        }
    }

    public void Lower()
    {
        if (!IsAtBottom())
        {
            down = true;
            up = false;
        }
    }

    bool IsAtBottom()
    {
        if (this.transform.position.y <= bottom.transform.position.y)
            return true;

        return false;
    }

    bool IsAtTop()
    {
        if (this.transform.position.y >= top.transform.position.y)
            return true;

        return false;
    }
}
