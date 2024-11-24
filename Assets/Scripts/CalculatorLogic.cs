using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculatorLogic
{
    public static double EvaluateExpression(string input)
    {
        try
        {
            List<string> tokens = Tokenize(input);
            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    values.Push(number);
                }
                else
                {
                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(token))
                    {
                        double right = values.Pop();
                        double left = values.Pop();
                        string op = operators.Pop();
                        values.Push(ApplyOperator(left, right, op));
                    }
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                double right = values.Pop();
                double left = values.Pop();
                string op = operators.Pop();
                values.Push(ApplyOperator(left, right, op));
            }

            return values.Pop();
        }
        catch
        {
            return double.NaN; // Return NaN for invalid expressions
        }
    }

    private static List<string> Tokenize(string input)
    {
        List<string> tokens = new List<string>();
        string number = "";

        foreach (char c in input)
        {
            if (char.IsDigit(c) || c == '.') // Include digits and dots in the number
            {
                number += c;
            }
            else if (c == '+' || c == '-' || c == '×' || c == '÷') // Recognize only supported operators
            {
                if (!string.IsNullOrEmpty(number))
                {
                    tokens.Add(number); // Add the current number to tokens
                    number = "";
                }
                tokens.Add(c.ToString()); // Add the operator to tokens
            }
        }

        if (!string.IsNullOrEmpty(number))
        {
            tokens.Add(number); // Add the last number if any
        }

        return tokens;
    }


    private static int Precedence(string op)
    {
        return (op == "×" || op == "÷") ? 2 : 1;
    }

    private static double ApplyOperator(double left, double right, string op)
    {
        return op switch
        {
            "+" => left + right,
            "-" => left - right,
            "×" => left * right,
            "÷" => right != 0 ? left / right : double.NaN, // Avoid divide-by-zero
            _ => double.NaN // Invalid operator
        };
    }


}
