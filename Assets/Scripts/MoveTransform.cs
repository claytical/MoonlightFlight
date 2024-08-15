using UnityEngine;
using System.Collections;

public class MoveTransform : MonoBehaviour
{
    public enum VerticalDirection { Up, Down }
    public enum HorizontalDirection { Left, Right }

    [Header("Up/Down Movement Settings")]
    public bool enableVerticalMovement = true;
    public VerticalDirection verticalStartDirection = VerticalDirection.Up;
    public float verticalAmount = 5.0f;
    public float verticalSpeed = 2.0f;
    public float verticalPauseDuration = 1.0f; // Pause duration in seconds

    [Header("Left/Right Movement Settings")]
    public bool enableHorizontalMovement = true;
    public HorizontalDirection horizontalStartDirection = HorizontalDirection.Right;
    public float horizontalAmount = 5.0f;
    public float horizontalSpeed = 2.0f;
    public float horizontalPauseDuration = 1.0f; // Pause duration in seconds

    private Vector3 initialPosition;
    private Vector3 verticalTargetPosition;
    private Vector3 horizontalTargetPosition;
    private bool isPaused = false;

    private void Start()
    {
        initialPosition = transform.position;
        SetInitialTargetPositions();
    }

    private void Update()
    {
        if (!isPaused)
        {
            MoveVertical();
            MoveHorizontal();
        }
    }

    private void SetInitialTargetPositions()
    {
        // Set initial target positions based on the starting direction
        if (enableVerticalMovement)
        {
            float verticalOffset = verticalAmount * (verticalStartDirection == VerticalDirection.Up ? 1 : -1);
            verticalTargetPosition = initialPosition + new Vector3(0, verticalOffset, 0);
        }

        if (enableHorizontalMovement)
        {
            float horizontalOffset = horizontalAmount * (horizontalStartDirection == HorizontalDirection.Right ? 1 : -1);
            horizontalTargetPosition = initialPosition + new Vector3(horizontalOffset, 0, 0);
        }
    }

    private void MoveVertical()
    {
        if (enableVerticalMovement)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.MoveTowards(transform.position.y, verticalTargetPosition.y, verticalSpeed * Time.deltaTime),
                transform.position.z
            );

            if (Mathf.Approximately(transform.position.y, verticalTargetPosition.y))
            {
                StartCoroutine(PauseVerticalMovement());
            }
        }
    }

    private void MoveHorizontal()
    {
        if (enableHorizontalMovement)
        {
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, horizontalTargetPosition.x, horizontalSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );

            if (Mathf.Approximately(transform.position.x, horizontalTargetPosition.x))
            {
                StartCoroutine(PauseHorizontalMovement());
            }
        }
    }

    private IEnumerator PauseVerticalMovement()
    {
        isPaused = true;
        yield return new WaitForSeconds(verticalPauseDuration);

        // Swap between the initial position and the target position
        verticalTargetPosition = verticalTargetPosition == initialPosition
            ? initialPosition + new Vector3(0, verticalAmount * (verticalStartDirection == VerticalDirection.Up ? 1 : -1), 0)
            : initialPosition;

        isPaused = false;
    }

    private IEnumerator PauseHorizontalMovement()
    {
        isPaused = true;
        yield return new WaitForSeconds(horizontalPauseDuration);

        // Swap between the initial position and the target position
        horizontalTargetPosition = horizontalTargetPosition == initialPosition
            ? initialPosition + new Vector3(horizontalAmount * (horizontalStartDirection == HorizontalDirection.Right ? 1 : -1), 0, 0)
            : initialPosition;

        isPaused = false;
    }

    // Method to adjust vertical speed dynamically
    public void SetVerticalSpeed(float newSpeed)
    {
        verticalSpeed = newSpeed;
    }

    // Method to adjust horizontal speed dynamically
    public void SetHorizontalSpeed(float newSpeed)
    {
        horizontalSpeed = newSpeed;
    }
}
