using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public struct FrameStruct{
    public Vector2 Move;
    public bool Jump;
}

public class PlayerInputScript : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    private InputAction _move, _jump;

    public FrameStruct FrameStruct { get; private set; }

    private void Awake()
    {
        _playerInputs = new PlayerInputs();

        _move = _playerInputs.Player.Move;
        _jump = _playerInputs.Player.Jump;
    }

    private void OnEnable()
    {
        _playerInputs.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
    }

    private void Update()
    {
        FrameStruct = GatherInput();
    }

    private FrameStruct GatherInput()
    {
        return new FrameStruct{
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame(),
        };
    }
}
