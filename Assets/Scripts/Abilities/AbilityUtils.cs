using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Pathfinding.Util.RetainedGizmos;

public static class AbilityUtils
{

    public static Vector2 GetFacingDirection(Vector3 CasterPosition)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 playerPosition = CasterPosition;

        // Get the direction from the player to the mouse
        Vector2 direction = mousePosition - playerPosition;

        // Get the angle of the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Get the facing direction based on the angle
        Vector2 facingDirection;
        if (angle > -45f && angle <= 45f)
        {
            return facingDirection = Vector2.right; // Right
        }
        else if (angle > 45f && angle <= 135f)
        {
            return facingDirection = Vector2.up; // Up
        }
        else if (angle > 135f || angle <= -135f)
        {
            return facingDirection = Vector2.left; // Left
        }
        else
        {
            return facingDirection = Vector2.down; // Down
        }
    }
    public static float GetLeftOrRightDirection(Vector3 CasterPosition)
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 playerPosition = CasterPosition;

        // Get the direction from the player to the mouse
        Vector2 direction = mousePosition - playerPosition;

        // Get the angle of the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Check if the mouse is to the left or right of the given position
        if (angle > 90f || angle < -90f)
        {
            return -1f; // Left
        }
        else
        {
            return 1f; // Right
        }
    }

    public static Vector3 GetClosestPointToMouse(Vector3 CasterPosition, Vector3 spawnPoint, float distanceFromCaster = -0.2f)
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - CasterPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        Vector3 closestPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * distanceFromCaster;
        closestPoint += spawnPoint;

        return closestPoint;
    }

    public static Vector3 GetOffsetMousePosition(Vector2 offset) 
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, 0);
    }
    public static Vector3 GetMousePosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return new Vector3(mousePos.x, mousePos.y, 0);
    }
    public static float GetFiringArcIncrement(float index, float arc, int count)
    {
        if (count == 1)
            return 0;

        return index * arc / (count - 1);

    }
}
