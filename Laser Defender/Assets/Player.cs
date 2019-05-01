using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        move();
    }

    private void move() {
        var deltaX = Input.GetKeyDown;
        var newXPos = transform.position.x + deltaX;
    }
}
