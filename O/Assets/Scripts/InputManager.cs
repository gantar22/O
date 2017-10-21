using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Dictionary <string, ArrayList> inputDictionary;
    //@invariant there are never multiple occurances of the same keycode in
    //           the pairs in the ArrayList. This would fuck everything up 
    //           but I'm pretty sure I respect this so I hope it's okay

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

    public void Map(string actionName,KeyCode inputName)
    //the action name is "object_function" and the input name
    //is "player_button". This makes it so that Get_Buttons
    // returns this is input name when actors want to know what
    //input to listen to.
    {
    	ArrayList thisList = null;
        if (instance.inputDictionary.TryGetValue (actionName, out thisList))
        {
            bool flag = true;
            foreach (Pair<KeyCode,int> input in thisList) {
                if (input.fst == inputName) {
                    flag = false;
                    input.snd++;
                }
            }
            if(flag) 
				thisList.Add(new Pair<KeyCode,int>(inputName,0));
        } 
        else
        {
            thisList = new ArrayList ();
            thisList.Add(new Pair<KeyCode,int>(inputName,1));
            instance.inputDictionary.Add(actionName, thisList);
        }
    }
    public bool Remove(string actionName,KeyCode inputName)
    {
    	ArrayList thisList = null;
        if (instance.inputDictionary.TryGetValue (actionName, out thisList)) {
            Pair<KeyCode,int> forRemove = null;
            foreach (Pair<KeyCode,int> input in thisList) {
                if (input.fst == inputName) {
                    if (input.snd > 1) {
                        input.snd--;
                        return true;
                    } else {
                        forRemove = input;
                    }
                }
            }
            if(forRemove != null) {
                thisList.Remove(forRemove);
                return true;
            }
            return false;
        }
        return false;
            
    }
    public ArrayList Get_Buttons(string actionName)
    {
    	ArrayList thisList = null;
    	if (instance.inputDictionary.TryGetValue (actionName, out thisList)) {
            ArrayList r = new ArrayList();
            foreach (Pair<KeyCode,int> input in thisList) {
                r.Add(input.fst);
            }
    		return r;
        }
    	return new ArrayList();
    }



}

