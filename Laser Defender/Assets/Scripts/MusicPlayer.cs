using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake(){

		//If there are already music players, destroy this one
		if (FindObjectsOfType(GetType()).Length > 1) { Destroy(this.gameObject); }

		//There are no other music players so dont destroy this one on load
		else { DontDestroyOnLoad(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
