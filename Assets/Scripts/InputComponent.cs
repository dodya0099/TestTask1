using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Enums;

public class InputComponent : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;

    [Header("Spline настройки")]
    public SplineContainer splineContainer; // Контейнер со сплайном
    public float moveSpeed = 5f; // Скорость движения по сплайну

    [Header("Настройки бокового смещения")]
    public float maxSideOffset = 2f; // Максимальное смещение влево и вправо
    public float sideMoveSpeed = 3f; // Скорость бокового перемещения
    public float yOffset = 1f; // Смещение по Y, чтобы персонаж был выше объекта

    [Header("Настройки ротации")]
    public float maxTiltAngle = 15f; // Максимальный угол наклона при смещении влево или вправо
    public float rotationSmoothSpeed = 5f; // Скорость возврата к исходной ротации

    private float splinePosition = 0f; // Положение на сплайне (от 0 до 1)
    private float sideOffset = 0f; // Смещение влево и вправо
    private float currentYRotation = 0f; // Текущий угол поворота по Y

    private Vector2 startTouchPosition; // Начальная позиция свайпа
    private bool isSwipingLeft = false; // Флаг для удерживаемого свайпа влево
    private bool isSwipingRight = false; // Флаг для удерживаемого свайпа вправо

    void Update()
    {

        if (_levelManager.GetGameState() == GameState.play)
        {
            splinePosition += moveSpeed * Time.deltaTime / splineContainer.Spline.GetLength();
            splinePosition = Mathf.Clamp01(splinePosition); // Ограничиваем от 0 до 1, чтобы не выходить за пределы сплайна
            HandleSwipe();
            MoveCharacter();
        }
    }

    void HandleSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Запоминаем начальную позицию свайпа
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    // Определяем направление свайпа, но инвертируем
                    float swipeDeltaX = touch.position.x - startTouchPosition.x;

                    if (swipeDeltaX > 50) // Свайп вправо (инвертирован для движения влево)
                    {
                        isSwipingRight = true; // Устанавливаем флаг свайпа вправо
                        isSwipingLeft = false;  // Сбрасываем флаг свайпа влево
                    }
                    else if (swipeDeltaX < -50) // Свайп влево (инвертирован для движения вправо)
                    {
                        isSwipingRight = false; // Сбрасываем флаг свайпа вправо
                        isSwipingLeft = true;   // Устанавливаем флаг свайпа влево
                    }
                    break;

                case TouchPhase.Ended:
                    // Сбрасываем флаги свайпа при окончании касания
                    isSwipingRight = false;
                    isSwipingLeft = false;
                    break;
            }
        }
    }

    void MoveCharacter()
    {
        // Боковое перемещение
        if (isSwipingRight)
        {
            sideOffset = Mathf.MoveTowards(sideOffset, -maxSideOffset, sideMoveSpeed * Time.deltaTime); // Инвертируем значение
            currentYRotation = maxTiltAngle; // Устанавливаем поворот влево
        }
        else if (isSwipingLeft)
        {
            sideOffset = Mathf.MoveTowards(sideOffset, maxSideOffset, sideMoveSpeed * Time.deltaTime); // Инвертируем значение
            currentYRotation = -maxTiltAngle; // Устанавливаем поворот вправо
        }
        else
        {
            // Возврат к нулевому углу
            currentYRotation = Mathf.LerpAngle(currentYRotation, 0, rotationSmoothSpeed * Time.deltaTime);
        }

        // Позиция и направление по сплайну
        Vector3 splinePositionPoint = splineContainer.Spline.EvaluatePosition(splinePosition);
        float3 splineDirection = splineContainer.Spline.EvaluateTangent(splinePosition);
        splineDirection = math.normalize(splineDirection); // Нормализуем направление с помощью Unity.Mathematics

        // Рассчитываем боковое смещение и конечную позицию
        Vector3 sideOffsetDirection = Vector3.Cross((Vector3)splineDirection, Vector3.up); // Направление бокового смещения
        Vector3 finalPosition = splinePositionPoint + sideOffsetDirection * sideOffset;

        // Добавляем смещение по Y
        finalPosition.y += yOffset;

        // Поворот персонажа с учетом текущего угла
        Quaternion rotation = Quaternion.Euler(0, currentYRotation, 0); // Поворот вокруг оси Y

        // Обновляем позицию и поворот персонажа
        transform.position = finalPosition;
        transform.rotation = Quaternion.LookRotation((Vector3)splineDirection) * rotation;
    }
}
