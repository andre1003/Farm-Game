using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantZoneSpawner : MonoBehaviour {
    public GameObject plantationZonePrefab;

    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.gameObject.tag != "Plantable")
                    Instantiate(plantationZonePrefab, hit.point, Quaternion.identity);
                else {
                    GameObject plantationZone = hit.collider.gameObject;
                    Vector3 spawnPosition = new Vector3(plantationZone.transform.position.x + 3, plantationZone.transform.position.y, plantationZone.transform.position.z);
                    Vector3 size = plantationZonePrefab.GetComponent<BoxCollider>().size + new Vector3(-1, -1, -1);
                    //bool canSpawn = Physics.CheckSphere(spawnPosition, groundMask);
                    bool canSpawn = PreventSpawnOverlap(spawnPosition);
                    if(canSpawn)
                        Instantiate(plantationZonePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    // Not working. Needs more research
    private bool PreventSpawnOverlap(Vector3 position) {
        Collider[] colliders = Physics.OverlapSphere(position, groundDistance, groundMask);

        for(int i = 0; i < colliders.Length; i++) {
            Vector3 center = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float height = colliders[i].bounds.extents.z;

            float leftExtent = center.x - width;
            float rightExtent = center.x + width;
            float lowerExtent = center.z - height;
            float upperExtent = center.z + height;

            if(position.x >= leftExtent && position.x <= rightExtent) {
                if(position.z >= lowerExtent && position.z <= upperExtent) {
                    return false;
                }
            }

            
        }
        return true;
    }
}
