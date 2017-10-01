using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Dictionary <string, ArrayList> inputDictionary;

    private static InputManager inputManager;

    public static InputManager instance
    {
        get
        {
            if (!inputManager)
            {
                inputManager = FindObjectOfType (typeof (InputManager)) as InputManager;

                if (!inputManager)
                {
                    Debug.LogError ("There needs to be one active InputManger script on a GameObject in your scene.");
                }
                else
                {
                    inputManager.Init (); 
                }
            }

            return inputManager;
        }
    }

    void Init ()
    {
        if (inputDictionary == null)
        {
            inputDictionary = new Dictionary<string, ArrayList>();
        }
    }

    public void Map(string actionName,string inputName)
    {
    	ArrayList thisList = null;
        if (instance.inputDictionary.TryGetValue (actionName, out thisList))
        {
            thisList.Add(inputName);
        } 
        else
        {
            thisList = new ArrayList ();
            thisList.Add(inputName);
            instance.inputDictionary.Add(actionName, thisList);
        }
    }

}

