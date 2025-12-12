using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCheckTarget : MonoBehaviour
{
    private FishingSkillCheckManager manager;

    public void Initialize(FishingSkillCheckManager m)
    {
        manager = m;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.HitSuccess();
        Destroy(gameObject);
    }
}
