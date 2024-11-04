using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Enums;

public class InputComponent : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameObject _playerBody;

    [Header("Spline настройки")]
    public SplineContainer splineContainer;
    public float moveSpeed = 5f;
    public float stopDistance = 0.98f;

    [Header("Настройки бокового смещения")]
    public float maxSideOffset = 2f;
    public float sideMoveSpeed = 3f;
    public float yOffset = 1f;

    [Header("Настройки ротации")]
    public float maxTiltAngle = 15f;
    public float rotationSmoothSpeed = 5f;

    private float splinePosition = 0f;
    private float sideOffset = 0f;
    private float currentYRotation = 0f;

    private Vector2 startTouchPosition;
    private bool isSwipingLeft = false;
    private bool isSwipingRight = false;
    private bool _isInputEnabled = true;

    void Update()
    {
        if (_levelManager.GetGameState() == GameState.play && _isInputEnabled)
        {
            splinePosition += moveSpeed * Time.deltaTime / splineContainer.Spline.GetLength();
            splinePosition = Mathf.Clamp01(splinePosition);

            if (splinePosition >= stopDistance)
            {
                _levelManager.SetGameState(GameState.win);
                _isInputEnabled = false;
                return;
            }

            HandleSwipe();
            MoveCharacter();
        }
    }

    void HandleSwipe()
    {
        if (Input.touchCount > 0 && _isInputEnabled)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    float swipeDeltaX = touch.position.x - startTouchPosition.x;

                    if (swipeDeltaX > 50)
                    {
                        isSwipingRight = true;
                        isSwipingLeft = false;
                    }
                    else if (swipeDeltaX < -50)
                    {
                        isSwipingRight = false;
                        isSwipingLeft = true;
                    }
                    break;

                case TouchPhase.Ended:
                    isSwipingRight = false;
                    isSwipingLeft = false;
                    break;
            }
        }
    }

    void MoveCharacter()
    {
        if (isSwipingRight)
        {
            sideOffset = Mathf.MoveTowards(sideOffset, -maxSideOffset, sideMoveSpeed * Time.deltaTime);
            currentYRotation = maxTiltAngle;
        }
        else if (isSwipingLeft)
        {
            sideOffset = Mathf.MoveTowards(sideOffset, maxSideOffset, sideMoveSpeed * Time.deltaTime);
            currentYRotation = -maxTiltAngle;
        }
        else
        {
            currentYRotation = Mathf.LerpAngle(currentYRotation, 0, rotationSmoothSpeed * Time.deltaTime);
        }

        Vector3 splinePositionPoint = splineContainer.Spline.EvaluatePosition(splinePosition);
        float3 splineDirection = splineContainer.Spline.EvaluateTangent(splinePosition);
        splineDirection = math.normalize(splineDirection);

        Vector3 sideOffsetDirection = Vector3.Cross((Vector3)splineDirection, Vector3.up);
        Vector3 finalPosition = splinePositionPoint + sideOffsetDirection * sideOffset;
        finalPosition.y += yOffset;

        transform.position = finalPosition;
        transform.rotation = Quaternion.LookRotation((Vector3)splineDirection);

        Quaternion tiltRotation = Quaternion.Euler(0, currentYRotation, 0);
        _playerBody.transform.localRotation = tiltRotation;
    }
}
