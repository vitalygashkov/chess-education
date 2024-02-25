using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChessLib;

public class Rules : MonoBehaviour
{
    DragAndDrop dad;
    Chess chess;

    // For testing mode
    [SerializeField]
    private TextMeshProUGUI taskDescription = null;
    int taskNumber;
    string moveColor;
    int gameMode;
    List<string> fenList = new List<string>();
    List<string> playerMoves = new List<string>();

    public void FillFen()
    {
        fenList.Clear();
        char[] splitChars = System.Environment.NewLine.ToCharArray();
        TextAsset textResourceAsset = null;

        switch (gameMode)
        {
            case 1:
                textResourceAsset = (TextAsset)Resources.Load("Mate-in-One", typeof(TextAsset));
                break;
            case 2:
                textResourceAsset = (TextAsset)Resources.Load("Mate-in-Two", typeof(TextAsset));
                break;
            case 3:
                textResourceAsset = (TextAsset)Resources.Load("Mate-in-Three", typeof(TextAsset));
                break;
            default:
                break;
        }
        string textFile = textResourceAsset.text;
        if (textResourceAsset != null)
        {
            foreach (string stringItem in textFile.Split(splitChars))
            {
                if (stringItem.Length > 5)
                {
                    fenList.Add(Regex.Replace(stringItem, @"[^\-\w\d\ \/]+", "", RegexOptions.Compiled));
                }   
            }
        }
    }

    public int GetTaskNumber()
    {
        return UnityEngine.Random.Range(0, fenList.Count);
    }

    public Rules()
    {
        dad = new DragAndDrop();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameMode = PlayerPrefs.GetInt("mode");
        switch (gameMode)
        {
            case 1:
            case 2:
            case 3:
                FillFen();
                Next();
                break;
            case 4:
                Debug.Log("[Mode] Game Online");
                taskDescription.text = "Игра по сети";
                chess = new Chess();
                ShowFigures();
                break;
            case 5:
                Debug.Log("[Mode] Game with AI");
                taskDescription.text = "Игра с компьютером";
                chess = new Chess();
                ShowFigures();
                break;
            default:
                Debug.Log("[Mode] Undefined");
                taskDescription.text = "Режим игры не определён";
                chess = new Chess();
                ShowFigures();
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dad.Action())
        {
            string from = GetSquare(dad.pickPosition);
            string to = GetSquare(dad.dropPosition);
            string figure = chess.GetFigureAt((int)(dad.pickPosition.x / 2.0), (int)(dad.pickPosition.y / 2.0)).ToString();
            string move = figure + from + to;
            
            if (chess != chess.Move(move))
            {
                chess = chess.Move(move);
                switch (gameMode)
                {
                    case 1:
                        {
                            if (chess.IsCheckmate())
                                taskDescription.text = "Задача решена правильно";
                            else
                                taskDescription.text = "Задача решена неправильно";
                            break;
                        }
                        
                    case 2:
                        if (playerMoves.Count < 3)
                            playerMoves.Add(move);
                        if (playerMoves.Count == 3 && !chess.IsCheckmate())
                            taskDescription.text = "Задача решена неправильно";
                        if (playerMoves.Count == 3 && chess.IsCheckmate())
                            taskDescription.text = "Задача решена правильно";
                        break;
                    case 3:
                        {
                            if (playerMoves.Count < 5)
                                playerMoves.Add(move);
                            if (playerMoves.Count == 5 && !chess.IsCheckmate())
                                taskDescription.text = "Задача решена неправильно";
                            if (playerMoves.Count == 5 && chess.IsCheckmate())
                                taskDescription.text = "Задача решена правильно";
                            break;
                        }
                        
                    case 4:
                        playerMoves.Add(move);
                        break;
                    case 5:
                        playerMoves.Add(move);
                        break;
                    default:
                        Debug.Log("[INFO] Game Mode is undefined");
                        break;
                }
                
            }
            if (chess.IsCheck())
                Debug.Log("[INFO] Check");
            if (chess.IsCheckmate())
                Debug.Log("[INFO] Checkmate");
            Debug.Log("[INFO] Move " + move + " is done");
            Debug.Log("[INFO] " + playerMoves.Count + " moves done");
            ShowFigures();
        }
    }

    public void Next()
    {
        playerMoves.Clear();
        taskNumber = GetTaskNumber();
        chess = new Chess(fenList[taskNumber]);
        moveColor = chess.GetMoveColor();
        taskDescription.text = "#" + taskNumber + ". " + (moveColor == "black" ? "Чёрные" : "Белые") + " начинают и ставят мат в " + gameMode + " ход(а)";
        ShowFigures();
        Debug.Log("[INFO] Task #" + taskNumber + " created");
        Debug.Log("[INFO] FEN Code: (" + fenList[taskNumber] + ")");
        //if (chess.IsCheck())
        //{
        //    switch (PlayerPrefs.GetInt("mode"))
        //    {
        //        case 1:
        //            Debug.Log("[Status] Task Completed");
        //            chess = new Chess(fenList[UnityEngine.Random.Range(0, fenList.Count)]);
        //            ShowFigures();
        //            break;
        //        case 2:
        //            Debug.Log("[Status] Game Over");
        //            //chess = new Chess();
        //            break;
        //        case 3:
        //            Debug.Log("[Status] Game Over");
        //            //chess = new Chess();
        //            break;
        //        default:
        //            chess = new Chess();
        //            ShowFigures();
        //            break;
        //    }
        //}
    }

    string GetSquare(Vector2 position) // e2
    {
        int x = Convert.ToInt32(position.x / 2.0);
        int y = Convert.ToInt32(position.y / 2.0);
        return ((char)('a' + x)).ToString() + (y + 1).ToString();
    }

    void ShowFigures()
    {
        int nr = 0;
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                string figure = chess.GetFigureAt(x, y).ToString();
                if (figure == ".") continue;
                PlaceFigure("box" + nr, figure, x, y);
                nr++;
            }
        for (; nr < 32; nr++)
            PlaceFigure("box" + nr, "q", 9, 9);
    }

    //void MarkValidFigures()
    //{
    //    for (int y = 0; y < 8; y++)
    //        for (int x = 0; x < 8; x++)
    //            MarkSquare(x, y, false);
    //    foreach (string moves in chess.YieldValidMoves())
    //    { доделать Marking Figures
            
    //    }
    //}

    void PlaceFigure(string box, string figure, int x, int y)
    {
        //Debug.Log(box + " " + figure + " " + x + y);
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure); // K R P n b etc.
        GameObject goSquare = GameObject.Find("" + y + x);

        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;

        goBox.transform.position = goSquare.transform.position;
    }

    void MarkSquare(int x, int y, bool isMarked)
    {
        GameObject goSquare = GameObject.Find("" + y + x);
        GameObject goCell;
        string color = (x + y) % 2 == 0 ? "Black" : "White";
        if (isMarked)
            goCell = GameObject.Find(color + "SquareMarked");
        else
            goCell = GameObject.Find(color + "Square");
        var spriteSquare = goSquare.GetComponent<SpriteRenderer>();
        var spriteCell = goCell.GetComponent<SpriteRenderer>();
        spriteSquare.sprite = spriteCell.sprite;
    }
}

class DragAndDrop
{
    enum State
    {
        none,
        drag,
    }

    public Vector2 pickPosition { get; private set; }
    public Vector2 dropPosition { get; private set; }

    State state;
    GameObject item;
    Vector2 offset;

    public DragAndDrop()
    {
        state = State.none;
        item = null;
    }

    public bool Action()
    {
        switch (state)
        {
            case State.none:
                if (IsMouseButtonPressed())
                    PickUp();
                break;
            case State.drag:
                if (IsMouseButtonPressed())
                    Drag();
                else
                {
                    Drop();
                    return true;
                }
                break;
        }
        return false;
    }

    bool IsMouseButtonPressed()
    {
        return Input.GetMouseButton(0);
    }

    void PickUp()
    {
        Vector2 clickPosition = GetClickPosition();
        Transform clickedItem = GetItemAt(clickPosition);
        if (clickedItem == null) return;

        pickPosition = clickedItem.position;
        item = clickedItem.gameObject;
        state = State.drag;
        offset = pickPosition - clickPosition;
        Debug.Log("Item has picked up: " + item.name);
    }

    Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    Transform GetItemAt(Vector2 position)
    {
        RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);
        if (figures.Length == 0)
            return null;
        return figures[0].transform;
    }

    void Drag()
    {
        item.transform.position = GetClickPosition() + offset;
    }

    void Drop()
    {
        dropPosition = item.transform.position;
        state = State.none;
        item = null;
    }
}