using UnityEngine;
using System.Collections.Generic;

public class TrashCarryVisuals : MonoBehaviour
{
    [Header("Holding Point")]
    [SerializeField] private Transform holdPoint;

    [Header("Visual Prefabs")]
    [SerializeField] private GameObject organicaPrefab;
    [SerializeField] private GameObject papelPrefab;
    [SerializeField] private GameObject plasticoPrefab;


    private GameObject currentVisual;

    Dictionary<TrashType, GameObject> prefabByType;

    private void Awake()
    {
        prefabByType = new Dictionary<TrashType, GameObject>
             {
            { TrashType.Organica, organicaPrefab },
            { TrashType.Papel, papelPrefab },
            { TrashType.Plastico, plasticoPrefab },
        };
    }

    private void OnEnable()
    {
        TrashPlayerCarry.OnCarryChanged += OnCarryChanged;
    }

    private void OnDisable()
    {
        TrashPlayerCarry.OnCarryChanged -= OnCarryChanged;
    }

    void OnCarryChanged(TrashType? type) 
    {
        ClearVisual();

        if (!type.HasValue) return;

        if (!prefabByType.TryGetValue(type.Value, out GameObject prefab)) return;

        if (prefab == null || holdPoint == null) return;

        currentVisual = Instantiate(prefab, holdPoint);
        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;
    }

    void ClearVisual() 
    {
        if (currentVisual != null)
        {
            Destroy(currentVisual);
        }
    }

}
