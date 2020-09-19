using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : Participant
{

    public void BeginTurn()
    {
        if (Cards != null && Cards.Count == 2)
        {
            Cards[1].Flip();
            Check();
        }
    }
}
