using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to represent a single node.
public class Node : MonoBehaviour
{ 
    //The left node.
    private Node left;
    //The right node.
    private Node right;

    //Node's Value Text GameObject.
    [SerializeField]
    private GameObject valueGameObject = null;

    //Node's InputField GameObject.
    [SerializeField]
    private GameObject readTextGameObject = null;

    //The GameManager. 
    private GameManager gameManager = null;

    //The value of the node.
    private int value;

    public static int arr_counter = 0;

    //Start is called at the first frame.
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Level Designer").GetComponent<GameManager>();
    }

    //The height of the binary tree.
    public int height(Hashtable nodePath)
    {
        //Height of left subtree.
		int left_height = 0;
        //Height of right subtree.
		int right_height = 0;

        if (nodePath.ContainsKey(this))
        {
            return -1;
        }
        nodePath.Add(this, null);
        
        //Basis Case (tree is just one node). Return 1 in basis case.
        if (left==null && right==null)
        {
            return 1;
        }
        //If left is not null, use recursion to get height of left subtree.
		if (left != null)
		{
			left_height = left.height(nodePath);
		}
        //If right is not null, use recursion to get height of right subtree.
        if (right != null)
		{
			right_height = right.height(new Hashtable(nodePath));
		}

        if(left_height==-1 || right_height == -1)
        {
            return -1;
        }
		//Recursive definition of height of binary tree.
		return Mathf.Max(left_height, right_height) + 1;
    }

    //Function to create the tree.
    public bool createTree()
    {
        //The string in the Node's InputField.
        string string_read = readTextGameObject.GetComponent<Text>().text;
        //String array to store left and right strings.
        string[] string_read_split;
        //Index of left node.
        int left_int;
        //Index of right node.
        int right_int;

        //Try/catch used to catch invalid inputs.
        try
        {
            if (string_read.Contains("."))
            {
                StartCoroutine(gameManager.popUp("Error: Node values were entered incorrectly. Please enter two valid comma seperated integers.", false));
                return false;
            }
            //Comma-seperate the string into two strings.
            string_read_split = string_read.Split(',');
            //If there is more than one comma, display an error and return false.
			if (string_read_split.Length != 2)
			{
				StartCoroutine(gameManager.popUp("Error: Node values were entered incorrectly. Please enter two valid comma seperated integers.", false));
				return false;
			}
            left_int = int.Parse(string_read_split[0]);
            //If the left value is not within an acceptable range, or is a floating point value, display an error and return false.
            if (left_int < -1 || left_int >= gameManager.getCurrentNodes().Count)
            {
                StartCoroutine(gameManager.popUp("Error: Node values were entered incorrectly. Please enter two valid comma seperated integers.", false));
                return false;
            }
            right_int = int.Parse(string_read_split[1]);
            //If the right value is not within an acceptable range, or is a floating point value, display an error and return false.
            if (right_int < -1 || right_int >= gameManager.getCurrentNodes().Count)
            {
                StartCoroutine(gameManager.popUp("Error: Node values were entered incorrectly. Please enter two valid comma seperated integers.", false));
                return false;
            }
        }
        //For any invalid inputs, display an error and return false.
        catch
        {
            StartCoroutine(gameManager.popUp("Error: Node values were entered incorrectly. Please enter two valid comma seperated integers.", false));
            return false;
        }
        //If left_int is -1, then there is no left node.
		if (left_int == -1)
		{
			left = null;
		}
        //Otherwise, the left node is gameManager's currentNode[left_int].
		else
		{
			left = gameManager.getCurrentNodes()[left_int].GetComponent<Node>();
		}
        //If the right_int is -1, then there is no right node.
		if (right_int == -1)
		{
			right = null;
		}
        //Otherwise, the right node is gameManager's currentNode[right_int].
		else
		{
			right = gameManager.getCurrentNodes()[right_int].GetComponent<Node>();
        }
        //Since the left and right nodes were gotten correctly, return true.
        return true;
    }


    private void storeInOrder(Node root, int[] arr)
    {
        if (root == null)
            return;
        storeInOrder(root.left, arr);
        arr[arr_counter] = value;
        arr_counter++;
        storeInOrder(root.right, arr);
    }

    public bool isBST()
    {
        int[] arr = new int[gameManager.getCurrentNodes().Count];
        arr_counter = 0;
        storeInOrder(this, arr);
        for(int i=1; i<arr.Length; i++)
        {
            if(arr[i] < arr[i - 1])
            {
                return false;
            }
        }
        return true;
    }

    //Function to determine if player determined the correct tree.
    public bool isCorrectTree()
    {
        /* Basis Cases:
         * 1) Return true if tree is just one node with no children.
         * 2) Tell the user the correct tree was not produced and return false
         * if the parent node only has one child.
         */

        if (gameManager.addToTransversedNodes(this) == true)
        {
            gameManager.setNodeCounter(gameManager.getNodeCounter() + 1);
        }

        if (left==null && right==null)
        {
            return true;
        }
        if(left==null ^ right == null)
		{
			StartCoroutine(gameManager.popUp("The correct tree was not produced.", false));
			return false;
		}

        // If it is not the case that left.value <= node.value <= right.value, then tell the user that the correct tree was not produced and return false.
        if(left.getValue()>value || right.getValue() < value)
        {
            StartCoroutine(gameManager.popUp("The correct tree was not produced.", false));
            return false;
        }

        //Storing the heights of the left subtree and the right subtree in integer variables.
        int left_height = left.height(new Hashtable());
        int right_height = right.height(new Hashtable());

        if (left_height==-1 || right_height == -1)
        {
            StartCoroutine(gameManager.popUp("The correct tree was not produced.", false));
            return false;
        }

        //If the binary tree fits the recursive definition of a balanced binary tree, return true.
        if(left_height-right_height==0 && left.isCorrectTree()==true && right.isCorrectTree() == true)
        {
            return true;
        }
		//Else, tell the user that the correct tree was not produced and return false.
		StartCoroutine(gameManager.popUp("The correct tree was not produced.",false));
        return false;
    }

    //Getter method for node value.
    public int getValue()
    {
        return value;
    }

    //Setter method for node value.
    public void setValue(int val)
    {
        value = val;
    }

    //Function to update node's text to display value.
    public void displayValue()
    {
        valueGameObject.GetComponent<Text>().text = value.ToString();
    }


}
