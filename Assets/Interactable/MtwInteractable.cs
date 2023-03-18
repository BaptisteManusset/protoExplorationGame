using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

[RequireComponent(typeof(Usable))]
public class MtwInteractable : MonoBehaviour
{
    private Usable _usable;

    public bool InInteraction = false;

    private void Awake()
    {
        _usable = GetComponent<Usable>();
        _usable.events.onDeselect.AddListener(OnDeselect);
        _usable.events.onSelect.AddListener(OnSelect);
        _usable.events.onUse.AddListener(OnUse);
    }

    private void OnDestroy()
    {
        _usable.events.onDeselect.RemoveListener(OnDeselect);
        _usable.events.onSelect.RemoveListener(OnSelect);
        _usable.events.onUse.RemoveListener(OnUse);
    }

    private void OnUse()
    {
        if (InInteraction) return;
        Debug.Log("OnUse");
        InInteraction = true;
    }

    private void OnSelect()
    {
        Debug.Log("OnSelect");
    }

    void OnDeselect()
    {
        if (!InInteraction) return;
        Debug.Log("OnDeselect");
        InInteraction = false;
    }
}