using PixelCrushers.DialogueSystem.Wrappers;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableSphere : MonoBehaviour
{
    private Usable m_usable;

    [SerializeField] private bool m_state = false;

    private void Awake()
    {
        m_usable = GetComponent<Usable>();
    }
}