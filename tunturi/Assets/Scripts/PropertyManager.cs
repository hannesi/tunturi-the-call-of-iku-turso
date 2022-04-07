using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyType
{
    Item,
    Experience,
    Dmg,
    Hp,
    Armor,
    Ap
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
}


public class PropertyManager : MonoBehaviour
{
    // PartyMember whomst'd is used for character specific queries
    public PartyMember target;
    public void Modify(PropertyModificationRequest request)
    {
        switch (request.type)
        {
            case PropertyType.Armor:
                target.adjustARM(request.value);
                Debug.Log($"Received {request.value} armor! New armor value: {target.getARM()}");
                break;
            default:
                Debug.Log($"Unhandled PropertyModificationRequest! Type: {request.type}");
                break;
        }
    } 
    public (bool, int) Compare(PropertyComparisonQuery query)
    {
        // TODO: replace with an actual implementation: charValue value is supposed to be fetched from stats, gold amount etc with eg. a switch
        int charValue = query.type switch {
            PropertyType.Dmg => target.getDMG(),
            _ => -1
        };
        // TODO: remove logging below after debugging
        if (charValue == -1) Debug.Log($"Unhandled PropertyComparisonQuery type: {query.type}");
        return (query.value.CompareTo(charValue) < 1, charValue);
    }
}
