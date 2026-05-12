using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class BGBSliderManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject slidingGameDescription;

    public int puzzleType;
    public Transform[] nullLocations;
    public GameObject[] liquotholPieces;
    public GameObject[] phalangesPieces;
    public GameObject[] defraudPieces;

    private SlidePuzzlePiece blank;

    public bool gameActive;
    public bool started;
    public bool countdownDone;

    private float startTimer;
    private float gameTimeTrack;

    public GameObject winPanel;
    public GameObject loseOnePanel;


    public TextMeshProUGUI countdown;
    public TextMeshProUGUI gameCountdown;

    SlidePuzzlePiece[] liquotholPieceScripts;
    SlidePuzzlePiece[] phalangesPieceScripts;
    SlidePuzzlePiece[] defraudPieceScripts;

    public int countdownValue;
    public int gameTime;

    public BonusGameBlitzInteraction interactScript;
    public BGBlitzManager blitzScript;
    public BGBTimer timerScript;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;
    public AudioSource whistle;
    bool hasPlayedLoseTheme;

    public bool normalBeat, expertBeat, egBeat;
    public GameObject normalSelect, expertSelect, egSelect;

    // Start is called before the first frame update
    void Start()
    {
        puzzleType = 0;
        gameActive = false;

        liquotholPieceScripts = new SlidePuzzlePiece[liquotholPieces.Length];
        for (int i = 0; i < liquotholPieces.Length; i++)
            liquotholPieceScripts[i] = liquotholPieces[i].GetComponent<SlidePuzzlePiece>();

        phalangesPieceScripts = new SlidePuzzlePiece[phalangesPieces.Length];
        for (int i = 0; i < phalangesPieces.Length; i++)
            phalangesPieceScripts[i] = phalangesPieces[i].GetComponent<SlidePuzzlePiece>();

        defraudPieceScripts = new SlidePuzzlePiece[defraudPieces.Length];
        for (int i = 0; i < defraudPieces.Length; i++)
            defraudPieceScripts[i] = defraudPieces[i].GetComponent<SlidePuzzlePiece>();

        countdownValue = 3;
        for (int i = 0; i < liquotholPieces.Length; i++)
        {
            Button btn = liquotholPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;

            liquotholPieces[i].SetActive(false);
        }

        for (int i = 0; i < phalangesPieces.Length; i++)
        {
            Button btn = phalangesPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;

            phalangesPieces[i].SetActive(false);
        }

        for (int i = 0; i < defraudPieces.Length; i++)
        {
            Button btn = defraudPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;

            defraudPieces[i].SetActive(false);
        }

        startTimer = 1f;
        gameTimeTrack = 1f;
        gameCountdown.enabled = false;
        hasPlayedLoseTheme = false;

        normalBeat = false;
        expertBeat = false;
        egBeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            countdown.enabled = true;
            if (countdownValue > 0)
            {
                startTimer -= Time.deltaTime;
                if (startTimer <= 0f)
                {
                    countdownValue--;
                    countdown.text = countdownValue.ToString();
                    startTimer = 1f;
                }
            }
            else if (started)
            {
                countdownDone = true;
                started = false;
                StartCoroutine(StartGame());
            }
        }

        if (gameActive)
        {
            gameCountdown.enabled = true;

            if (gameTime > 0)
            {
                gameTimeTrack -= Time.deltaTime;
                if (gameTimeTrack <= 0f)
                {
                    gameTime--;
                    gameCountdown.text = gameTime.ToString();
                    gameTimeTrack = 1f;
                }
            }
            else
            {
                gameActive = false;
                gameCountdown.enabled = false;
                if (!hasPlayedLoseTheme)
                {
                    bonusLoseTheme.Play();
                    hasPlayedLoseTheme = true;
                }
                for (int i = 0; i < liquotholPieces.Length; i++)
                {
                    Button btn = liquotholPieces[i].GetComponent<Button>();
                    if (btn != null)
                        btn.enabled = false;
                }

                for (int i = 0; i < phalangesPieces.Length; i++)
                {
                    Button btn = phalangesPieces[i].GetComponent<Button>();
                    if (btn != null)
                        btn.enabled = false;
                }

                for (int i = 0; i < defraudPieces.Length; i++)
                {
                    Button btn = defraudPieces[i].GetComponent<Button>();
                    if (btn != null)
                        btn.enabled = false;
                }
                loseOnePanel.SetActive(true);
            }
        }
    }

    public void SetupSlidePuzzle()
    {
        ResetPuzzleBoard();
        puzzleType = Random.Range(0, 3);
        int size = 3;
        slidingGameDescription.SetActive(false);

        countdownValue = 3;
        startTimer = 1f;
        gameTimeTrack = 1f;
        countdown.text = countdownValue.ToString();
        started = true;

        Transform[] shuffledLocations = new Transform[nullLocations.Length];
        nullLocations.CopyTo(shuffledLocations, 0);

        for (int i = 0; i < liquotholPieces.Length; i++)
        {
            Button btn = liquotholPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;

        }

        for (int i = 0; i < phalangesPieces.Length; i++)
        {
            Button btn = phalangesPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;

        }

        for (int i = 0; i < defraudPieces.Length; i++)
        {
            Button btn = defraudPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;
        }


        if (puzzleType == 0)
        {
            ShuffleUntilValid(shuffledLocations, nullLocations, liquotholPieces[liquotholPieces.Length - 1].transform);

            // Place first 8 tiles into shuffled slots
            for (int i = 0; i < liquotholPieces.Length - 1; i++)
            {
                liquotholPieces[i].transform.position = shuffledLocations[i].position;
                liquotholPieces[i].SetActive(true);

                SlidePuzzlePiece p = liquotholPieces[i].GetComponent<SlidePuzzlePiece>();
                NullSlots slot = shuffledLocations[i].GetComponent<NullSlots>();
                p.x = slot.x;
                p.y = slot.y;

                Debug.Log($"{p.name} at ({p.x},{p.y}) should be at ({p.pieceID % 3},{p.pieceID / 3})");
            }

            // Force the blank into bottom-right (2,2)
            blank = liquotholPieceScripts[liquotholPieceScripts.Length - 1];
            NullSlots blankSlot = nullLocations[8].GetComponent<NullSlots>(); // slot index 8 = (2,2)
            blank.x = blankSlot.x;
            blank.y = blankSlot.y;
        }
        else if (puzzleType == 1)
        {
            ShuffleUntilValid(shuffledLocations, nullLocations, phalangesPieces[phalangesPieces.Length - 1].transform);

            // Place first 8 tiles into shuffled slots
            for (int i = 0; i < phalangesPieces.Length - 1; i++)
            {
                phalangesPieces[i].transform.position = shuffledLocations[i].position;
                phalangesPieces[i].SetActive(true);

                SlidePuzzlePiece p = phalangesPieces[i].GetComponent<SlidePuzzlePiece>();
                NullSlots slot = shuffledLocations[i].GetComponent<NullSlots>();
                p.x = slot.x;
                p.y = slot.y;

                Debug.Log($"{p.name} at ({p.x},{p.y}) should be at ({p.pieceID % 3},{p.pieceID / 3})");
            }

            // Force the blank into bottom-right (2,2)
            blank = phalangesPieceScripts[phalangesPieceScripts.Length - 1];
            NullSlots blankSlot = nullLocations[8].GetComponent<NullSlots>(); // slot index 8 = (2,2)
            blank.x = blankSlot.x;
            blank.y = blankSlot.y;
        }
        else
        {
            ShuffleUntilValid(shuffledLocations, nullLocations, defraudPieces[defraudPieces.Length - 1].transform);

            // Place first 8 tiles into shuffled slots
            for (int i = 0; i < defraudPieces.Length - 1; i++)
            {
                defraudPieces[i].transform.position = shuffledLocations[i].position;
                defraudPieces[i].SetActive(true);

                SlidePuzzlePiece p = defraudPieces[i].GetComponent<SlidePuzzlePiece>();
                NullSlots slot = shuffledLocations[i].GetComponent<NullSlots>();
                p.x = slot.x;
                p.y = slot.y;

                Debug.Log($"{p.name} at ({p.x},{p.y}) should be at ({p.pieceID % 3},{p.pieceID / 3})");
            }

            // Force the blank into bottom-right (2,2)
            blank = defraudPieceScripts[defraudPieceScripts.Length - 1];
            NullSlots blankSlot = nullLocations[8].GetComponent<NullSlots>(); // slot index 8 = (2,2)
            blank.x = blankSlot.x;
            blank.y = blankSlot.y;
        }
    }
    private IEnumerator StartGame()
    {
        whistle.Play();
        countdown.text = string.Format("GO!");
        yield return new WaitForSeconds(1f);
        countdown.enabled = false;
        gameActive = true;
        gameCountdown.text = gameTime.ToString();

        for (int i = 0; i < liquotholPieces.Length; i++)
        {
            Button btn = liquotholPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = true;
        }

        for (int i = 0; i < phalangesPieces.Length; i++)
        {
            Button btn = phalangesPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = true;
        }

        for (int i = 0; i < defraudPieces.Length; i++)
        {
            Button btn = defraudPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = true;
        }

    }

    public static void FisherYatesShuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int j = UnityEngine.Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    bool IsSolved(Transform[] pieces, Transform[] solutionPositions)
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != solutionPositions[i])
                return false;
        }
        return true;
    }

    bool IsSolvable(Transform[] pieces, Transform blankTile)
    {
        // Map each piece to an ID (here I assume siblingIndex = solved order)

        List<int> values = new List<int>();
        foreach (Transform piece in pieces)
        {
            if (piece != blankTile)
                values.Add(piece.GetSiblingIndex());
        }

        int inversions = 0;
        for (int i = 0; i < values.Count; i++)
        {
            for (int j = i + 1; j < values.Count; j++)
            {
                if (values[i] > values[j])
                    inversions++;
            }
        }
        return inversions % 2 == 0;
    }

    void ShuffleUntilValid(Transform[] pieces, Transform[] solutionPositions, Transform blankTile)
    {
        do
        {
            // Only shuffle first 8 slots (leave last slot reserved for blank)

            for (int i = 0; i < pieces.Length - 2; i++)
            {
                int j = UnityEngine.Random.Range(i, pieces.Length - 1);
                Transform temp = pieces[i];
                pieces[i] = pieces[j];
                pieces[j] = temp;
            }

            // Force blank to last slot (index 8 in a 3x3)
            pieces[pieces.Length - 1] = blankTile;
        }
        while (IsSolved(pieces, solutionPositions) || !IsSolvable(pieces, blankTile));
    }

    public void OnPieceClicked(SlidePuzzlePiece piece)
    {
        if (!gameActive) return;

        int rowDiff = Mathf.Abs(piece.y - blank.y);
        int colDiff = Mathf.Abs(piece.x - blank.x);

        if ((rowDiff == 1 && colDiff == 0) || (rowDiff == 0 && colDiff == 1))
        {
            // Swap positions
            Vector3 tempPos = piece.transform.position;
            piece.transform.position = blank.transform.position;
            blank.transform.position = tempPos;

            // Swap row/col
            int tempCol = piece.x;
            int tempRow = piece.y;
            piece.x = blank.x;
            piece.y = blank.y;
            blank.x = tempCol;
            blank.y = tempRow;
            StartCoroutine(MidPieceSlide());
        }

        if (puzzleType == 0)
        {
            if (CheckWin(liquotholPieceScripts))
            {
                gameActive = false;

                winPanel.SetActive(true);
                bonusWinTheme.Play();
                if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Normal)
                {
                    blitzScript.score += 180;
                    Color c = normalSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    normalSelect.GetComponent<Image>().color = c;
                    normalSelect.GetComponent<Button>().enabled = false;
                    normalBeat = true;
                    blitzScript.normalsBeat += 1;
                    blitzScript.checks[6].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Expert)
                {
                    blitzScript.score += 200;
                    Color c = expertSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    expertSelect.GetComponent<Image>().color = c;
                    expertSelect.GetComponent<Button>().enabled = false;
                    expertBeat = true;
                    blitzScript.expertsBeat += 1;
                    blitzScript.checks[7].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.EpicGamer)
                {
                    blitzScript.score += 280;
                    Color c = egSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    egSelect.GetComponent<Image>().color = c;
                    egSelect.GetComponent<Button>().enabled = false;
                    egBeat = true;
                    blitzScript.egsBeat += 1;
                    blitzScript.checks[8].SetActive(true);
                }
                else
                {

                }
                if (normalBeat && expertBeat && egBeat)
                {
                    timerScript.timer += 60f;
                }
                gameCountdown.enabled = false;
            }
        }
        else if (puzzleType == 1)
        {
            if (CheckWin(phalangesPieceScripts))
            {
                gameActive = false;

                winPanel.SetActive(true);
                bonusWinTheme.Play();
                if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Normal)
                {
                    blitzScript.score += 180;
                    Color c = normalSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    normalSelect.GetComponent<Image>().color = c;
                    normalSelect.GetComponent<Button>().enabled = false;
                    normalBeat = true;
                    blitzScript.normalsBeat += 1;
                    blitzScript.checks[6].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Expert)
                {
                    blitzScript.score += 200;
                    Color c = expertSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    expertSelect.GetComponent<Image>().color = c;
                    expertSelect.GetComponent<Button>().enabled = false;
                    expertBeat = true;
                    blitzScript.expertsBeat += 1;
                    blitzScript.checks[7].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.EpicGamer)
                {
                    blitzScript.score += 280;
                    Color c = egSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    egSelect.GetComponent<Image>().color = c;
                    egSelect.GetComponent<Button>().enabled = false;
                    egBeat = true;
                    blitzScript.egsBeat += 1;
                    blitzScript.checks[8].SetActive(true);
                }
                else
                {

                }
                if (normalBeat && expertBeat && egBeat)
                {
                    timerScript.timer += 60f;
                }
                gameCountdown.enabled = false;
            }
        }
        else
        {
            if (CheckWin(defraudPieceScripts))
            {
                gameActive = false;

                winPanel.SetActive(true);
                bonusWinTheme.Play();
                if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Normal)
                {
                    blitzScript.score += 180;
                    Color c = normalSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    normalSelect.GetComponent<Image>().color = c;
                    normalSelect.GetComponent<Button>().enabled = false;
                    normalBeat = true;
                    blitzScript.normalsBeat += 1;
                    blitzScript.checks[6].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Expert)
                {
                    blitzScript.score += 200;
                    Color c = expertSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    expertSelect.GetComponent<Image>().color = c;
                    expertSelect.GetComponent<Button>().enabled = false;
                    expertBeat = true;
                    blitzScript.expertsBeat += 1;
                    blitzScript.checks[7].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.EpicGamer)
                {
                    blitzScript.score += 280;
                    Color c = egSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    egSelect.GetComponent<Image>().color = c;
                    egSelect.GetComponent<Button>().enabled = false;
                    blitzScript.egsBeat += 1;
                    egBeat = true;
                    blitzScript.checks[8].SetActive(true);
                }
                else
                {

                }
                gameCountdown.enabled = false;
                if (normalBeat && expertBeat && egBeat)
                {
                    timerScript.timer += 60f;
                }
            }
        }
    }

    bool CheckWin(SlidePuzzlePiece[] pieces)
    {
        foreach (SlidePuzzlePiece p in pieces)
        {
            if (p.isBlank) continue;
            Debug.Log($"{p.name} at ({p.x},{p.y}) should be at ({p.pieceID % 3},{p.pieceID / 3})");
            Debug.Log(blank.x + ", " + blank.y + " are the coordinates of the blank piece!");
            if (p.y != p.pieceID / 3 || p.x != p.pieceID % 3)
                return false;
        }
        return true;
    }

    private IEnumerator MidPieceSlide()
    {
        for (int i = 0; i < liquotholPieces.Length; i++)
        {
            Button btn = liquotholPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;
        }

        for (int i = 0; i < phalangesPieces.Length; i++)
        {
            Button btn = phalangesPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;
        }

        for (int i = 0; i < defraudPieces.Length; i++)
        {
            Button btn = defraudPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = false;
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < liquotholPieces.Length; i++)
        {
            Button btn = liquotholPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = true;
        }

        for (int i = 0; i < phalangesPieces.Length; i++)
        {
            Button btn = phalangesPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = true;
        }

        for (int i = 0; i < defraudPieces.Length; i++)
        {
            Button btn = defraudPieces[i].GetComponent<Button>();
            if (btn != null)
                btn.enabled = true;
        }
    }
    public void ReturnToAuction()
    {
        winPanel.SetActive(false);
        loseOnePanel.SetActive(false);
        hasPlayedLoseTheme = false;

        if (interactScript.bonusGameTheme.isPlaying) interactScript.bonusGameTheme.Stop();
        interactScript.bgbTheme.Play();
        blitzScript.scorePanel.SetActive(true);
        if (blitzScript.hasPlayed == 1)
        {
            blitzScript.bestScorePanel.SetActive(true);
        }
        else
        {
            blitzScript.bestScorePanel.SetActive(false);
        }



        foreach (GameObject piece in phalangesPieces)
        {
            piece.SetActive(false);
        }
        foreach (GameObject piece in defraudPieces)
        {
            piece.SetActive(false);
        }
        foreach (GameObject piece in liquotholPieces)
        {
            piece.SetActive(false);
        }
        if (blitzScript.normalsBeat == 4 && blitzScript.expertsBeat == 4 && blitzScript.egsBeat == 4)
        {
            timerScript.timer = 0f;
        }
    }

    private void ResetPuzzleBoard()
    {
        // Reset Liquothol pieces
        for (int i = 0; i < liquotholPieces.Length; i++)
        {
            liquotholPieces[i].SetActive(false);

            SlidePuzzlePiece p = liquotholPieceScripts[i];
            p.x = i % 3;      // solved column
            p.y = i / 3;      // solved row

            // Position pieces into solved locations (optional but safe)
            liquotholPieces[i].transform.position = nullLocations[i].position;
        }

        // Reset Phalanges pieces
        for (int i = 0; i < phalangesPieces.Length; i++)
        {
            phalangesPieces[i].SetActive(false);

            SlidePuzzlePiece p = phalangesPieceScripts[i];
            p.x = i % 3;
            p.y = i / 3;

            phalangesPieces[i].transform.position = nullLocations[i].position;
        }

        // Reset Defraud pieces
        for (int i = 0; i < defraudPieces.Length; i++)
        {
            defraudPieces[i].SetActive(false);

            SlidePuzzlePiece p = defraudPieceScripts[i];
            p.x = i % 3;
            p.y = i / 3;

            defraudPieces[i].transform.position = nullLocations[i].position;
        }
    }
}
