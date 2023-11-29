using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextValidator : MonoBehaviour
{
    [SerializeField] List<InputFieldChecker> inputFields;

    [Serializable]
    public class InputFieldChecker
    {
        public List<TMP_InputField> inputFields;
        public List<char> illegalChars;
    }

    public void UpdateOnValidateInput()
    {
        foreach (InputFieldChecker inputFieldChecker in inputFields)
            foreach (TMP_InputField inputField in inputFieldChecker.inputFields)
                inputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValdiateChar(addedChar, inputFieldChecker); };
    }

    private char ValdiateChar(char charToValidate, InputFieldChecker inputFieldChecker)
    {
        return inputFieldChecker.illegalChars.Contains(charToValidate) ? '\0' : charToValidate;
    }
}