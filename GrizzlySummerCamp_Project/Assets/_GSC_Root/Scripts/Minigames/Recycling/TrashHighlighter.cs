using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class TrashHighlighter : MonoBehaviour
{
    [SerializeField] private Color emissionColor = Color.green;
    [SerializeField] private float emissionIntensity = 1.5f;

    private Material mat;
    private Transform player;
    private TrashItem trash;
    private TrashPlayerCarry carry;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        trash = GetComponent<TrashItem>();

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            player = playerObj.transform;
            carry = playerObj.GetComponent<TrashPlayerCarry>();
        }
    }

    private void Update()
    {
        if (player == null || trash == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool inRange = dist <= trash.InteractionDistance;

        if (inRange)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }
        else 
        {
            mat.SetColor("_EmissionColor", Color.black);
        }
    }
}
