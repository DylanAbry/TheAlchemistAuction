using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int x;
    public int y;

    void OnMouseDown()
    {
        ChessManager.Instance.TryMove(x, y);
    }
}
