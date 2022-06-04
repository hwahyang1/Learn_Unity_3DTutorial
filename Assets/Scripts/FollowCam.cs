using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] FollowCam
 * 카메라가 플레이어를 따라가도록 처리합니다.
 */
public class FollowCam : MonoBehaviour
{
	public Transform target;

	public float distance = 10.0f;
	public float height = 5.0f;
	public float rotateSpeed = 5.0f;

	/* 앞으로 3인칭 카메라 할 때 그냥 복붙해도 되긴 함 */
	private void LateUpdate()
	{
		/*
		 * [Note: Method]
		 * Mathf.LerpAngle(float a, float b, float t)
		 * 수의 선형 보간(수가 시간을 가지고 변경되는 것)을 처리합니다.
		 * 
		 * <float a>
		 * 시작 값을 입력합니다.
		 * 
		 * <float b>
		 * 목표 값을 입력합니다.
		 * 
		 * <float t>
		 * 목표 값까지의 시간을 입력합니다.
		 */
		float angle = Mathf.LerpAngle(transform.eulerAngles.y, target.eulerAngles.y, rotateSpeed * Time.deltaTime);

		// 오일러 각도(+0 ~ +360) -> 쿼터니언 각도(-90 ~ +90) 변환
		Quaternion rotation = Quaternion.Euler(0, angle, 0);

		transform.position = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

		transform.LookAt(target);
	}
}
