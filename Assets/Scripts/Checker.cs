using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Class to represent the button that checks if the tree produced is valid or not.
public class Checker : MonoBehaviour, IPointerDownHandler
{
    //GameObject where root index is entered.
	[SerializeField]
	GameObject root_text = null;

    //GameManager GameObject.
	[SerializeField]
	GameManager gameManager = null;

    //Integer index of root.
    int root_index;

    //When the button is clicked...
    public void OnPointerDown(PointerEventData pointerEventData)
    {

        gameManager.setNodeCounter(0);
        gameManager.clearTransversedNodes();

        /* Loop through all the nodes and create the binary tree.
         * If an error is encountered, exit.
         */
        for (int i=0; i<gameManager.getCurrentNodes().Count; i++)
		{
			if (gameManager.getCurrentNodes()[i].GetComponent<Node>().createTree() == false)
			{
				return;
			}
		}

		//Get the root index from the input field.
		try
		{
			root_index = int.Parse(root_text.GetComponent<Text>().text);
            if (root_index < 0 || root_index >= gameManager.getCurrentNodes().Count || root_text.GetComponent<Text>().text.Contains("."))
            {
                StartCoroutine(gameManager.popUp("Error: Please enter a valid root index.", false));
                return;
            }
        }
		catch
		{
            StartCoroutine(gameManager.popUp("Error: Please enter a valid root index.", false));
            return;
		}
        /*Use the GameManager to check if the correct tree was produced
         *with the root of the tree being GameManager's currentNodes[root_index].
         */
		if (gameManager.getCurrentNodes()[root_index].GetComponent<Node>().isCorrectTree() &&
            gameManager.getCurrentNodes()[root_index].GetComponent<Node>().isBST())
		{
            if (gameManager.getNodeCounter() == gameManager.getCurrentNodes().Count) {
                StartCoroutine(gameManager.popUp("Level passed!", true));
                return;
            }
		}
        StartCoroutine(gameManager.popUp("The correct tree was not produced.", false));
    }
}
