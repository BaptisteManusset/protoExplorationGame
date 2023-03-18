using Cinemachine;
using NaughtyAttributes;
using UnityEngine;


public class CameraTargetSwitcher : MonoBehaviour
{
    public Transform playerTarget;
    public Transform playerLookAt;

    public Transform gliderTarget;
    public Transform gliderLookAt;


    public static CameraTargetSwitcher Instance;

    private CinemachineVirtualCamera m_virtualCamera;

    public bool IsWalking => m_playerState == PlayerState.Walk;
    public bool IsGlide => m_playerState == PlayerState.Glide;

    public enum PlayerState
    {
        Walk,
        Glide
    }

    public PlayerState m_playerState = PlayerState.Walk;

    private void Awake()
    {
        Instance = this;
        m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    [Button("change target")]
    public void ChangeTarget()
    {
        m_playerState = m_playerState == PlayerState.Glide ? PlayerState.Walk : PlayerState.Glide;
        m_virtualCamera.Follow = m_playerState == PlayerState.Glide ? gliderTarget : playerTarget;
        m_virtualCamera.LookAt = m_playerState == PlayerState.Glide ? gliderLookAt : playerLookAt;
    }
}