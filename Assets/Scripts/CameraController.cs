using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Настройки камеры")]
    public Transform target; // Объект персонажа
    public Vector3 offset = new Vector3(0, 5, -10); // Смещение камеры относительно персонажа
    public float smoothSpeed = 0.125f; // Скорость плавности

    [Header("Настройки направления")]
    public float lookDownAngle = 2f; // Смещение вниз по оси Y для имитации наклона камеры

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Не установлен объект персонажа для слежения.");
            return;
        }

        // Вычисляем позицию камеры позади персонажа на основе его направления
        Vector3 desiredPosition = target.position - target.forward * offset.z + Vector3.up * offset.y;

        // Плавно перемещаем камеру к желаемой позиции
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Вычисляем точку, на которую будет смотреть камера, добавляя наклон вниз
        Vector3 lookTargetPosition = target.position + Vector3.up * lookDownAngle;

        // Камера смотрит на смещенную точку, создавая эффект наклона
        transform.LookAt(lookTargetPosition);
    }
}
