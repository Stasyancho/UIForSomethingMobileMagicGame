using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DirectionsController : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _circleDirection;
    [SerializeField] GameObject _rectangleDirection;
    List<Spell> spells = new List<Spell>();

    void Start()
    {
        GlobalEvents.CastedJoystickMoveAdd(CastDirection);
        GlobalEvents.PickSpellAdd(SelectedDirection);
        GlobalEvents.CastSpellAdd(HideDirections);
        string[] assetNames = AssetDatabase.FindAssets("t:Spell", new[] { "Assets/Spells" });
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<Spell>(SOpath);
            spells.Add(character);
        }
    }

    void CastDirection(Vector2 data)
    {
        float y = -1;
        if (data.magnitude != 0)
        {
            y = 0.01f;
        }

        if (_circleDirection.activeSelf)
        {
            _circleDirection.transform.position = new Vector3(data.x * 10 + _player.transform.position.x, y, data.y * 10 + _player.transform.position.z);
        }

        if (_rectangleDirection.activeSelf)
        {
            _rectangleDirection.transform.position = new Vector3(data.x * 5 + _player.transform.position.x, y, data.y * 5 + _player.transform.position.z);
            _rectangleDirection.transform.eulerAngles = new Vector3(90, 0, (Vector3.SignedAngle(Vector3.up, new Vector3(data.x, data.y), Vector3.forward) + 360f));
            _rectangleDirection.transform.localScale = new Vector3(1, 10 * data.magnitude, 1);
        }
    }

    void SelectedDirection(int key)
    {
        Spell choosedSpell = spells.FirstOrDefault(s => s.Key == key);
        if (choosedSpell.SpellType == SpellType.Solo)
        {
            _rectangleDirection.SetActive(true);
        }
        if (choosedSpell.SpellType == SpellType.Mass)
        {
            _circleDirection.SetActive(true);
        }
        Debug.Log("PickedSpell:" + choosedSpell.name);
    }

    void HideDirections(int key, Vector2 vector2)
    {
        _circleDirection.SetActive(false);
        _rectangleDirection.SetActive(false);
    }
}
