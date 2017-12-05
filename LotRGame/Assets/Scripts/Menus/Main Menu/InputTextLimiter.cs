using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputTextLimiter : MonoBehaviour
{
    //The reference to this object's input field component
    private InputField ourInput;
    //The maximum number of characters for the input field text
    public int maxStringLength = 10;

    //The list of characters that aren't allowed
    private List<char> illegalChars; 



    //Function called when this object is created
    private void Awake()
    {
        this.ourInput = this.GetComponent<InputField>();
        this.illegalChars = new List<char>() { '<', '>', ':', '\\', '/', '\"', '|', '?', '*' };
    }


    //Function called from our InputField to make sure the text is within the correct length and doesn't contain illegal characters
    public void CheckTextCharacters(string textString_)
    {
        //The string that gets checked
        string checkedString = textString_;

        //If the string is beyond the max length, we trim it down
        if(checkedString.Length > this.maxStringLength)
        {
            checkedString = checkedString.Remove(this.maxStringLength, checkedString.Length - this.maxStringLength);
        }

        //Making sure all of the characters in the string are valid
        foreach(char ic in this.illegalChars)
        {
            //Looping through each character in the string to see if it's illegal
            int charIndex = 0;
            int textLength = checkedString.Length;
            while(charIndex < textLength)
            {
                //if we find an illegal character, we remove it and reduce the current char index
                if(checkedString[charIndex] == ic)
                {
                    checkedString = checkedString.Remove(charIndex, 1);
                    charIndex -= 1;
                    textLength -= 1;
                }

                //moving to the next index
                charIndex += 1;
            }
        }

        //Setting the text to the checked string
        this.ourInput.textComponent.text = checkedString;
        this.ourInput.text = checkedString;
    }
}
