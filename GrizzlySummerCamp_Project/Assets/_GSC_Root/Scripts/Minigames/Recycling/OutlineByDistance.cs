using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineByDistance : MonoBehaviour
{
    /*
    Renderer rend;
    MaterialPropertyBlock mpb;
    Transform player;
    TrashItem trash;

    bool highlighted;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = GetComponent<MaterialPropertyBlock>();
        trash = GetComponent<TrashItem>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null || trash == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool shouldHighlight = dist <= trash.InteractionDistance;

        if (shouldHighlight != highlighted) 
        {
            highlighted = shouldHighlight;
            SetHighlight(highlighted);
        }
    }

    void SetHighlight(bool highlight) {

    */

}