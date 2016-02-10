using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class MouseHandler : MonoBehaviour {

    private GameObject selectedTile;
    private GameObject playerTile;
    private GameObject player;
    

    private Vector3 lastMousePosition;
    private float scrollVelocity;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        lastMousePosition = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {

        selectedTile = null;

        // ==========================================================
        // === Highlighting tiles on mouseover
        // ==========================================================

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(r, int.MaxValue);
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        // set all to incative
        foreach (GameObject tile in tiles) {
            if (Physics.Raycast(tile.transform.position, Vector3.up, 2, 1<<8)) {
                playerTile = tile;
            } else tile.GetComponent<Highlighter>().setActive(false);
            Debug.DrawRay(tile.transform.position, Vector3.up * 2, Color.yellow);
        }

        if(playerTile != null)
            playerTile.GetComponent<Highlighter>().setActive(true);

        if (selectedTile != null)
            selectedTile.GetComponent<Highlighter>().setActive(true);

        // highlight objects under mouse
        foreach (RaycastHit hit in hits) {
            GameObject go = hit.collider.gameObject;
            if (go.CompareTag("Tile")) {
                Highlighter script = go.GetComponent<Highlighter>();
                if (script) {
                    script.setActive(true);
                    selectedTile = go;
                }

                break;
            }
        }

        // ==========================================================
        // === Handling Clicks
        // ==========================================================

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            foreach (GameObject t in tiles) {
                Highlighter h = t.GetComponent<Highlighter>();
                if (h) {
                    h.setLocked(false);
                    h.setActive(false);
                }
            }

        } else if (Input.GetKeyUp(KeyCode.Mouse0))
            player.GetComponent<MovementHandler>().moveTowards(selectedTile);



        float delta = (Input.mousePosition - lastMousePosition).y * Time.deltaTime;
        if (Input.GetMouseButton(1)) {
            scrollVelocity = delta;
        } else {
            scrollVelocity *= 0.95f;
        }
        Vector3 cameraPosition = Camera.main.transform.position + Vector3.down * scrollVelocity;
        cameraPosition.y = Math.Min(20, cameraPosition.y);
        Camera.main.transform.position = cameraPosition;
        lastMousePosition = Input.mousePosition;
    }
}
