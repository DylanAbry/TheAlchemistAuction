using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChessManager : MonoBehaviour
{
    public static ChessManager Instance;

    public enum ChessGameState
    {
        Inactive,
        Intro,
        WhiteTurn,
        WhiteMoved,
        BlackTurn,
        BlackMoved,
        Promotion,
        Checkmate,
        Stalemate,
        TimeUp
    }

    [Header("Turn panels")]
    public GameObject whiteTurnPanel;
    public GameObject blackTurnPanel;

    [Header("Check panels")]
    public GameObject whiteInCheckPanel;
    public GameObject blackInCheckPanel;

    [Header("End Turn buttons")]
    public GameObject whiteEndTurnButton;
    public GameObject blackEndTurnButton;

    [Header("Timers")]
    public float turnTime = 30f;
    float whiteTimer;
    float blackTimer;
    public TextMeshProUGUI whiteTimerText;
    public TextMeshProUGUI blackTimerText;

    public GameObject whiteTimerPanel;
    public GameObject blackTimerPanel;

    [Header("Times Up")]
    public GameObject whiteTimesUpPanel;
    public GameObject blackTimesUpPanel;

    [Header("End states")]
    public GameObject whiteCheckmatePanel;
    public GameObject blackCheckmatePanel;
    public GameObject stalematePanel;

    [Header("Pawn Promotion")]
    public GameObject blackPromotionPanel;
    public GameObject whitePromotionPanel;

    public GameObject whiteQueenPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteKnightPrefab;

    public GameObject blackQueenPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackKnightPrefab;

    [Header("Board")]
    public ChessBoard board = new ChessBoard();
    public ChessPiece selectedPiece;
    public PieceColor currentTurn = PieceColor.White;

    [Header("Spawn")]
    public float promotedPieceScale = 3f;

    Pawn pawnPendingPromotion;
    bool awaitingPromotion = false;

    public CompletionistInteraction compScript;
    public GameObject exitButton;
    public GameObject rageQuitButton;

    public AudioSource applause;
    public AudioSource chessClick;
    bool applausePlayed;

    readonly List<GameObject> debugMarkers = new();
    ChessGameState gameState = ChessGameState.Inactive;

    class PieceSnapshot
    {
        public GameObject prefab;
        public PieceColor color;
        public Vector2Int position;
    }
    List<PieceSnapshot> startingLayout = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Default UI off
        SetAllPanelsOff();
        RegisterAllPieces();

        // Start session as inactive until CompletionistInteraction tells us to start
        gameState = ChessGameState.Inactive;
        exitButton.SetActive(false);
        rageQuitButton.SetActive(false);
        applausePlayed = false;
    }

    // Called by CompletionistInteraction when entering chess mode
    public void StartChessSession()
    {
        SetAllPanelsOff();
        awaitingPromotion = false;
        selectedPiece = null;
        ClearHighlights();
        whiteTurnPanel.SetActive(true);

        compScript.chessIntroPanel.SetActive(false);
        rageQuitButton.SetActive(true);

        // Start at White turn by default
        StartWhiteTurn();
        Debug.Log(gameState);
    }

    void SetAllPanelsOff()
    {
        if (whiteTurnPanel) whiteTurnPanel.SetActive(false);
        if (blackTurnPanel) blackTurnPanel.SetActive(false);
        if (whiteInCheckPanel) whiteInCheckPanel.SetActive(false);
        if (blackInCheckPanel) blackInCheckPanel.SetActive(false);

        if (whiteEndTurnButton) whiteEndTurnButton.SetActive(false);
        if (blackEndTurnButton) blackEndTurnButton.SetActive(false);

        if (whitePromotionPanel) whitePromotionPanel.SetActive(false);
        if (blackPromotionPanel) blackPromotionPanel.SetActive(false);

        if (whiteTimesUpPanel) whiteTimesUpPanel.SetActive(false);
        if (blackTimesUpPanel) blackTimesUpPanel.SetActive(false);

        if (whiteCheckmatePanel) whiteCheckmatePanel.SetActive(false);
        if (blackCheckmatePanel) blackCheckmatePanel.SetActive(false);
        if (stalematePanel) stalematePanel.SetActive(false);
    }

    void RegisterAllPieces()
    {
        ChessPiece[] pieces = FindObjectsOfType<ChessPiece>();

        foreach (ChessPiece p in pieces)
        {
            Vector2Int sq = BoardMapper.Instance.WorldToBoard(p.transform.position);

            if (sq.x >= 0)
            {
                RegisterPiece(p, sq.x, sq.y);

                startingLayout.Add(new PieceSnapshot
                {
                    prefab = p.prefabReference,
                    color = p.color,
                    position = sq
                });
            }
            else
            {
                Debug.LogError("Piece off board: " + p.name);
            }
        }
    }

    public void RegisterPiece(ChessPiece piece, int x, int y)
    {
        piece.SetPosition(x, y);
        board.grid[x, y] = piece;
    }

    // -------------------- Turn Control --------------------

    void StartWhiteTurn()
    {
        gameState = ChessGameState.WhiteTurn;
        currentTurn = PieceColor.White;
        whiteTimer = turnTime;
        ShowWhiteTurn();
        if (whiteTimerText)
            whiteTimerText.text = Mathf.Ceil(whiteTimer).ToString();

        whiteTimerPanel.SetActive(true);
        blackTimerPanel.SetActive(false);

        whiteEndTurnButton.SetActive(false);
        blackEndTurnButton.SetActive(false);

        UpdateCheckUI();
    }

    void StartBlackTurn()
    {
        gameState = ChessGameState.BlackTurn;
        currentTurn = PieceColor.Black;
        blackTimer = turnTime;

        ShowBlackTurn();

        if (blackTimerText)
            blackTimerText.text = Mathf.Ceil(blackTimer).ToString();

        blackTimerPanel.SetActive(true);
        whiteTimerPanel.SetActive(false);

        whiteEndTurnButton.SetActive(false);
        blackEndTurnButton.SetActive(false);

        UpdateCheckUI();
    }

    void ShowWhiteTurn()
    {
        if (whiteTurnPanel) whiteTurnPanel.SetActive(true);
        whiteTimerPanel.SetActive(true);
        if (blackTurnPanel) blackTurnPanel.SetActive(false);
    }

    void ShowBlackTurn()
    {
        if (blackTurnPanel) blackTurnPanel.SetActive(true);
        blackTimerPanel.SetActive(true);
        if (whiteTurnPanel) whiteTurnPanel.SetActive(false);
    }

    public void WhiteEndTurn()
    {
        if (gameState != ChessGameState.WhiteMoved) return;

        Debug.Log("White End Turn pressed");
        // Evaluate game end for BLACK (since it becomes black's turn)
        EvaluateGameEndForNextPlayer(PieceColor.Black);

        if (gameState == ChessGameState.Checkmate || gameState == ChessGameState.Stalemate || gameState == ChessGameState.TimeUp)
            return;

        StartBlackTurn();
        chessClick.Play();
    }

    public void BlackEndTurn()
    {
        if (gameState != ChessGameState.BlackMoved) return;
        Debug.Log("Black End Turn pressed");

        EvaluateGameEndForNextPlayer(PieceColor.White);

        if (gameState == ChessGameState.Checkmate || gameState == ChessGameState.Stalemate || gameState == ChessGameState.TimeUp)
            return;

        StartWhiteTurn();
        chessClick.Play();
    }

    // -------------------- Selection / Movement --------------------

    public void SelectPiece(ChessPiece piece)
    {
        if (awaitingPromotion) return;
        if (gameState != ChessGameState.WhiteTurn && gameState != ChessGameState.BlackTurn) return;
        if (piece.color != currentTurn) return;

        List<Vector2Int> moves = piece.GetLegalMoves(board);
        if (moves.Count == 0) return;

        selectedPiece = piece;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        HighlightMoves(moves, piece);
    }

    public void TryMove(int x, int y)
    {
        if (awaitingPromotion) return;
        if (selectedPiece == null) return;

        // Only allow moving during active turns (not after moved)
        if (gameState != ChessGameState.WhiteTurn && gameState != ChessGameState.BlackTurn)
            return;

        List<Vector2Int> moves = selectedPiece.GetLegalMoves(board);

        foreach (var move in moves)
        {
            if (move.x == x && move.y == y)
            {
                if (board.WouldMoveCauseCheck(selectedPiece, x, y))
                    return;

                ExecuteMove(x, y);
                return;
            }
        }
    }

    void ExecuteMove(int x, int y)
    {
        ChessPiece movedPiece = selectedPiece;

        board.MovePiece(movedPiece, x, y);
        MoveVisual(movedPiece, x, y);

        selectedPiece = null;
        ClearHighlights();

        // Promotion locks the game until chosen
        if (movedPiece is Pawn &&
            ((movedPiece.color == PieceColor.White && y == 7) ||
             (movedPiece.color == PieceColor.Black && y == 0)))
        {
            BeginPawnPromotion((Pawn)movedPiece);
            return;
        }

        OnPieceMoved();
    }

    void OnPieceMoved()
    {
        // After moving, turn does NOT switch until End Turn pressed
        if (currentTurn == PieceColor.White)
        {
            gameState = ChessGameState.WhiteMoved;
            whiteEndTurnButton.SetActive(true);
        }
        else
        {
            gameState = ChessGameState.BlackMoved;
            blackEndTurnButton.SetActive(true);
        }

        UpdateCheckUI();
    }

    // -------------------- Check UI / End Conditions --------------------

    void UpdateCheckUI()
    {
        bool whiteCheck = board.IsKingInCheck(PieceColor.White);
        bool blackCheck = board.IsKingInCheck(PieceColor.Black);

        if (whiteInCheckPanel) whiteInCheckPanel.SetActive(whiteCheck);
        if (blackInCheckPanel) blackInCheckPanel.SetActive(blackCheck);
    }

    void EvaluateGameEndForNextPlayer(PieceColor nextPlayer)
    {
        // After ending turn, we evaluate if the player about to move is checkmated or stalemated
        Debug.Log("Evaluating for: " + nextPlayer);
        Debug.Log("Is checkmate? " + board.IsCheckmate(nextPlayer));
        if (board.IsCheckmate(nextPlayer))
        {
            gameState = ChessGameState.Checkmate;

            if (nextPlayer == PieceColor.White)
            {
                blackCheckmatePanel.SetActive(true);
                applause.Play();
                blackTimerPanel.SetActive(false);
                blackTurnPanel.SetActive(false);
                rageQuitButton.SetActive(false);
                whiteInCheckPanel.SetActive(false);
                exitButton.SetActive(true);
            }               
            else
            {
                whiteCheckmatePanel.SetActive(true);
                applause.Play();
                whiteTimerPanel.SetActive(false);
                whiteTurnPanel.SetActive(false);
                rageQuitButton.SetActive(false);
                blackInCheckPanel.SetActive(false);
                exitButton.SetActive(true);
            }
                

            EndGame();
            return;
        }

        if (board.IsStalemate(nextPlayer))
        {
            gameState = ChessGameState.Stalemate;
            stalematePanel.SetActive(true);
            applause.Play();
            whiteTimerPanel.SetActive(false);
            whiteTurnPanel.SetActive(false);
            blackInCheckPanel.SetActive(false);
            blackTimerPanel.SetActive(false);
            blackTurnPanel.SetActive(false);
            rageQuitButton.SetActive(false);
            whiteInCheckPanel.SetActive(false);
            exitButton.SetActive(true);
            EndGame();
        }
    }

    void EndGame()
    {
        // Lock everything down
        ClearHighlights();
        selectedPiece = null;
        whiteEndTurnButton.SetActive(false);
        blackEndTurnButton.SetActive(false);
        awaitingPromotion = true; // simplest “hard lock”
    }

    // -------------------- Pawn Promotion --------------------

    void BeginPawnPromotion(Pawn pawn)
    {
        pawnPendingPromotion = pawn;
        awaitingPromotion = true;
        gameState = ChessGameState.Promotion;

        if (pawn.color == PieceColor.White)
            whitePromotionPanel.SetActive(true);
        else
            blackPromotionPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PromotePawn(GameObject prefab)
    {
        if (pawnPendingPromotion == null) return;

        int x = pawnPendingPromotion.x;
        int y = pawnPendingPromotion.y;
        PieceColor color = pawnPendingPromotion.color;

        board.grid[x, y] = null;
        Destroy(pawnPendingPromotion.gameObject);

        GameObject newPieceObj =
            Instantiate(prefab, BoardMapper.Instance.BoardToWorld(x, y), Quaternion.identity);

        newPieceObj.transform.localScale = Vector3.one * promotedPieceScale;

        ChessPiece newPiece = newPieceObj.GetComponent<ChessPiece>();
        newPiece.color = color;
        newPiece.SetPosition(x, y);
        newPiece.hasMoved = true;
        board.grid[x, y] = newPiece;

        pawnPendingPromotion = null;

        if (color == PieceColor.White) whitePromotionPanel.SetActive(false);
        else blackPromotionPanel.SetActive(false);

        awaitingPromotion = false;

        // After promotion, you still haven't ended the turn automatically
        OnPieceMoved();
    }

    public void PromoteToWhiteQueen() => PromotePawn(whiteQueenPrefab);
    public void PromoteToWhiteRook() => PromotePawn(whiteRookPrefab);
    public void PromoteToWhiteBishop() => PromotePawn(whiteBishopPrefab);
    public void PromoteToWhiteKnight() => PromotePawn(whiteKnightPrefab);

    public void PromoteToBlackQueen() => PromotePawn(blackQueenPrefab);
    public void PromoteToBlackRook() => PromotePawn(blackRookPrefab);
    public void PromoteToBlackBishop() => PromotePawn(blackBishopPrefab);
    public void PromoteToBlackKnight() => PromotePawn(blackKnightPrefab);

    // -------------------- Move highlighting --------------------

    void HighlightMoves(List<Vector2Int> moves, ChessPiece piece)
    {
        ClearHighlights();

        foreach (var m in moves)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Vector3 pos = BoardMapper.Instance.BoardToWorld(m.x, m.y);
            pos.y += 0.01f;

            marker.transform.position = pos;

            float size = BoardMapper.Instance.SquareSize * 2.3f;
            marker.transform.localScale = new Vector3(size, 0.02f, size);

            marker.GetComponent<Collider>().enabled = false;

            bool isCapture =
                board.GetPiece(m.x, m.y) != null &&
                board.GetPiece(m.x, m.y).color != piece.color;

            Renderer r = marker.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Standard"));
            r.material.color = isCapture
                ? new Color(1f, 0f, 0f, 0.6f)
                : new Color(0f, 1f, 0f, 0.6f);

            debugMarkers.Add(marker);
        }
    }

    void ClearHighlights()
    {
        for (int i = 0; i < debugMarkers.Count; i++)
            if (debugMarkers[i] != null) Destroy(debugMarkers[i]);

        debugMarkers.Clear();
    }

    void MoveVisual(ChessPiece piece, int x, int y)
    {
        piece.transform.position = BoardMapper.Instance.BoardToWorld(x, y);
    }

    public void ResetBoard()
    {
        // Destroy all existing pieces
        ChessPiece[] pieces = FindObjectsOfType<ChessPiece>();
        foreach (ChessPiece p in pieces)
            Destroy(p.gameObject);

        // Clear grid
        board = new ChessBoard();

        // Respawn original layout
        foreach (var snapshot in startingLayout)
        {
            GameObject newPiece =
                Instantiate(snapshot.prefab,
                    BoardMapper.Instance.BoardToWorld(snapshot.position.x, snapshot.position.y),
                    Quaternion.identity);

            newPiece.transform.localScale = Vector3.one * promotedPieceScale;

            ChessPiece cp = newPiece.GetComponent<ChessPiece>();
            cp.color = snapshot.color;
            cp.SetPosition(snapshot.position.x, snapshot.position.y);
            cp.hasMoved = false;

            board.grid[snapshot.position.x, snapshot.position.y] = cp;
        }

        // Reset state
        currentTurn = PieceColor.White;
        selectedPiece = null;
        awaitingPromotion = false;
        gameState = ChessGameState.Inactive;

        SetAllPanelsOff();
    }

    // -------------------- Timers --------------------

    void Update()
    {
        if (gameState == ChessGameState.WhiteTurn || gameState == ChessGameState.WhiteMoved)
        {
            whiteTimer -= Time.deltaTime;
            if (whiteTimerText) whiteTimerText.text = Mathf.Ceil(whiteTimer).ToString();

            if (whiteTimer <= 0f)
            {
                gameState = ChessGameState.TimeUp;
                if (whiteTimesUpPanel) whiteTimesUpPanel.SetActive(true);
                whiteTimerPanel.SetActive(false);
                whiteTurnPanel.SetActive(false);
                exitButton.SetActive(true);
                rageQuitButton.SetActive(false);
                if (!applausePlayed)
                {
                    applause.Play();
                    applausePlayed = true;
                }
                EndGame();
            }
        }

        if (gameState == ChessGameState.BlackTurn || gameState == ChessGameState.BlackMoved)
        {
            blackTimer -= Time.deltaTime;
            if (blackTimerText) blackTimerText.text = Mathf.Ceil(blackTimer).ToString();

            if (blackTimer <= 0f)
            {
                gameState = ChessGameState.TimeUp;
                if (blackTimesUpPanel) blackTimesUpPanel.SetActive(true);
                blackTimerPanel.SetActive(false);
                blackTurnPanel.SetActive(false);
                rageQuitButton.SetActive(false);
                exitButton.SetActive(true);
                if (!applausePlayed)
                {
                    applause.Play();
                    applausePlayed = true;
                }
                EndGame();
            }
        }
    }

    public void LeaveChess()
    {
        awaitingPromotion = false;
        selectedPiece = null;

        rageQuitButton.SetActive(false);
        gameState = ChessGameState.Inactive;

        blackTimerPanel.SetActive(false);
        whiteTimerPanel.SetActive(false);
        
        ClearHighlights();

        
        SetAllPanelsOff();
        whiteEndTurnButton.SetActive(false);
        blackEndTurnButton.SetActive(false);

        
        StopAllCoroutines();
        StartCoroutine(ExitChessRoutine());
    }

    private IEnumerator ExitChessRoutine()
    {
        stalematePanel.SetActive(false);
        exitButton.SetActive(false);
        blackTimesUpPanel.SetActive(false);
        whiteTimesUpPanel.SetActive(false);
        whiteCheckmatePanel.SetActive(false);
        blackCheckmatePanel.SetActive(false);
        compScript.chessIntroPanel.SetActive(false);
        compScript.goopy.Play();
        yield return new WaitForSeconds(0.6f);
        ResetBoard();
        compScript.cursorIcon.SetActive(false);
        compScript.raycastIcon.SetActive(true);
        compScript.ExitChessMode();
        compScript.movementScript.enabled = true;
        applausePlayed = false;
    }

    public void Nevermind()
    {
        StartCoroutine(NeverMinder());
    }

    private IEnumerator NeverMinder()
    {
        compScript.chessIntroPanel.SetActive(false);
        compScript.goopy.Play();
        yield return new WaitForSeconds(0.6f);
        compScript.cursorIcon.SetActive(false);
        compScript.raycastIcon.SetActive(true);
        compScript.ExitChessMode();
        compScript.movementScript.enabled = true;
    }
}