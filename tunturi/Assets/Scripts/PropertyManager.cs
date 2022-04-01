using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComparisonType
{
    LessThan,
    LessThanOrEqual,
    Equal,
    GreaterThanOrEqual,
    GreaterThan
}

public enum PropertyType
{
    Item,
    Experience,
}


[System.Serializable]
public struct PropertyModificationRequest
{
    public PropertyType type;
    public int value;
    public int id;
}

[System.Serializable]
public struct PropertyComparisonQuery
{
    public PropertyType type;
    public int value;
    public int id;
    public ComparisonType comparisonType;
}

public class PropertyManager : MonoBehaviour
{
    public void Modify(PropertyModificationRequest request)
    {
        switch (request.type)
        {
            // TODO: EXAMPLE! Replace Experience handling with an actual implementation
            case PropertyType.Experience:
                Debug.Log($"Example: Received {request.value} experience!");
                break;
            default:
                Debug.Log($"Unhandled PropertyModificationRequest! Type: {request.type}");
                break;
        }
    } 
    public bool Compare(PropertyComparisonQuery query)
    {
        // TODO: replace with an actual implementation: compareTo value is supposed to be fetched from stats, gold amount etc with eg. a switch
        int compareTo = 5;
        int comparisonResult = query.value.CompareTo(compareTo);
        return query.comparisonType switch
        {
            ComparisonType.LessThan           => comparisonResult == -1,
            ComparisonType.LessThanOrEqual    => comparisonResult <  1,
            ComparisonType.Equal              => comparisonResult == 0,
            ComparisonType.GreaterThan        => comparisonResult == 1,
            ComparisonType.GreaterThanOrEqual => comparisonResult > -1,
            _ => false  // default, should never be reached. Sole purpose of existence is to suppress warnings in Unity 
        };
    }
}
