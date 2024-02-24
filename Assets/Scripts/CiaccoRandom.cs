using System;
using UnityEngine;

public class CiaccoRandom
{
    private static int tree ;

    // set the seed
    public void setSeed(int seed)
    {
        tree = Math.Abs(seed) % 9999999 + 1;
        getRand(0, 9999999);
    }

    // get a random int between $min and $max [both included]
    public int getRand(int min, int max)
    {
        //Debug.Log("tree " + tree);
        tree = (tree * 123) % 69522569;
        //Debug.Log("tree " + tree);
        return tree % (max - min + 1) + min;
    }
}