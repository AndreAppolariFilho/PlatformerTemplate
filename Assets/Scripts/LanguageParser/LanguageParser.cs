using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageParser
{
    // Start is called before the first frame update
    public static bool IsValid(string word)
    {
        List<char> openings = new List<char>();
        List<char> closings = new List<char>();
        foreach(char c in word)
        {
            if(c=='[')
            {
                openings.Add(c);
            }
            else if(c == ']')
            {
                closings.Add(c);
            }
        }
        
        return openings.Count == closings.Count;
    }
    public static LanguageEnums.Types GetExecution(string word, int cpu)
    {
        if(word[cpu] == 'U')
        {
            return LanguageEnums.Types.upArrow;
        }
        if(word[cpu] == 'D')
        {
            return LanguageEnums.Types.downArrow;
        }
        if(word[cpu] == 'R')
        {
            return LanguageEnums.Types.rightArrow;
        }
        if(word[cpu] == 'L')
        {
            return LanguageEnums.Types.leftArrow;
        }
        if(word[cpu] == '[')
        {
            return LanguageEnums.Types.beginForLoop;
        }
        if(word[cpu] == ']')
        {
            return LanguageEnums.Types.endForLoop;
        }
        if(word[cpu] == '{')
        {
            return LanguageEnums.Types.turnAntiClockWise;
        }
        if(word[cpu] == '}')
        {
            return LanguageEnums.Types.turnClockWise;
        }
        return LanguageEnums.Types.doNothing;
    }
    public static int GetNumberOfTimes(string word, int start_position, out int cpu)
    {
        string number = "";
        
        if (word[start_position] == '[')
        {
            start_position += 1;
        }
        cpu = start_position;
        if(word[start_position] == '*')
        {
            cpu += 1;
            return Int32.MaxValue;
        }
        
        while(true)
        {
            
            if(word[start_position] >= '0' && word[start_position] <= '9')
            {
                number += word[start_position];
            }
            else
            {
                break;
            }
            start_position++;
        }
        cpu = start_position;
        //Debug.Log("Number to Parse " + number);
        return Int32.Parse(number);
    }
    public static Dictionary<int, int> GetTokensForLoop(string word)
    {
        Dictionary<int, int> tokens = new Dictionary<int, int>();
        Queue<int> openings = new Queue<int>();
        Stack<int> clossings = new Stack<int>();
        for(int i = 0; i < word.Length; i++)
        {
            if(word[i] == '[')
            {
                openings.Enqueue(i);
            }
            else if(word[i] == ']')
            {
                clossings.Push(i);
            }
        }
        while(true)
        {
            if(openings.Count == 0)
            {
                break;
            }
            int opening_key = openings.Dequeue();
            int clossing_key = clossings.Pop();

            tokens[opening_key] = clossing_key;
        }
        return tokens;
    }
}
