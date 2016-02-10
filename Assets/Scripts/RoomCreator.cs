using UnityEngine;
using System.Collections;

public class RoomCreator : MonoBehaviour {

    public GameObject roof;
    public GameObject pillar;
    public GameObject tile;
    public Vector2 size;
    public Vector2 startingPosition;
    public GameObject player;

    public GameObject[,] tiles;

    // Use this for initialization
    void Start () {

        tiles = new GameObject[(int)size.x, (int)size.y];
        size.x = Mathf.Floor(size.x);
        size.y = Mathf.Floor(size.y);

        // Create Roof
        if (roof != null)
            ((GameObject)Instantiate(roof)).transform.parent = transform;

        // Spawn Tiles
        if (tile != null)
            for (int i = 0; i < size.x; i++) {
                for (int j = 0; j < size.y; j++) {
                    GameObject t = (GameObject)Instantiate(tile, new Vector3(
                        transform.position.x - size.x / 2 + 0.5f + i,
                        transform.position.y,
                        transform.position.z - size.y / 2 + 0.5f + j
                    ), Quaternion.identity);

                    t.transform.parent = transform;
                    t.tag = "Tile";
                    t.GetComponent<NeighboringTile>().neighbors = new GameObject[8];

                    tiles[i, j] = t;

                    // if corner spawn pillar
                    if (i + j == 0
                        || i+j == size.x + size.y - 2
                        || (i==0 && j == size.y - 1)) {
                        GameObject p = ((GameObject)Instantiate(pillar, t.transform.position + Vector3.up * 4, Quaternion.identity));
                        p.transform.parent = transform;
                        t.GetComponent<NeighboringTile>().contains = p;
                    }

                    // j+ -> north
                    // i+ -> east
                    if (i > 0) { // attach west 
                        GameObject westTile = tiles[i - 1, j];
                        westTile.GetComponent<NeighboringTile>().neighbors[0] = t;
                        t.GetComponent<NeighboringTile>().neighbors[4] = westTile;

                        if (j < size.y - 1) {
                            GameObject northWestTile = tiles[i - 1, j + 1];
                            if (northWestTile != null) {
                                northWestTile.GetComponent<NeighboringTile>().neighbors[7] = t;
                                t.GetComponent<NeighboringTile>().neighbors[3] = northWestTile;
                            }
                        }
                    }

                    if (j > 0) { // attach south
                        GameObject southTile = tiles[i, j - 1];
                        southTile.GetComponent<NeighboringTile>().neighbors[2] = t;
                        t.GetComponent<NeighboringTile>().neighbors[6] = southTile;

                        if (i > 0) {
                            GameObject southWestTile = tiles[i - 1, j - 1];
                            if (southWestTile != null) {
                                southWestTile.GetComponent<NeighboringTile>().neighbors[1] = t;
                                t.GetComponent<NeighboringTile>().neighbors[5] = southWestTile;
                            }
                        }
                    }

                    if(startingPosition != null && player != null && startingPosition.x == i && startingPosition.y == j) {
                        player.GetComponent<MovementHandler>().teleportTo(t);
                    }
                }
            }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
