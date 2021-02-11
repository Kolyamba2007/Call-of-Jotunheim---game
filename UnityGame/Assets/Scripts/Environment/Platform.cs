using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[ExecuteAlways]
[Serializable]
public class Platform : MonoBehaviour
{
    public enum OrbitType { NONE, LINE, ELLIPSE, RECTANGLE, SQUARE }
    public OrbitType Orbit = OrbitType.LINE;
    public enum OrbitOrientationType { CLOCKWISE, COUNTERCLOCKWISE }
    public OrbitOrientationType OrbitOrientation;
    public float MovementSpeed;
    public Vector3 StartPosition;

    private List<Unit> AttachedUnits = new List<Unit>();

    public List<Vector2> Points = new List<Vector2>();

    /* ========= ПРЯМАЯ ========= */
    public enum LineOrbitType { HORIZONTAL, VERTICAL }
    public LineOrbitType LineOrbit;
    public float LineOrbitDistance;

    /* ========= ЭЛЛИПС ========= */
    public float EllipseBigRadius;
    public float EllipseSmallRadius;

    /* ========= ПРЯМОУГОЛЬНИК ========= */
    private int RectangleIndex;
    public float RectangleBigSize;
    public float RectangleSmallSize;

    /* ========= КВАДРАТ ========= */
    public float SquareSize;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            switch (Orbit)
            {
                case OrbitType.LINE:
                    if (LineOrbitDistance > 0 && MovementSpeed > 0) StartCoroutine(LineCoroutine());
                    break;
                case OrbitType.ELLIPSE:
                    if (EllipseBigRadius > 0 && EllipseSmallRadius > 0 && MovementSpeed > 0) StartCoroutine(EllipseCoroutine());
                    break;
                case OrbitType.RECTANGLE:
                    if (RectangleBigSize > 0 && RectangleSmallSize > 0 && MovementSpeed > 0) StartCoroutine(RectangleCoroutine());
                    break;
                case OrbitType.SQUARE:
                    if (SquareSize > 0 && MovementSpeed > 0) StartCoroutine(RectangleCoroutine());
                    break;
            }
        }
    }
    private void LateUpdate()
    {
        if (!Application.isPlaying) StartPosition = transform.position;
        switch (Orbit)
        {
            case OrbitType.LINE:
                switch (LineOrbit)
                {
                    case LineOrbitType.HORIZONTAL:
                        Debug.DrawLine(StartPosition + new Vector3(-LineOrbitDistance, 0, 0), StartPosition + new Vector3(LineOrbitDistance, 0, 0), UnityEngine.Color.yellow);
                        break;
                    case LineOrbitType.VERTICAL:
                        Debug.DrawLine(StartPosition + new Vector3(0, -LineOrbitDistance, 0), StartPosition + new Vector3(0, LineOrbitDistance, 0), UnityEngine.Color.yellow);
                        break;
                }
                break;
            case OrbitType.RECTANGLE:
                Debug.DrawLine(StartPosition, StartPosition + new Vector3(RectangleBigSize, 0, 0), UnityEngine.Color.yellow);
                Debug.DrawLine(StartPosition + new Vector3(RectangleBigSize, 0, 0), StartPosition + new Vector3(RectangleBigSize, 0, 0) + new Vector3(0, -RectangleSmallSize, 0), UnityEngine.Color.yellow);
                Debug.DrawLine(StartPosition + new Vector3(0, -RectangleSmallSize, 0), StartPosition + new Vector3(RectangleBigSize, 0, 0) + new Vector3(0, -RectangleSmallSize, 0), UnityEngine.Color.yellow);
                Debug.DrawLine(StartPosition, StartPosition + new Vector3(0, -RectangleSmallSize, 0), UnityEngine.Color.yellow);
                break;
            case OrbitType.ELLIPSE:
                int segments = 100;
                Vector3 start = StartPosition + new Vector3(EllipseBigRadius, 0, 0);
                for (int i = 0; i < segments; i++)
                {
                    float angle = ((float)i / segments) * 360 * Mathf.Deg2Rad;
                    float x = start.x + Mathf.Sin(angle) * EllipseBigRadius;
                    float y = start.y + Mathf.Cos(angle) * EllipseSmallRadius;

                    float next_angle = ((float)(i + 1) / segments) * 360 * Mathf.Deg2Rad;
                    float next_x = start.x + Mathf.Sin(next_angle) * EllipseBigRadius;
                    float next_y = start.y + Mathf.Cos(next_angle) * EllipseSmallRadius;
                    Debug.DrawLine(new Vector3(x, y, 0), new Vector3(next_x, next_y, 0), UnityEngine.Color.yellow);
                }
                break;
            case OrbitType.SQUARE:
                Debug.DrawLine(StartPosition, StartPosition + new Vector3(SquareSize, 0, 0), UnityEngine.Color.yellow);
                Debug.DrawLine(StartPosition + new Vector3(SquareSize, 0, 0), StartPosition + new Vector3(SquareSize, 0, 0) + new Vector3(0, -SquareSize, 0), UnityEngine.Color.yellow);
                Debug.DrawLine(StartPosition + new Vector3(0, -SquareSize, 0), StartPosition + new Vector3(SquareSize, 0, 0) + new Vector3(0, -SquareSize, 0), UnityEngine.Color.yellow);
                Debug.DrawLine(StartPosition, StartPosition + new Vector3(0, -SquareSize, 0), UnityEngine.Color.yellow);
                break;
        }
    }

    private IEnumerator LineCoroutine()
    {        
        Vector2 target = Points[0];
        Vector3 start = transform.position;
        while (true)        
        {
            float startTime = Time.time;
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), target);
            while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), target) > 0)
            {
                float elapsedTime = Time.time - startTime;
                transform.position = Vector3.Lerp(start, target, (elapsedTime * MovementSpeed) / distance);
                yield return null;
            }
            start = transform.position;
            if (target == Points[0]) target = Points[1];
            else if (target == Points[1]) target = Points[0];
            yield return null;
        }
    }
    private IEnumerator EllipseCoroutine()
    {
        Vector3 start = transform.position + new Vector3(EllipseBigRadius, 0, 0);
        while (true)
        {
            int segments = (int)(100 * (1 / MovementSpeed));
            float x, y, angle = 0;
            for (int i = 0; i < segments; i++)
            {                
                switch (OrbitOrientation)
                {
                    case OrbitOrientationType.CLOCKWISE:
                        angle = ((float)i / segments) * 360 * Mathf.Deg2Rad;       
                        break;
                    case OrbitOrientationType.COUNTERCLOCKWISE:
                        angle = ((float)-i / segments) * 360 * Mathf.Deg2Rad;                        
                        break;
                }
                x = start.x + Mathf.Sin(angle) * EllipseBigRadius;
                y = start.y + Mathf.Cos(angle) * EllipseSmallRadius;
                transform.position = new Vector3(x, y, transform.position.z);
                yield return null;
            }
        }
    }
    private IEnumerator RectangleCoroutine()
    {
        switch (OrbitOrientation)
        {
            case OrbitOrientationType.CLOCKWISE:
                RectangleIndex = 1;
                break;
            case OrbitOrientationType.COUNTERCLOCKWISE:
                RectangleIndex = 3;
                break;
        }
        while (true)
        {
            Vector3 start = transform.position;
            Vector3 target = Points[RectangleIndex];
            float startTime = Time.time;
            float distance = Vector3.Distance(transform.position, target);
            while (Vector3.Distance(transform.position, target) > 0)
            {
                float elapsedTime = Time.time - startTime;               
                transform.position = Vector3.Lerp(start, target, elapsedTime * MovementSpeed / distance);
                yield return null;
            }
            switch (OrbitOrientation)
            {
                case OrbitOrientationType.CLOCKWISE:
                    if (RectangleIndex < 3) RectangleIndex++;
                    else RectangleIndex = 0;
                    break;
                case OrbitOrientationType.COUNTERCLOCKWISE:
                    if (RectangleIndex > 0) RectangleIndex--;
                    else RectangleIndex = 3;
                    break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>())
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal == Vector2.down)
                {
                    AttachedUnits.Add(collision.gameObject.GetComponent<Unit>());
                    collision.gameObject.GetComponent<Unit>().transform.SetParent(transform);
                    break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>())
        {
            foreach (Unit unit in AttachedUnits)
            {
                if (unit == collision.gameObject.GetComponent<Unit>())
                {
                    AttachedUnits.Remove(collision.gameObject.GetComponent<Unit>());
                    collision.gameObject.GetComponent<Unit>().transform.SetParent(GameObject.Find("Units").transform);
                    break;
                }
            }
        }
    }
}
