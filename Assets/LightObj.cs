using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObj : MonoBehaviour {

	void Awake () {
        SceneArranger.lights.Add(this);
	}
}
