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

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentItem = board.allItems[i, j];
                if (currentItem != null)
                {
                    if(i > 0 && i < board.width - 1)  //horizontal
                    {
                        GameObject leftItem = board.allItems[i - 1, j];
                        GameObject rightItem = board.allItems[i + 1, j];

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

                    if (j > 0 && j < board.height - 1)  //vertical
                    {
                        GameObject upItem = board.allItems[i, j + 1];
                        GameObject downItem = board.allItems[i, j - 1];
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
