using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemanNameGenerator : MonoBehaviour
{
    string[] names = 
    {
        "Grug",
        "Grog",
        "Gruk",
        "Krag",
        "Thok",
        "Urg",
        "Og",
        "Glok",
        "Thog",
        "Snork",
        "Brunk",
        "Glog",
        "Ugg",
        "Krunk",
        "Grish",
        "Thunk",
        "Gorp",
        "Trog",
        "Flug",
        "Snok",
        "Krug",
        "Sprog",
        "Knork",
        "Klork",
        "Druk",
        "Skrunk",
        "Skrunkl",
        "Krog",
        "Gork",
        "Borsk",
        "Thug",
        "Brug",
        "Drask",
        "Frunk",
        "Gnok",
        "Hruk",
        "Ig",
        "Jog",
        "Klunk",
        "Lug",
        "Mork",
        "Nug",
        "Ork",
        "Pluk",
        "Quag",
        "Runk",
        "Skag",
        "Trunk",
        "Vug",
        "Wok",
        "Xog",
        "Yuk",
        "Zank",
        "Brog",
        "Crank",
        "Dork",
        "Fruk",
        "Glunk",
        "Hork",
        "Jug",
        "Krank",
        "Lork",
        "Mug",
        "Nork",
        "Pruk",
        "Quark",
        "Rag",
        "Skulk",
        "Trag",
        "Vork",
        "Wruk",
        "Xunk",
        "Yog",
        "Zug",
        "Blag",
        "Crog",
        "Dusk",
        "Hug",
        "Junk",
        "Lunk",
        "Musk",
        "Plog",
        "Quork",
        "Rusk",
        "Skork",
        "Zag",
        "Brusk",
        "Crug",
        "Drunk",
        "Flog",
        "Glug",
        "Husk",
        "Skog"
    };

    string[] connectors =
    {
        "en",
        "kr",
        "an",
        "on",
        "ul",
        "ik",
        "or",
        "ing",
        "og",
        "uk",
        "ek",
        "ruk",
        "un",
        "ank",
        "esk",
        "unk",
        "el"
    };

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        print(GenerateRandomName());
    }
    string GenerateRandomName()
    {
        int numberOfNames = Random.Range(1, 4);

        string name = string.Empty;

        for (int i = 0; i < numberOfNames; i++)
        {
            name += names[Random.Range(0, names.Length - 1)];
            if(i != numberOfNames - 1)
            {
                if (CalculateChance(1, 5))
                {
                    name += connectors[Random.Range(0, connectors.Length - 1)];
                }
            }
        }

        string firstLetter = name.Substring(0, 1).ToUpper();
        string remainingLetters = name.Substring(1).ToLower();

        string cavemanName = $"{firstLetter}{remainingLetters}";

        return cavemanName;
    }

    bool CalculateChance(int min, int max)
    {
        if(Random.Range(min, max) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
