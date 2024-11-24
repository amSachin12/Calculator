using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public TMP_InputField displayField;

    private string currentInput = "";

    private bool isResultDisplayed = false;


    public void OnButtonClick(string buttonValue)
    {
        // If a result was displayed
        if (isResultDisplayed)
        {
            // If the button is an operator, append it to the result for chaining
            if (buttonValue == "+" || buttonValue == "-" || buttonValue == "×" || buttonValue == "÷")
            {
                currentInput = displayField.text + buttonValue;
            }
            else
            {
                // Otherwise, clear the display for new input
                currentInput = buttonValue;
            }

            isResultDisplayed = false; // Reset the flag
        }
        else
        {
            // Prevent multiple dots
            if (buttonValue == "." && currentInput.Contains(".")) return;

            currentInput += buttonValue;
        }

        // Update the display
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

        string sanitizedInput = SanitizeInput(currentInput);

        double result = CalculatorLogic.EvaluateExpression(sanitizedInput);

        // Display the result or error
        displayField.text = double.IsNaN(result) || double.IsInfinity(result) ? "Error" : result.ToString();

        // Store the result in current input for chaining calculations
        currentInput = result.ToString();

        // Set the result flag to true
        isResultDisplayed = true;
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
