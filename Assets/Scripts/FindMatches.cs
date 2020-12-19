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

        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
<<<<<<< HEAD
                GameObject currentItem = board.allItems[y, x];
                if (currentItem != null)
                {
                    if(y > 0 && y < board.height - 1)  //horizontal
=======
                GameObject currentItem = board.allItems[i, j];

                if (currentItem != null)
                {
                    if (i > 0 && i < board.width - 1)  //horizontal
>>>>>>> cfb494f68ee1b65ad3d0ebe01fe0bb62dc0f5f8c
                    {
                        GameObject leftItem = board.allItems[y - 1, x];
                        GameObject rightItem = board.allItems[y + 1, x];

                        if (leftItem != null && rightItem != null && (leftItem.tag == currentItem.tag && rightItem.tag == currentItem.tag)) //if they exists
                        {
                            GetNearbyPieces(leftItem, currentItem, rightItem);
                        }
                    }

                    if (x > 0 && x < board.width - 1)  //vertical
                    {
<<<<<<< HEAD
                        GameObject upItem = board.allItems[y, x + 1];
                        GameObject downItem = board.allItems[y, x - 1];
                        if (upItem != null && downItem != null && (currentItem.CompareTag(upItem.tag) && currentItem.CompareTag(downItem.tag))) 
=======
                        GameObject upItem = board.allItems[i, j + 1];
                        GameObject downItem = board.allItems[i, j - 1];

                        if (upItem != null && downItem != null && (upItem.tag == currentItem.tag && downItem.tag == currentItem.tag))
>>>>>>> cfb494f68ee1b65ad3d0ebe01fe0bb62dc0f5f8c
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
