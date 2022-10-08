using UnityEngine;
using PixelCrushers;

/// <summary>
/// Registers Start Assets' Input System inputs with the Dialogue System.
/// </summary>
public class StarterInputRegistration : MonoBehaviour
{

    /// <summary>
    /// I generated a new C# class for the Starter Assets' Input Action Asset.
    /// </summary>
    private StarterAssets.StarterAssets inputs;

    // Track which instance of this script registered the inputs, to prevent
    // another instance from accidentally unregistering them.
    protected static bool isRegistered = false;
    private bool didIRegister = false;

    void Awake()
    {
        inputs = new StarterAssets.StarterAssets();
    }

    void OnEnable()
    {
        if (!isRegistered)
        {
            isRegistered = true;
            didIRegister = true;
            InputDeviceManager.RegisterInputAction("Jump", inputs.Player.Jump);
        }
    }

    void OnDisable()
    {
        if (didIRegister)
        {
            isRegistered = false;
            didIRegister = false;
            InputDeviceManager.UnregisterInputAction("Jump");
        }
    }

}
