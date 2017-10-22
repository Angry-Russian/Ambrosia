using UnityEngine;
using System.Collections;

public class NeighboringTile : MonoBehaviour {
    
    public GameObject contains;
    public GameObject[] neighbors; // 0: east, 1: north, 2: west, 3: south

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public bool isPassable() {
        return contains == null;
    }

    public GameObject getNeighbor(int n) {
        if (neighbors.Length > 0)
            return neighbors[n % neighbors.Length];
        return null;
    }

    public IEnumerable getNeighbors() {
        var result = new ArrayList();
        foreach (var n in neighbors) {

            if (!n) continue;

            var nt = n.GetComponent<NeighboringTile>();
            if (nt.contains == null)
                result.Add(n);
        }
        return result;
    }
}
