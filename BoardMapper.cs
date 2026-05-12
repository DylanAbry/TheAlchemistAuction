using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMapper : MonoBehaviour
{
    public static BoardMapper Instance;

    [Header("Board Settings")]
    public Transform boardTransform;

    public float boardSize = 8f; // Total width
    [SerializeField] private float squareSize = 1.25f;
    public float SquareSize => squareSize;
    [SerializeField] Vector3 bottomLeftOffset = new(-0.6f, 0, -0.6f);

    void Awake()
    {
        Instance = this;
    }

    public Vector2Int WorldToBoard(Vector3 hitPoint)
    {
        Vector3 local =
        boardTransform.InverseTransformPoint(hitPoint);

        local -= bottomLeftOffset;

        int x = Mathf.FloorToInt(local.x / squareSize);
        int y = Mathf.FloorToInt(local.z / squareSize);

        // FLIP BOTH AXES
        x = 7 - x;
        y = 7 - y;

        if (x < 0 || x > 7 || y < 0 || y > 7)
            return new Vector2Int(-1, -1);

        return new Vector2Int(x, y);
    }

    public Vector3 BoardToWorld(int x, int y)
    {
        // Flip back
        int fx = 7 - x;
        int fy = 7 - y;

        Vector3 pos = new Vector3(
            fx * squareSize + squareSize / 2,
            0,
            fy * squareSize + squareSize / 2
        );

        pos += bottomLeftOffset;

        return boardTransform.TransformPoint(pos);
    }

    void OnDrawGizmos()
    {
        if (boardTransform == null) return;

        Gizmos.color = Color.green;

        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                Vector3 pos = BoardToWorld(x, y);
                Gizmos.DrawSphere(pos, 0.25f);
            }
    }
}
