using UnityEngine;
using System.Collections.Generic;

public class RandomActivator : MonoBehaviour
{
    //Este script sirve tanto para pesca como para basura

    [System.Serializable]
    public class Zone
    {
        public Transform zoneParent;
        public int minToActivate = 3;
        public int maxToActivate = 3;
    }

    [Header("Zonas del mapa")]
    public List<Zone> zones = new List<Zone>();

    void Start()
    {
        ActivateZones();
    }

    public void ActivateZones()
    {
        foreach (Zone zone in zones)
        {
            if (zone.zoneParent == null) continue;

            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in zone.zoneParent)
            {
                children.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }

            if (children.Count == 0) continue;

            int amount = Random.Range(zone.minToActivate, zone.maxToActivate + 1);
            amount = Mathf.Clamp(amount, 0, children.Count);

            List<GameObject> tempList = new List<GameObject>(children);

            for(int i = 0; i < amount; i++)
            {
                int randomIndex = Random.Range(0, tempList.Count);
                tempList[randomIndex].SetActive(true);
                tempList.RemoveAt(randomIndex);
            }
        }
    }
}