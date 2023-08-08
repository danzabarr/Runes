using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public float speed;
    public float randomness;
    public float radius;

    [Range(0, 1)] public float damping;

    private Vector3 acceleration;
    private Vector3 velocity;
    private Vector3 target;

    public string input;

    void Update()
    {
        Lexer.tokens.Add("sha");
        Lexer.tokens.Add("om");
        Lexer.tokens.Add("el");
        Lexer.tokens.Add("va");

        List<string> tokens = Lexer.tokenize(input);
        for (int i = 0; i < tokens.Count; i++)
        {
            Debug.Log(tokens[i]);
		}

        acceleration = (target - transform.localPosition) * Time.deltaTime * speed;
        
        velocity += acceleration * Time.deltaTime;
        velocity *= (1 - damping);
        
        transform.localPosition += velocity;

        if (Random.value < randomness)
            target = Random.insideUnitSphere * radius;
    }
}
