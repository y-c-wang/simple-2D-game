using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
  private readonly int MIN_ROW_IDX = 0;
  private readonly int MIN_COL_IDX = 0;
  private readonly int MAX_ROW_IDX = 15;
  private readonly int MAX_COL_IDX = 15;

  private string status;
  private string turn;

  private int rowIdx;
  private int colIdx;

  private Row[] rows;
  private Text[] texts;

  void Start()
  {
    Debug.Log("Program Start");
    rows = GetComponentsInChildren<Row>();
    texts = GetComponentsInChildren<Text>();
    NewGame();
  }

  void Update()
  {
    bool ret = true;
    HandleQuitKeyInput();
    switch (status)
    {
      case "in-game":
        Debug.Log("in-game");
        ret = this.HandleInGameKeyInput();
        break;
      case "end":
        Debug.Log("end");
        this.HandleEndKeyInput();
        break;
      default:
        break;
    }
  }
  void HandleQuitKeyInput()
  {
    if (Input.GetKey(KeyCode.Escape))
    {
      Application.Quit();
    }
  }
  public void NewGame()
  {
    this.turn = "black";
    texts[0].text = this.turn + "'s turn.";
    texts[1].text = "Move by pressing [W/A/S/D] and";
    texts[2].text = "place the pieces by pressing [space].";
    texts[3].text = "Exit by pressing [Esc].";
    this.status = "in-game";
    if (this.findValidPosIdx() == false) Debug.LogError("Error");
  }

  bool HandleInGameKeyInput()
  {
    this.HandleMove();
    if (this.HandlePress() == false) return false;
    return true;
  }

  void HandleEndKeyInput()
  {
    HandleRestartPress();
  }

  void HandleMove()
  {
    if (Input.GetKeyDown(KeyCode.W))
    {
      int nextRowIdx = rowIdx - 1;
      while (nextRowIdx >= MIN_ROW_IDX)
      {
        string nextChess = this.rows[nextRowIdx].blocks[this.colIdx].chess;
        if (nextChess == "none")
        {
          break;
        }
        nextRowIdx--;
      }
      if (nextRowIdx >= MIN_ROW_IDX)
      {
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("none");
        this.rowIdx = nextRowIdx;
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("tmp-" + this.turn);
      }
    }

    if (Input.GetKeyDown(KeyCode.A))
    {
      int nextColIdx = colIdx - 1;
      while (nextColIdx >= MIN_COL_IDX)
      {
        string nextChess = this.rows[this.rowIdx].blocks[nextColIdx].chess;
        if (nextChess == "none")
        {
          break;
        }
        nextColIdx--;
      }
      if (nextColIdx >= MIN_COL_IDX)
      {
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("none");
        this.colIdx = nextColIdx;
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("tmp-" + this.turn);
      }
    }

    if (Input.GetKeyDown(KeyCode.S))
    {
      int nextRowIdx = rowIdx + 1;
      while (nextRowIdx <= MAX_ROW_IDX)
      {
        string nextChess = this.rows[nextRowIdx].blocks[this.colIdx].chess;
        if (nextChess == "none")
        {
          break;
        }
        nextRowIdx++;
      }
      if (nextRowIdx <= MAX_ROW_IDX)
      {
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("none");
        this.rowIdx = nextRowIdx;
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("tmp-" + this.turn);
      }
    }

    if (Input.GetKeyDown(KeyCode.D))
    {
      int nextColIdx = colIdx + 1;
      while (nextColIdx <= MAX_COL_IDX)
      {
        string nextChess = this.rows[this.rowIdx].blocks[nextColIdx].chess;
        if (nextChess == "none")
        {
          break;
        }
        nextColIdx++;
      }
      if (nextColIdx <= MAX_COL_IDX)
      {
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("none");
        this.colIdx = nextColIdx;
        this.rows[this.rowIdx].blocks[this.colIdx].setChess("tmp-" + this.turn);
      }
    }
  }

  bool HandlePress()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      this.rows[rowIdx].blocks[colIdx].setChess(this.turn);
      if (this.isWin())
      {
        this.status = "end";
        texts[0].text = "Game ended, " + this.turn + " wins!";
        texts[1].text = "Restart by pressing [space].";
        texts[2].text = "Exit by pressing [Esc].";
        texts[3].text = "";
        return true;
      }
      this.changeTurn();
      texts[0].text = this.turn + "'s turn.";
      texts[1].text = "Move by pressing [W/A/S/D] and";
      texts[2].text = "place the pieces by pressing [space].";
      texts[3].text = "Exit by pressing [Esc].";
      if (this.findValidPosIdx() == false)
      {
        return false;
      }
    }
    return true;
  }

  void HandleRestartPress()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      clearBoard();
      NewGame();
    }
  }

  bool findValidPosIdx()
  {
    for (this.rowIdx = MIN_ROW_IDX; rowIdx <= MAX_ROW_IDX; rowIdx++)
    {
      for (this.colIdx = MIN_COL_IDX; colIdx <= MAX_COL_IDX; colIdx++)
      {
        string curChess = this.rows[rowIdx].blocks[colIdx].chess;
        if (curChess == "none")
        {
          this.rows[this.rowIdx].blocks[this.colIdx].setChess("tmp-" + this.turn);
          Debug.Log("Pos = (" + rowIdx + ", " + colIdx + ")");
          return true;
        }
      }
    }
    return false;
  }

  void clearBoard()
  {
    for (this.rowIdx = MIN_ROW_IDX; rowIdx <= MAX_ROW_IDX; rowIdx++)
    {
      for (this.colIdx = MIN_COL_IDX; colIdx <= MAX_COL_IDX; colIdx++)
      {
        this.rows[rowIdx].blocks[colIdx].setChess("none");
      }
    }
    this.rowIdx = 0;
    this.colIdx = 0;
  }

  bool isWin()
  {
    if (isWinUpDown()) return true;
    if (isWinLeftRight()) return true;
    if (isWinLeftUpRightDown()) return true;
    if (isWinLeftDownRightUp()) return true;
    return false;
  }

  bool isWinUpDown()
  {
    int consecutive_cnt = 0;
    for (int tmpShift = -4; tmpShift <= 4; tmpShift++)
    {
      int tmpRowIdx = this.rowIdx + tmpShift;
      if (tmpRowIdx < MIN_ROW_IDX) continue;
      if (tmpRowIdx > MAX_ROW_IDX) continue;

      int tmpColIdx = this.colIdx;

      string curChess = this.rows[tmpRowIdx].blocks[tmpColIdx].chess;
      if (curChess == this.turn)
      {
        consecutive_cnt++;
        if (consecutive_cnt >= 5) return true;
      }
      else
      {
        consecutive_cnt = 0;
      }
    }
    return false;
  }

  bool isWinLeftRight()
  {
    int consecutive_cnt = 0;
    for (int tmpShift = -4; tmpShift <= 4; tmpShift++)
    {
      int tmpRowIdx = this.rowIdx;

      int tmpColIdx = this.colIdx + tmpShift;
      if (tmpColIdx < MIN_COL_IDX) continue;
      if (tmpColIdx > MAX_COL_IDX) continue;

      string curChess = this.rows[tmpRowIdx].blocks[tmpColIdx].chess;
      if (curChess == this.turn)
      {
        consecutive_cnt++;
        if (consecutive_cnt >= 5) return true;
      }
      else
      {
        consecutive_cnt = 0;
      }
    }
    return false;
  }

  bool isWinLeftUpRightDown()
  {
    int consecutive_cnt = 0;
    for (int tmpShift = -4; tmpShift <= 4; tmpShift++)
    {
      int tmpRowIdx = this.rowIdx + tmpShift;
      if (tmpRowIdx < MIN_ROW_IDX) continue;
      if (tmpRowIdx > MAX_ROW_IDX) continue;

      int tmpColIdx = this.colIdx + tmpShift;
      if (tmpColIdx < MIN_COL_IDX) continue;
      if (tmpColIdx > MAX_COL_IDX) continue;

      string curChess = this.rows[tmpRowIdx].blocks[tmpColIdx].chess;
      if (curChess == this.turn)
      {
        consecutive_cnt++;
        if (consecutive_cnt >= 5) return true;
      }
      else
      {
        consecutive_cnt = 0;
      }
    }
    return false;
  }

  bool isWinLeftDownRightUp()
  {
    int consecutive_cnt = 0;
    for (int tmpShift = -4; tmpShift <= 4; tmpShift++)
    {
      int tmpRowIdx = this.rowIdx - tmpShift;
      if (tmpRowIdx < MIN_ROW_IDX) continue;
      if (tmpRowIdx > MAX_ROW_IDX) continue;

      int tmpColIdx = this.colIdx + tmpShift;
      if (tmpColIdx < MIN_COL_IDX) continue;
      if (tmpColIdx > MAX_COL_IDX) continue;

      string curChess = this.rows[tmpRowIdx].blocks[tmpColIdx].chess;
      if (curChess == this.turn)
      {
        consecutive_cnt++;
        if (consecutive_cnt >= 5) return true;
      }
      else
      {
        consecutive_cnt = 0;
      }
    }
    return false;
  }

  void changeTurn()
  {
    switch (this.turn)
    {
      case "black":
        this.turn = "white";
        break;

      case "white":
        this.turn = "black";
        break;

      default:
        break;
    }
  }
}
