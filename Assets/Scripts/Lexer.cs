using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexer 
{
    public static HashSet<string> tokens = new HashSet<string>();

    public static List<string> tokenize(string s)
    {
        s = s.ToLower();
        List<string> tokens = new List<string>();

        while (s.Length > 0)
        {
            bool found = false;
            for (int i = s.Length; i > 0; i--)
            {
				string token = s.Substring(0, i);
				if (Lexer.tokens.Contains(token))
                {
					tokens.Add(token);
					s = s.Substring(i);
                    found = true;
                    break;
				}
			}
            
            if (!found)
            {
                Debug.LogError("Could not tokenize: " + s);
                return new List<string>();
            }
        }

        Debug.Log("Tokenized: " + string.Join(", ", tokens.ToArray()));

        return tokens;
    
    }
}


