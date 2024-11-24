using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public TMP_InputField displayField;

    private string currentInput = "";

    public void OnButtonClick(string value)
    {
        // Prevent adding operators at the start
        if (string.IsNullOrEmpty(currentInput) && (value == "+" || value == "-" || value == "×" || value == "÷"))
        {
            return;
        }

        // Prevent consecutive operators
        if (!string.IsNullOrEmpty(currentInput) &&
            (currentInput[currentInput.Length - 1] == '+' ||
             currentInput[currentInput.Length - 1] == '-' ||
             currentInput[currentInput.Length - 1] == '×' ||
             currentInput[currentInput.Length - 1] == '÷') &&
            (value == "+" || value == "-" || value == "×" || value == "÷"))
        {
            return;
        }

        // Update input and display
        currentInput += value;
        displayField.text = currentInput;
    }

    public void OnClearButtonClick()
    {
        currentInput = "";
        displayField.text = "";
    }

    public void OnEqualButtonClick()
    {
        if (string.IsNullOrEmpty(currentInput)) return;

        // Sanitize input before sending it to the calculator
        string sanitizedInput = SanitizeInput(currentInput);

        double result = CalculatorLogic.EvaluateExpression(sanitizedInput);

        // Display the result or error
        displayField.text = double.IsNaN(result) || double.IsInfinity(result) ? "Error" : result.ToString();

        // Reset current input
        currentInput = result.ToString();
    }


    public void OnBackspaceClick()
    {
        if (!string.IsNullOrEmpty(currentInput))
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            displayField.text = currentInput;
        }
    }

    public void OnValueChanged(string input)
    {
        // Replace invalid characters
        string sanitizedInput = input.Replace('*', '×').Replace('/', '÷');

        // Prevent invalid operators at the start
        if (sanitizedInput.Length > 0 && (sanitizedInput[0] == '+' || sanitizedInput[0] == '-' || sanitizedInput[0] == '×' || sanitizedInput[0] == '÷'))
        {
            sanitizedInput = sanitizedInput.Substring(1);
        }

        // Assign the sanitized input back
        currentInput = sanitizedInput;
        displayField.text = sanitizedInput;
    }

    private string SanitizeInput(string input)
    {
        return input.Replace('*', '×').Replace('/', '÷');
    }

}
