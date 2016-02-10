using UnityEngine;
using System.Collections;

public class TowerCreator : MonoBehaviour {

    public GameObject roof;
    public int floors = 3;
    public float floorHeight = 8;
    public GameObject roomCreator;

    public int startingFloor = 0;
    public Vector2 startingPosition;
    public GameObject player;

    private bool _connected = false;
    // Use this for initialization
    void Start() {
        GameObject lastRoom = null;
        for (int i = 0; i < floors; i++) {
            GameObject room = (GameObject)Instantiate(roomCreator, transform.position + Vector3.down * floorHeight * i, Quaternion.identity);
            room.transform.parent = transform;

            if (roof != null) {
                GameObject r = (GameObject)Instantiate(roof, room.transform.position, room.transform.rotation);
                r.transform.parent = room.transform;
                roof = null;
            }

            if (i == startingFloor) {
                RoomCreator rc = room.GetComponent<RoomCreator>();
                rc.startingPosition = startingPosition;
                rc.player = Instantiate(player);
                /*if (rc) {
                    GameObject p = (GameObject)Instantiate(player); //, new Vector3(startingPosition.x, -floorHeight * i + player.transform.localScale.y + 0.1f, startingPosition.y), Quaternion.identity
                    GameObject tile = rc.tiles[(int)startingPosition.x, (int)startingPosition.y];
                    p.GetComponent<MovementHandler>().teleportTo(tile);

                } else Instantiate(player, new Vector3(startingPosition.x, -floorHeight * i + player.transform.localScale.y + 0.1f, startingPosition.y), Quaternion.identity);//*/
            }

            lastRoom = room;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
