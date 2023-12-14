using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float runningMovementSpeed { get; private set; } = 6f;
    public float walkingMovementSpeed { get; private set; } = 3f;
    public float crouchingMovementSpeed { get; private set; } = 2f;
    public float gravity { get; private set; } = -21f;
    public float jumpHeight { get; private set; } = 1.5f;
    public float standingHeightY = 2f;
    public float crouchingHeightY = 0.5f;
    public float crouchTransitionSpeed = 10f;
}
