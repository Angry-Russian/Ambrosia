using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MovementHandler : MonoBehaviour {

    public ArrayList path;
    public float maxSpeed = 5;
    public GameObject playerTile;

	// Use this for initialization
	void Start () {
        Camera.main.transform.position = Vector3.up * transform.position.y + new Vector3(1, 1, -1) * 10;
    }

    // Update is called once per frame
    void Update() {
        if (path != null && path.Count > 0) {
            GameObject nextTile = (GameObject) path[0];
            Vector3 direction = (nextTile.transform.position + Vector3.up * (nextTile.transform.localScale.y / 2 + transform.localScale.y + 0.1f)) - transform.position;
            //direction.y = 0;
            if (direction.magnitude > maxSpeed * Time.deltaTime)
                direction = direction.normalized * maxSpeed * Time.deltaTime;

            transform.position += direction;

            if (direction.magnitude == 0) {
                nextTile.GetComponent<Highlighter>().setLocked(false);
                playerTile = nextTile;
                path.RemoveAt(0);
            }
        }
    }

    public void teleportTo(GameObject tile) {
        transform.position = (tile.transform.position + Vector3.up * (tile.transform.localScale.y / 2 + transform.localScale.y + 0.1f));
        playerTile = tile;
    }

    public void moveTowards(GameObject selectedTile) {
        if (!selectedTile) return;
        path = new ArrayList();
        ArrayList newPath = aStar(playerTile, selectedTile);
        if(newPath != null)
            path.AddRange(newPath);
    }

    private ArrayList aStar(GameObject startTile, GameObject endTile) {

        if (!startTile || !endTile)
            return null;

        ArrayList openList;
        ArrayList closedList;
        GameObject currentTile = null;

        closedList = new ArrayList();
        openList = new ArrayList();
        openList.Add(startTile);

        Dictionary<GameObject, float> g = new Dictionary<GameObject, float>();
        g.Add(startTile, 0);
        Dictionary<GameObject, float> f = new Dictionary<GameObject, float>();
        f.Add(startTile, (startTile.transform.position - endTile.transform.position).magnitude);

        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        fScoreComparer fc = new fScoreComparer();
        fc.fList = f;

        do {
            openList.Sort(fc);
            currentTile = (GameObject)openList[0];
            if (currentTile == endTile)
                return reconstructPath(cameFrom, currentTile);

            openList.RemoveAt(0);
            closedList.Add(currentTile);

            foreach (GameObject item in currentTile.GetComponent<NeighboringTile>().getNeighbors()) {
                if (item == null || closedList.Contains(item))
                    continue;

                float currentScore = g[currentTile];
                float approx = currentScore + (currentTile.transform.position - endTile.transform.position).magnitude;

                if (!openList.Contains(item))
                    openList.Add(item);
                else if (approx >= g[item])
                    continue;

                cameFrom[item] = currentTile;
                g[item] = approx;
                f[item] = approx + 0; // 0 = heuristic cost. Not implemented yet.
            }

        } while (openList.Count > 0);

        return null;
    }

    public ArrayList reconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current) {
        ArrayList totalPath = new ArrayList();
        totalPath.Add(current);
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        return totalPath;
    }
    
    public class fScoreComparer : IComparer {
        public Dictionary<GameObject, float> fList;

        public int Compare(object x, object y) {
            if (fList == null) throw new Exception("Must Set fList property");
            return (fList[(GameObject)x] < fList[(GameObject)y]) ? -1 : 1;
        }
    }
}
