using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Enum] MoveType
 * 플레이어의 이동 방식을 정의합니다.
 */
public enum MoveType
{
	Transform,
	Rididbody_Move
}

/*
 * [Class] KeyboardUI
 * 키보드의 WASD UI를 입력받습니다.
 */
[System.Serializable]
public class KeyboardUI
{
	/*
	 * 직렬화된 클래스 (Serialized Class)
	 * 
	 * 다른 응용 프로그램이나 다른 클래스에서 읽어가기 쉽도록 변수를 공개하고 정렬해두는 것.
	 * 제일 많이 사용한 예시가 [Serializefield]
	 */

	public Image keyW, keyA, keyS, keyD;
}

/*
 * [Class] PlayerController
 * 플레이어의 움직임을 관리합니다.
 */
public class PlayerController : MonoBehaviour
{
	// 키보드 UI 이미지
	public KeyboardUI keyboard;

	// 현재 움직임을 설정하는 UI
	public Dropdown dropMove;

	// 현재 플레이어의 좌표계 설정하는 UI
	public Dropdown dropSpace;

	// 마우스로 화면 회전 설정 UI
	public Toggle toggleMouse;

	// 방향키로 화면 회전 설정 UI
	public Toggle toggleKey;

	// Transform 움직임 속도
	public float transformSpeed = 3f;

	// Rigidbody 움직임 속도
	public float rigidbodySpeed = 3f;

	// 화면 회전 속도
	public float rotationSpeed = 5f;

	// 현재 움직임을 컨트롤하는 방법
	private MoveType moveType = MoveType.Transform;

	// 현재 플레이어의 좌표계(World/Self)
	private Space space = Space.World;

	// 마우스로 화면을 회전할지
	private bool allowMouseRotation = true;

	// 방향키로 화면을 회전할지
	private bool allowKeyRotation = false;

	// 플레이어 GameObject에 붙어있는 Rigidbody
	private Rigidbody rig;

	private void Start()
	{
		rig = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		showPressedKey();

		// 수평(Horizontal, x축, 좌우) 방향에 대한 *키보드* 입력값
		float h = Input.GetAxis("Horizontal");

		// 수직(Vertical, z축, 앞뒤) 방향에 대한 *키보드* 입력값
		float v = Input.GetAxis("Vertical");

		// with NO SMOOTHING (rotationSpeed로 제어함)
		float mouseX = Input.GetAxisRaw("Mouse X");

		switch (moveType)
		{
			case MoveType.Transform:
				transform.Translate(new Vector3(h * transformSpeed * Time.deltaTime, 0, v * transformSpeed * Time.deltaTime), space/*default: Self*/);

				if (allowKeyRotation)
				{
					transform.Rotate(new Vector3(0f, h * rotationSpeed, 0f));
				}

				if (allowMouseRotation)
				{
					transform.Rotate(new Vector3(0f, mouseX, 0f) * rotationSpeed);
				}

				break;
			case MoveType.Rididbody_Move:
				Vector3 moveDir = new Vector3(h * rigidbodySpeed * Time.deltaTime, 0f, v * rigidbodySpeed * Time.deltaTime);

				if (space == Space.World)
				{
					rig.MovePosition(rig.position + moveDir);
				}
				else
				{
					rig.MovePosition(rig.position + transform.TransformDirection(moveDir));
				}

				if (allowKeyRotation)
				{
					rig.MoveRotation(rig.rotation * Quaternion.Euler(0f, h * rotationSpeed, 0f));
				}

				if (allowMouseRotation)
				{
					rig.MoveRotation(rig.rotation * Quaternion.Euler(0f, mouseX * rotationSpeed, 0f));
				}

				break;
		}
	}

	/*
	 * [Method] showPressedKey(): void
	 * 현재 눌린 키를 표시합니다.
	 */
	private void showPressedKey()
	{
		Color pressedColor = new Color(0.1176471f, 0.1176471f, 0.1176471f, 0.3921569f);
		Color unpressedColor = new Color(1f, 1f, 1f, 0.3921569f);

		if (Input.GetKeyDown(KeyCode.W))
		{
			keyboard.keyW.color = pressedColor;
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			keyboard.keyA.color = pressedColor;
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			keyboard.keyS.color = pressedColor;
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			keyboard.keyD.color = pressedColor;
		}

		if (Input.GetKeyUp(KeyCode.W))
		{
			keyboard.keyW.color = unpressedColor;
		}

		if (Input.GetKeyUp(KeyCode.A))
		{
			keyboard.keyA.color = unpressedColor;
		}

		if (Input.GetKeyUp(KeyCode.S))
		{
			keyboard.keyS.color = unpressedColor;
		}

		if (Input.GetKeyUp(KeyCode.D))
		{
			keyboard.keyD.color = unpressedColor;
		}
	}

	/*
	 * [Method] OnMoveTypeValueChanged(): void
	 * Move Control 드롭다운 값이 변경되었을 때 호출됩니다.
	 */
	public void OnMoveTypeValueChanged()
	{
		moveType = (MoveType)dropMove.value;
	}

	/*
	 * [Method] OnSpaceValueChanged(): void
	 * World Space 드롭다운 값이 변경되었을 때 호출됩니다.
	 */
	public void OnSpaceValueChanged()
	{
		space = (Space)dropSpace.value;
	}

	/*
	 * [Method] OnMouseRotationValueChanged(): void
	 * Mouse Rot. 토글 값이 변경되었을 때 호출됩니다.
	 */
	public void OnMouseRotationValueChanged()
	{
		allowMouseRotation = toggleMouse.isOn;
	}

	/*
	 * [Method] OnKeyRotationValueChanged(): void
	 * Key Rot. 토글 값이 변경되었을 때 호출됩니다.
	 */
	public void OnKeyRotationValueChanged()
	{
		allowKeyRotation = toggleKey.isOn;
	}
}
