using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
    //board variables
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;

    public bool isMatched = false;
    public bool inMove    = true;

    private Board board;
    private GameObject otherItem;

    private FindMatches findMatches;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    public float swipeAngle = 0;
    public float swipeResist = 0.1f;   //1f

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;
   /*     if (Mathf.Abs(targetX - transform.position.x) > .1 ||
            Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(targetX, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
            board.allItems[row, column] = this.gameObject;
            return;
        }

        if (inMove)
        {
            tempPosition = new Vector2(targetX, targetY);
            transform.position = tempPosition;
            inMove = false;
            board.tilesToMove--;
            findMatches.FindAllMatchhes();
        }*/

        if (Mathf.Abs(targetX - transform.position.x) > .1) //move towards target
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
            board.allItems[row, column] = this.gameObject;
        }
        else  //set position
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;     
            inMove = false;
            board.tilesToMove--;     
            //findMatches.FindAllMatchhes();
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1) 
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
            board.allItems[row, column] = this.gameObject;
        }
        else 
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            inMove = false;
            board.tilesToMove--;
            //findMatches.FindAllMatchhes();
        }
    }

    private void OnMouseDown()
    {
        if(board.currentState == GameState.WaitingForActions)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //for movement limit
            Debug.Log(firstTouchPosition.y + " " + firstTouchPosition.x + "\n");
            Debug.Log(row + " " + column + "\n");
            Debug.Log(transform.position.y + " " + transform.position.x + "\n");
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.WaitingForActions)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
               Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
            {
                swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                    finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
                board.currentState = GameState.MovingTiles;
                MovePieces();
            }
        }
    }
    void MovePieces()
    {
        bool correctSwap = false;
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)  //right swipe (-1 for index out of range fix)
        {
            otherItem = board.allItems[row, column +1];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().column -= 1;
            column += 1;
            correctSwap = true;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)  //up swipe
        {
            otherItem = board.allItems[row + 1, column];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().row -= 1;
            row += 1;
            correctSwap = true;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)  //left swipe
        {
            otherItem = board.allItems[row, column - 1];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().column += 1;
            column -= 1;
            correctSwap = true;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)  //down swipe
        {
            otherItem = board.allItems[row - 1, column];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().row += 1;
            row -= 1;
            correctSwap = true;
        }
        if (correctSwap)
        {
            StartCoroutine(CheckMoveCoroutine());
        }
        else
        {
            board.currentState = GameState.WaitingForActions;
        }
    }

    public IEnumerator CheckMoveCoroutine() //if moved pieces dont match
    {
        yield return new WaitForSeconds(0.4f);
        if(otherItem != null)
        {
            if(!isMatched && !otherItem.GetComponent<Item>().isMatched)
            {
                inMove = true;
                board.tilesToMove++;
                otherItem.GetComponent<Item>().row = row;
                otherItem.GetComponent<Item>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.WaitingForActions;
            }
            else
            {
                board.DestroyMatches();
                
            }
            otherItem = null;
        }
        
    }

}
