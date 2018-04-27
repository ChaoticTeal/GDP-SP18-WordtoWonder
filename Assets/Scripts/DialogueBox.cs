using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
        CheckExit();
	}

    void CheckExit()
    {
        if (Input.GetButtonDown("Interact"))
            Destroy(gameObject);
    }
}
