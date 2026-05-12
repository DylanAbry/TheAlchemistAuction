using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardClick : MonoBehaviour
{
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast through pieces and pick the board hit behind them
        RaycastHit[] hits = Physics.RaycastAll(ray, 500f);
        if (hits == null || hits.Length == 0) return;

        // Find the closest hit that is the board
        float bestDist = float.PositiveInfinity;
        RaycastHit bestHit = default;
        bool foundBoard = false;

        foreach (var h in hits)
        {
            if (h.collider != null && h.collider.CompareTag("ChessBoard"))
            {
                if (h.distance < bestDist)
                {
                    bestDist = h.distance;
                    bestHit = h;
                    foundBoard = true;
                }
            }
        }

        if (!foundBoard) return;

        Vector2Int square = BoardMapper.Instance.WorldToBoard(bestHit.point);
        if (square.x < 0) return;

        ChessManager.Instance.TryMove(square.x, square.y);
    }
}
