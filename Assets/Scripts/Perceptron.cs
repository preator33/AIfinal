using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron : MonoBehaviour
{
    public float[] featureVector;
    private float[] weights;
    private int featureVectorLength;
    private float bias;

    public float Sigmoid(float num)
    {
        const float e = 2.71828182845904523536f;
        return 1.0f / (1.0f + Mathf.Pow(e, -num));
    }

    public void Initialize(int featureVectorSize)
    { // basic constructor 
        weights = new float[featureVectorSize];
        featureVectorLength = featureVectorSize;
        featureVector = new float[featureVectorLength];
        for (int i = 0; i < featureVectorSize; ++i)
        {
            weights[i] = 0.5f;
        }
    }

    public Perceptron()
    { // basic constructor 
        weights = new float[4];
        featureVectorLength = 4;
        featureVector = new float[featureVectorLength];
        for (int i = 0; i < 4; ++i)
        {
            weights[i] = 0.5f;
        }
    }
    public Perceptron(int featureVectorSize)
    { // basic constructor 
        weights = new float[featureVectorSize];
        featureVectorLength = featureVectorSize;
        featureVector = new float[featureVectorLength];
        for (int i = 0; i < featureVectorSize; ++i)
        {
            weights[i] = 0.5f;
        }
    }

    Perceptron(Perceptron parent)
    { //copy constructor
        featureVectorLength = parent.featureVectorLength;

        for (int i = 0; i < featureVectorLength; i++)
        {
            featureVector[i] = parent.featureVector[i];
            weights[i] = parent.weights[i];
        }
        bias = parent.bias;
    }

    public void RandomizeValues()
    {
        for (int i = 0; i < featureVector.Length; i++)
        {
            weights[i] = Random.Range(-2.0f, 2.0f);
        }
        bias = Random.Range(-2.0f, 2.0f);
    }

    public Perceptron Crossover(Perceptron parent1, Perceptron parent2)
    {
        Perceptron result;
        result = new Perceptron(parent1.featureVector.Length);
        for (int i = 0; i < result.featureVector.Length; ++i)
        {
            result.weights[i] = (parent1.weights[i] + parent2.weights[i]) / 2.0f;
        }

        result.bias = (parent1.bias + parent2.bias) / 2.0f;

        return result;
    }

    public float Evaluate(float fvect)
    {

        float result = 0.0f;
        for (int i = 0; i < featureVector.Length; ++i)
        {
            result += featureVector[i] * weights[i];
        }

        result += bias;

        return Sigmoid(result);
    }
}