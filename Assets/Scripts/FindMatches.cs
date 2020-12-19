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

    private IEnumerator FindAllMatchesCoroutine()
    {
        yield return new WaitForSeconds(.2f);

        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
                GameObject currentItem = board.allItems[y, x];
                if (currentItem != null)
                {
                    if(y > 0 && y < board.height - 1)  //horizontal
                    {
                        GameObject leftItem = board.allItems[y - 1, x];
                        GameObject rightItem = board.allItems[y + 1, x];

                        if(leftItem != null && rightItem != null && currentItem.CompareTag(leftItem.tag) && currentItem.CompareTag(rightItem.tag)) //if they exists
                        {       
                            if(!currentMatches.Contains(leftItem))                                     //this needs to rework
                            {
                                currentMatches.Add(leftItem);
                            }
                            leftItem.GetComponent<Item>().isMatched = true;
                            if (!currentMatches.Contains(rightItem))                                     
                            {
                                currentMatches.Add(rightItem);
                            }
                            rightItem.GetComponent<Item>().isMatched = true;
                            if (!currentMatches.Contains(currentItem))
                            {
                                currentMatches.Add(currentItem);
                            }
                            currentItem.GetComponent<Item>().isMatched = true;
                        }
                    }

                    if (x > 0 && x < board.width - 1)  //vertical
                    {
                        GameObject upItem = board.allItems[y, x + 1];
                        GameObject downItem = board.allItems[y, x - 1];
                        if (upItem != null && downItem != null && (currentItem.CompareTag(upItem.tag) && currentItem.CompareTag(downItem.tag))) 
                        {
                            if (!currentMatches.Contains(upItem))                                //same here
                            {
                                currentMatches.Add(upItem);
                            }
                            upItem.GetComponent<Item>().isMatched = true;
                            if (!currentMatches.Contains(downItem))
                            {
                                currentMatches.Add(downItem);
                            }
                            downItem.GetComponent<Item>().isMatched = true;
                            if (!currentMatches.Contains(currentItem))
                            {
                                currentMatches.Add(currentItem);
                            }
                            currentItem.GetComponent<Item>().isMatched = true;
                        }
                    }
                }
            }
        }

    }
}
