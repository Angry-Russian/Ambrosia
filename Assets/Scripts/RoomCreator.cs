using UnityEngine;
using System.Collections;

public class RoomCreator : MonoBehaviour {

    public GameObject roof;
    public GameObject pillar;
    public GameObject tile;
    public Vector2 size;
    public Vector2 startingPosition;
    public GameObject player;

	public RoomCreator roomBelow;

    public GameObject[,] tiles;

	public float floorHeight = 8;

	private GameObject[] steps;
	private Action _onBuild = null;

	void Awake(){
		tiles = new GameObject[(int)size.x, (int)size.y];
		size.x = Mathf.Floor(size.x);
		size.y = Mathf.Floor(size.y);
	}

    // Use this for initialization
    void Start () {

        // Create Roof
        if (roof != null)
            ((GameObject)Instantiate(roof)).transform.parent = transform;

        // Spawn Tiles
		if (tile != null) {
			for (int i = 0; i < size.x; i++) {
				for (int j = 0; j < size.y; j++) {
					GameObject t = (GameObject)Instantiate (tile, new Vector3 (
						                              transform.position.x - size.x / 2 + 0.5f + i,
						                              transform.position.y,
						                              transform.position.z - size.y / 2 + 0.5f + j
					                              ), Quaternion.identity);

					t.transform.parent = transform;
					t.tag = "Tile";
					t.GetComponent<NeighboringTile> ().neighbors = new GameObject[8];

					tiles [i, j] = t;

					// if corner, spawn pillar
					if (i + j == 0
					                   || i + j == size.x + size.y - 2
					                   || (i == 0 && j == size.y - 1)) {
						GameObject p = ((GameObject)Instantiate (pillar, t.transform.position + Vector3.up * 4, Quaternion.identity));
						p.transform.parent = transform;
						t.GetComponent<NeighboringTile> ().contains = p;
					}

					// j+ -> north
					// i+ -> east
					if (i > 0) { // attach west 
						GameObject westTile = tiles [i - 1, j];
						westTile.GetComponent<NeighboringTile> ().neighbors [0] = t;
						t.GetComponent<NeighboringTile> ().neighbors [4] = westTile;

						if (j < size.y - 1) {
							GameObject northWestTile = tiles [i - 1, j + 1];
							if (northWestTile != null) {
								northWestTile.GetComponent<NeighboringTile> ().neighbors [7] = t;
								t.GetComponent<NeighboringTile> ().neighbors [3] = northWestTile;
							}
						}
					}

					if (j > 0) { // attach south
						GameObject southTile = tiles [i, j - 1];
						southTile.GetComponent<NeighboringTile> ().neighbors [2] = t;
						t.GetComponent<NeighboringTile> ().neighbors [6] = southTile;

						if (i > 0) {
							GameObject southWestTile = tiles [i - 1, j - 1];
							if (southWestTile != null) {
								southWestTile.GetComponent<NeighboringTile> ().neighbors [1] = t;
								t.GetComponent<NeighboringTile> ().neighbors [5] = southWestTile;
							}
						}
					}

					if (player != null && startingPosition.x == i && startingPosition.y == j) {
						player.GetComponent<MovementHandler> ().teleportTo (t);
					}
				}
			}

			if (_onBuild != null)
				_onBuild.Execute ();
		}


		// spawn connecting stairs
		if (roomBelow != null) {
			int stepCount = 20;
			steps = new GameObject[stepCount];
			Quaternion q = new Quaternion ();

			Vector3 normal = (Vector3.right * (size.x + 0.5f)) * 0.375f;
			Vector3 center = Vector3.right * (size.x - 2) * 0.5f + Vector3.forward * (size.y - 2) * 0.5f;

			GameObject lastTile = tiles [4, (int)size.y - 1];
			for (int a = 0; a < stepCount - 1; a++) {
				q = Quaternion.Euler (0, (1 + a) * 270 / stepCount - 180, 0);
				steps [a] = (GameObject)Instantiate (tile, center + q * normal + Vector3.down / (stepCount - 2) * a * floorHeight, Quaternion.identity);
				steps [a].GetComponent<NeighboringTile> ().neighbors = new GameObject[2];
				steps [a].tag = "Tile";
				steps [a].transform.SetParent (transform, false);

				int n = 2 % lastTile.GetComponent<NeighboringTile> ().neighbors.Length;
				lastTile.GetComponent<NeighboringTile> ().neighbors [n] = steps [a];
				steps [a].GetComponent<NeighboringTile> ().neighbors [1] = lastTile;

				lastTile = steps [a];
			}
			lastTile.GetComponent<NeighboringTile> ().neighbors [0] = steps [steps.Length - 3];

			// RoomBelow isn't initialized yet, will run connect.execute() once the floor is created
			Connect connect = new Connect ();
			connect.Prime (lastTile, roomBelow, new Vector2((int)size.x - 1, 4));
			roomBelow.OnBuild (connect);

		}
	}

	void Update () {
	
	}

	public void OnBuild(Action a){
		_onBuild = a;
	}

	// Action pattern
	private class Connect : Action{
		GameObject _tile;
		RoomCreator _dr;
		Vector2 _dc;
		public void Prime(GameObject tile, RoomCreator destinationRoom, Vector2 destinationCoords){
			_tile = tile;
			_dr = destinationRoom;
			_dc = destinationCoords;
		}

		override public void Execute(){
			_dr.tiles [(int)_dc.x, (int)_dc.y].GetComponent<NeighboringTile> ().neighbors[1] = _tile;
			_tile.GetComponent<NeighboringTile> ().neighbors[1] = _dr.tiles [(int)_dc.x, (int)_dc.y];
		}
	}
}
