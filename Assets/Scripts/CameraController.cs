using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    [Header("Настройки камеры")]
    public Transform target; // Объект персонажа
    public Vector3 offset = new Vector3(0, 5, -10); // Смещение камеры относительно персонажа
    public float smoothSpeed = 0.125f; // Скорость плавности

    [Header("Настройки направления")]
    public float lookAheadDistance = 2f; // Смещение вперед от позиции персонажа для направления камеры

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Не установлен объект персонажа для слежения.");
            return;
        }

        // Вычисляем позицию камеры позади персонажа, основываясь на его направлении
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        
        // Плавно перемещаем камеру к желаемой позиции
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Вычисляем точку перед персонажем, на которую будет смотреть камера
        Vector3 lookTargetPosition = target.position + target.forward * lookAheadDistance;

        // Поворачиваем камеру, чтобы она была направлена чуть впереди персонажа
        transform.LookAt(lookTargetPosition);
    }
}
