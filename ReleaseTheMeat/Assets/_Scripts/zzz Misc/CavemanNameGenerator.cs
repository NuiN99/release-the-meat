using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CavemanNameGenerator : MonoBehaviour
{
    string[] names = 
    {
        "Bar",
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
        "Thunk",
        "Trog",
        "Flug",
        "Snok",
        "Krug",
        "Sprog",
        "Knork",
        "Druk",
        "Skrunk",
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
        "Xog",
        "Yuk",
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
        "Hug",
        "Junk",
        "Lunk",
        "Musk",
        "Plog",
        "Skork",
        "Brusk",
        "Crug",
        "Drunk",
        "Flog",
        "Glug",
        "Husk",
        "Skog",
    };

    string[] connectors =
    {
        "tholomew",
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
        "el",
        "us",
    };

    public string caveManName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            caveManName = GenerateRandomName();
            print(caveManName);
        }
    }
    string GenerateRandomName()
    {
        int numberOfNames = Random.Range(1, 3);

        string name = string.Empty;

        for (int i = 0; i < numberOfNames; i++)
        {
            name += names[Random.Range(0, names.Length - 1)];

            if (numberOfNames == 1 || i != numberOfNames - 1)
            {
                if (CalculateChance(1, 3))
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
