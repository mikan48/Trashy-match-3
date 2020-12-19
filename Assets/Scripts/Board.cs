using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MovingTiles,
    WaitingForActions
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.WaitingForActions;

    public int width;
    public int height;
    public int offSet;
    public int tilesToMove;

    public GameObject tilePrefab;
    public GameObject[] items;
    public GameObject[,] allItems;

    private BackgroundTile[,] allTiles;

    private FindMatches findMatches;


    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        tilesToMove = height * width;
        allTiles = new BackgroundTile[height, width];
        allItems = new GameObject[height, width];
        SetUp();
    }

    private void SetUp()
    {
        for (int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Vector2 tempPosition = new Vector2(x, y + offSet);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition,Quaternion.identity);
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + y + ", " + x + " )";

                int RandomItem = Random.Range(0, items.Length);

                int maxIterations = 0;

                while(MatchesAt(y, x, items[RandomItem]) && maxIterations < 100)
                {
                    RandomItem = Random.Range(0, items.Length);
                    maxIterations++;
                }

                GameObject item = Instantiate(items[RandomItem], tempPosition, Quaternion.identity);
                item.GetComponent<Item>().row = y;
                item.GetComponent<Item>().column = x;

                item.transform.parent = this.transform;
                item.name = "( " + y + ", " + x + " )";
                allItems[y, x] = item;
            }
        }
    }

    private bool MatchesAt(int row, int column, GameObject piece)
    {
        if(row > 1 && column > 1)
        {
            if(piece.CompareTag(allItems[row - 1, column].tag) && piece.CompareTag(allItems[row - 2, column].tag) ||
                piece.CompareTag(allItems[row, column - 1].tag) && piece.CompareTag(allItems[row, column - 2].tag))
            {
                return true;
            }

        }
        else
        {
            if(column > 1 && piece.CompareTag(allItems[row, column - 1].tag) && piece.CompareTag(allItems[row, column - 2].tag) ||
                row > 1 && piece.CompareTag(allItems[row - 1, column].tag) && piece.CompareTag(allItems[row - 2, column].tag))
            {
                return true;                
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int row, int column)
    {
        if(allItems[row, column].GetComponent<Item>().isMatched)
        {
            findMatches.currentMatches.Remove(allItems[row, column]);
            Destroy(allItems[row, column]);
            allItems[row, column] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(allItems[y, x] != null)
                {
                    DestroyMatchesAt(y, x);
                }
            }
        }
        StartCoroutine(DecreaseRowCoroutine());
    }

    private IEnumerator DecreaseRowCoroutine()
    {
        int nullCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(allItems[y, x] == null)
                {
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    allItems[y, x].GetComponent<Item>().row -= nullCount;
                    allItems[y, x] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCoroutine());
    }

    private void RefillBoard()
    {
        Debug.Log("RefillBoard\n");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (allItems[y, x] == null)
                {
                    Vector2 tempPosition = new Vector2(x, y + offSet);
                    int RandomItem = Random.Range(0, items.Length);
                    GameObject piece = Instantiate(items[RandomItem], tempPosition, Quaternion.identity);
                    allItems[y, x] = piece;
                    piece.GetComponent<Item>().row = y;
                    piece.GetComponent<Item>().column = x;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        Debug.Log("MatchesOnBoard\n");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (allItems[y, x] == null && allItems[y, x].GetComponent<Item>().isMatched)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCoroutine()
    {
        Debug.Log("FillBoardCoroutine\n");
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        yield return new WaitForSeconds(.5f);

        currentState = GameState.WaitingForActions;
    }
}
