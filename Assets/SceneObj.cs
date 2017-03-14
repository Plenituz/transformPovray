using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObj : MonoBehaviour {
    public bool include = false;

	void Awake()
    {
        SceneArranger.objs.Add(this);
    }
}
