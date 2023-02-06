using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class to represent Level
[System.Serializable]
public class Level
{
    [SerializeField]
    //List of nodes with their values.
    private List<int> nodeValues = new List<int>();

    //Getter method for nodeValues.
    public List<int> getNodeValues()
    {
        return nodeValues;
    }
}

//Class to represent GameManager.
public class GameManager : MonoBehaviour
{
    //List of levels.
    [SerializeField]
    private List<Level> levels = new List<Level>();
    //Build index of the credits scene.
	[SerializeField]
	private int credits_scene = 0;
    //Position of the first created node.
	[SerializeField]
	private Vector3 original_position = new Vector3(0, 0,0);
    //Y-distance between each consecutive node.
	[SerializeField]
	private float y_offset = 0;
    //Prefab for the nodes.
    [SerializeField]
    private GameObject nodePrefab = null;

    //The color of the text when level is passed. 
    [SerializeField]
    private Color successfulColor = new Color(0, 0, 0);

    //The color of the text when an error has occurred (either through reading input or failing level).
    [SerializeField]
    private Color errorColor = new Color(0, 0, 0);

    [SerializeField]
    private GameObject messageGameObject = null;

    [SerializeField]
    private float messageDuration = 0.0f;

    [SerializeField]
    private Hashtable transversedNodes = new Hashtable();

    //List of the current nodes.
    List<GameObject> currentNodes = new List<GameObject>();

    private int nodeCounter = 0;

    //Number to represent current level (0-based indexing).
	private int current_level;


    public int getNodeCounter()
    {
        return nodeCounter;
    }

    public void setNodeCounter(int val)
    {
        nodeCounter = val;
    }
    //Function to create the level.
    private void createLevel()
    {
        //Set the y_position to the original position's y.
        float y_position = original_position.y;

        //If all levels are passed, go to the credits and end the function.
        if (current_level >= levels.Count)
        {
            SceneManager.LoadScene(credits_scene);
            return;
        }

        //If there are previous nodes from the past level, delete all of them.
        while (currentNodes.Count > 0)
        {
            GameObject obj = currentNodes[0];
            currentNodes.RemoveAt(0);
            Destroy(obj);
        }

        //For all the nodes in the level...
        for (int i = 0; i < levels[current_level].getNodeValues().Count; i++)
        {
            //Create a node.
            GameObject node = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity);
            //Change the y_position for the next node.
            y_position += y_offset;
            //Set the node's value correctly.
            node.gameObject.GetComponent<Node>().setValue(levels[current_level].getNodeValues()[i]);
            //Update the node's text to display the new value.
            node.gameObject.GetComponent<Node>().displayValue();

            //Set the node's parent to the Canvas.
            node.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            //Set the node's position correctly.
            node.GetComponent<RectTransform>().anchoredPosition = new Vector3(original_position.x, y_position, original_position.z);
            //Update currentNodes list to contain all nodes from this current level.
            currentNodes.Add(node);
        }
    }

    public void clearTransversedNodes()
    {
        transversedNodes.Clear();
    }

    public Hashtable getTransversedNodes()
    {
        return transversedNodes;
    }

    public bool addToTransversedNodes(Node val)
    {
        if (transversedNodes.ContainsKey(val))
        {
            return false;
        }
        transversedNodes.Add(val, null);
        return true;
    }
    //Getter method for getting nodes.
    public List<GameObject> getCurrentNodes()
    {
        return currentNodes;
    }

    //Getter method for succesful color.
    public Color getSuccessfulColor(){
        return successfulColor;
    }

    //Setter method for error color.
    public Color getErrorColor(){
        return errorColor;
    }

    //Method to pop up text.
    public IEnumerator popUp(string message, bool successful)
    {
        //Set the message text's correctly.
        messageGameObject.GetComponent<Text>().text = message;
        //Set the message text's color correctly based on if the player passed the level or not.
        if(successful)
            messageGameObject.GetComponent<Text>().color = successfulColor;
        else
            messageGameObject.GetComponent<Text>().color = errorColor;
        //Display the message.
        messageGameObject.SetActive(true);
        //After a few seconds, hide the message.
        yield return new WaitForSeconds(messageDuration);
        messageGameObject.SetActive(false);
        //If this is a message of success, move on to the next level.
        if (successful)
        {
            current_level+=1;
            createLevel();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the level to the first level.
        current_level = 0;
        //Create the first level at start.
        createLevel();
    }

}
