using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tags : MonoBehaviour {

    public List<GameObject> AllTags;

    public bool Contains(string _tag)
    {
        return AllTags.Any(x => x.CompareTag(_tag));
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
