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
        else return null;
    }

    public IEnumerable getNeighbors() {
        ArrayList result = new ArrayList();
        foreach (GameObject n in neighbors) {

            if (!n) continue;

            NeighboringTile nt = n.GetComponent<NeighboringTile>();
            if (nt.contains == null)
                result.Add(n);
        }
        return result;
    }
}
