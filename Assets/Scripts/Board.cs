using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;

    public int width;
    public int height;
    public int offSet;

    public GameObject tilePrefab;
    public GameObject[] items;
    public GameObject[,] allItems;

    private BackgroundTile[,] allTiles;

    private FindMatches findMatches;


    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();

        allTiles = new BackgroundTile[width, height];
        allItems = new GameObject[width, height]; 
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition,Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";

                int RandomItem = Random.Range(0, items.Length);

                int maxIterations = 0;

                while(MatchesAt(i, j, items[RandomItem]) && maxIterations < 100)
                {
                    RandomItem = Random.Range(0, items.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject item = Instantiate(items[RandomItem], tempPosition, Quaternion.identity);
                item.GetComponent<Item>().row = j;
                item.GetComponent<Item>().column = i;

                item.transform.parent = this.transform;
                item.name = "( " + i + ", " + j + " )";
                allItems[i, j] = item;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if(allItems[column - 1, row].tag == piece.tag && allItems[column - 2, row].tag == piece.tag)
            {
                return true;
            }

            if (allItems[column, row - 1].tag == piece.tag && allItems[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if(row > 1 && allItems[column, row - 1].tag == piece.tag && allItems[column, row - 2].tag == piece.tag)
            {
                return true;                
            }

            if (column > 1 && allItems[column - 1, row].tag == piece.tag && allItems[column - 2, row].tag == piece.tag)
            {                
                return true;               
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if(allItems[column, row].GetComponent<Item>().isMatched)
        {
            findMatches.currentMatches.Remove(allItems[column, row]);
            Destroy(allItems[column, row]);
            allItems[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allItems[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCoroutine());
    }

    private IEnumerator DecreaseRowCoroutine()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allItems[i, j] == null)
                {
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    allItems[i, j].GetComponent<Item>().row -= nullCount;
                    allItems[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCoroutine());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allItems[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int RandomItem = Random.Range(0, items.Length);
                    GameObject piece = Instantiate(items[RandomItem], tempPosition, Quaternion.identity);
                    allItems[i, j] = piece;
                    piece.GetComponent<Item>().row = j;
                    piece.GetComponent<Item>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allItems[i, j] == null)
                {
                    if(allItems[i, j].GetComponent<Item>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCoroutine()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        yield return new WaitForSeconds(.4f);

        currentState = GameState.move;
    }
}
