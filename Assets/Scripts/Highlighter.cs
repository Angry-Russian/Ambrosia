using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {

    private bool active = false;
    private bool _locked = false;

    public Material highlightMaterial;
    private Material currentMaterial;

    // Use this for initialization
    void Start () {
        currentMaterial = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        if (active && highlightMaterial != null) {
            GetComponent<Renderer>().material = highlightMaterial;

        } else if(currentMaterial != null){
            GetComponent<Renderer>().material = currentMaterial;
        }
    }

    public void setActive(bool a) {
        if (_locked) return;
        active = a;
    }

    public bool getActive() {
        return active;
    }

    public void setLocked(bool l) {
        _locked = l;
    }
}
