using UnityEngine;


public class CameraTargetSwitcher : MonoBehaviour
{
    public Transform playerTarget;
    public Transform gliderTarget;


    public enum PlayerState
    {
        Walk,
        Glide
    }

    public PlayerState m_playerState = PlayerState.Walk;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeTarget()
    {
        m_playerState = m_playerState == PlayerState.Glide ? PlayerState.Walk : PlayerState.Glide;
    }
}