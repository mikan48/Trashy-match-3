using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;

    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatchhes()
    {
        StartCoroutine(FindAllMatchesCoroutine());
    }

    private void AddToListAndMatch(GameObject item)
    {
        if (!currentMatches.Contains(item))
        {
            currentMatches.Add(item);
        }
        item.GetComponent<Item>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject item1, GameObject item2, GameObject item3)
    {
        AddToListAndMatch(item1);
        AddToListAndMatch(item2);
        AddToListAndMatch(item3);
    }

    private IEnumerator FindAllMatchesCoroutine()
    {
        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentItem = board.allItems[i, j];

                if (currentItem != null)
                {
                    if (i > 0 && i < board.width - 1)  //horizontal
                    {
                        GameObject leftItem = board.allItems[i - 1, j];
                        GameObject rightItem = board.allItems[i + 1, j];

                        if (leftItem != null && rightItem != null && (leftItem.tag == currentItem.tag && rightItem.tag == currentItem.tag)) //if they exists
                        {
                            GetNearbyPieces(leftItem, currentItem, rightItem);
                        }
                    }

                    if (j > 0 && j < board.height - 1)  //vertical
                    {
                        GameObject upItem = board.allItems[i, j + 1];
                        GameObject downItem = board.allItems[i, j - 1];

                        if (upItem != null && downItem != null && (upItem.tag == currentItem.tag && downItem.tag == currentItem.tag))
                        {
                            GetNearbyPieces(upItem, currentItem, downItem);
                        }
                    }
                }
            }
        }

        //yield return new WaitForSeconds(.3f);
    }
}
