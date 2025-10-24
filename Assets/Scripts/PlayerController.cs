using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5f;

	private InputSystem_Actions _inputActions;
	private CharacterController _characterController;

	void Awake()
	{
		TryGetComponent(out _characterController);
		_inputActions = InputManager.Controls;
	}

	void OnEnable()
	{
		_inputActions.Enable();
	}

	void OnDisable()
	{
		_inputActions.Disable();
	}

	void Update()
	{
		var move = _inputActions.Player.Move.ReadValue<Vector2>();
		Vector3 movement = new Vector3(move.x, 0, move.y) * moveSpeed * Time.deltaTime;
		_characterController.Move(movement);
	}
}
