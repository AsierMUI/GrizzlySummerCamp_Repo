using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineByDistance : MonoBehaviour
{
    Renderer rend;
    MaterialPropertyBlock mpb;
    Transform player;
    TrashItem trash;

    bool highlighted;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();

        trash = GetComponentInParent<TrashItem>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            player = playerObj.transform;
        }

        SetHighlight(false);
    }

    private void Update()
    {
        if (player == null || trash == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool shouldHighlight = dist <= trash.OutlineDistance;

        if (shouldHighlight != highlighted) 
        {
            highlighted = shouldHighlight;
            SetHighlight(highlighted);
        }
    }

    void SetHighlight(bool active) 
    {
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Highlight", active ? 1f : 0f);
        rend.SetPropertyBlock(mpb);
    }
}