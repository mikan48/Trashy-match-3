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

    private Board board;
    private GameObject otherItem;

    private FindMatches findMatches;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    public float swipeAngle = 0;
    public float swipeResist = 0.4f;   //1f

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();

        findMatches = FindObjectOfType<FindMatches>();

        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;
    }

    // Update is called once per frame
    void Update()
    {
        //findMatches.FindAllMatchhes();
        //FindMatches(); //finding matches every frame
        /*if(isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0, 0, 0, 0.2f);
        }*/
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1) //move towards target
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if(board.allItems[column, row] != this.gameObject)
            {
                board.allItems[column, row] = this.gameObject;
            }
            findMatches.FindAllMatchhes();
        }
        else //set position
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;            
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1) 
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (board.allItems[column, row] != this.gameObject)
            {
                board.allItems[column, row] = this.gameObject;
            }
            findMatches.FindAllMatchhes();
        }
        else 
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        if(board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //for movement limit
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AngleCalculate();
        }
    }

    void AngleCalculate()
    {
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)  //right swipe (-1 for index out of range fix)
        {
            otherItem = board.allItems[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)  //up swipe
        {
            otherItem = board.allItems[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)  //left swipe
        {
            otherItem = board.allItems[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)  //down swipe
        {
            otherItem = board.allItems[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherItem.GetComponent<Item>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCoroutine());
    }

    public IEnumerator CheckMoveCoroutine() //if moved pieces dont match
    {
        yield return new WaitForSeconds(.4f);
        if(otherItem != null)
        {
            if(!isMatched && !otherItem.GetComponent<Item>().isMatched)
            {
                otherItem.GetComponent<Item>().row = row;
                otherItem.GetComponent<Item>().column = column;
                row = previousRow;
                column = previousColumn;

                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
                
            }
            otherItem = null;
        }
        
    }

}
