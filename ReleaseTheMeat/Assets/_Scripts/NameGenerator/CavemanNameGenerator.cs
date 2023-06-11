using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//Remove unused statements


//namesapce
public class CavemanNameGenerator : MonoBehaviour
{
    readonly string[] _firstNames = new CavemanNames().firstNames;
    readonly string[] _connectors = new CavemanNames().connectors;
    readonly string[] _describingNames = new CavemanNames().describingNames;

    public string _caveManName;

    [SerializeField] TextMeshProUGUI _nameText;

    void Start()
    {
        string twoName = $"{RandomFirstName()} {RandomLastName()}";
        string describingName = RandomDescribingName();

        _caveManName = GenerateRandomName(twoName, describingName);
        _nameText.text = _caveManName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            string twoName = $"{RandomFirstName()} {RandomLastName()}";
            string describingName = RandomDescribingName();

            _caveManName = GenerateRandomName(twoName, describingName);
            _nameText.text = _caveManName;
        }
    }

    string RandomFirstName()
    {
        // for 2 names: (1, 3)
        int numberOfNames = Random.Range(1, 1);

        string name = string.Empty;

        for (int i = 0; i < numberOfNames; i++)
        {
            name += _firstNames[Random.Range(0, _firstNames.Length - 1)];

            /*if (CalculateChance(1, 4))
            {
                name += _connectors[Random.Range(0, _connectors.Length - 1)];
            }*/
        }

        string firstLetter = name.Substring(0, 1).ToUpper();
        string remainingLetters = name.Substring(1).ToLower();

        name = $"{firstLetter}{remainingLetters}";

        return name;
    }

    string RandomLastName()
    {
        int numberOfNames = Random.Range(1, 3);

        string name = string.Empty;

        for (int i = 0; i < numberOfNames; i++)
        {
            name += _firstNames[Random.Range(0, _firstNames.Length - 1)];

            if (CalculateChance(1, 4))
            {
                name += _connectors[Random.Range(0, _connectors.Length - 1)];
            }
        }

        string firstLetter = name.Substring(0, 1).ToUpper();
        string remainingLetters = name.Substring(1).ToLower();

        name = $"{firstLetter}{remainingLetters}";

        return name;
    }

    string RandomDescribingName()
    {
        string name = string.Empty;

        name += RandomFirstName();
        name += " the ";
        name += _describingNames[Random.Range(0, _describingNames.Length - 1)];

        return name;
    }

    string GenerateRandomName(string twoNames, string describingName)
    {
        int nameType = Random.Range(1, 3);

        switch (nameType)
        {
            case 1:
                return twoNames;

            case 2:
                return describingName;

            default:
                return string.Empty;
        }
    }

    bool CalculateChance(int min, int max)
    {
        return Random.Range(min, max) == min;
    }
}
