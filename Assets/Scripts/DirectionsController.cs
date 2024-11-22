using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DirectionsController : MonoBehaviour
{
    //shows direction for casted spells
    //you can add actions/effects "after cast spell" to this class
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _circleDirection;
    [SerializeField] GameObject _rectangleDirection;
    List<Spell> _spells = new List<Spell>();

    //"nothing" or "solo" or "area"
    DiectionType _selecteDirectionType = DiectionType.None;

    //follow to events, get all spells from project
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
            _spells.Add(character);
        }
    }

    void CastDirection(Vector2 data)
    {
        if (data.magnitude != 0)
        {
            if (_selecteDirectionType == DiectionType.Rectangle && !_rectangleDirection.activeSelf)
                _rectangleDirection.SetActive(true);
            if (_selecteDirectionType == DiectionType.Circle && !_circleDirection.activeSelf)
                _circleDirection.SetActive(true);
        }
        else
        {
            if (_selecteDirectionType == DiectionType.Rectangle && _circleDirection.activeSelf)
                _rectangleDirection.SetActive(false);
            if (_selecteDirectionType == DiectionType.Circle && _circleDirection.activeSelf)
                _circleDirection.SetActive(false);
        }

        if (_circleDirection.activeSelf)
        {
            _circleDirection.transform.position = new Vector3(data.x * 10 + _player.transform.position.x, 0.01f, data.y * 10 + _player.transform.position.z);
        }

        if (_rectangleDirection.activeSelf)
        {
            _rectangleDirection.transform.position = new Vector3(data.x * 5 + _player.transform.position.x, 0.01f, data.y * 5 + _player.transform.position.z);
            _rectangleDirection.transform.eulerAngles = new Vector3(90, 0, (Vector3.SignedAngle(Vector3.up, new Vector3(data.x, data.y), Vector3.forward) + 360f));
            _rectangleDirection.transform.localScale = new Vector3(1, 10 * data.magnitude, 1);
        }
    }

    void SelectedDirection(int key)
    {
        Spell choosedSpell = _spells.FirstOrDefault(s => s.Key == key);
        if (choosedSpell.SpellType == SpellType.Solo)
        {
            _selecteDirectionType = DiectionType.Rectangle;
        }
        if (choosedSpell.SpellType == SpellType.Mass)
        {
            _selecteDirectionType = DiectionType.Circle;
        }
        Debug.Log("PickedSpell:" + choosedSpell.name);
    }

    void HideDirections(int key, Vector2 vector2)
    {
        _selecteDirectionType = DiectionType.None;
        _circleDirection.SetActive(false);
        _rectangleDirection.SetActive(false);
    }

    enum DiectionType
    {
        None,
        Circle,
        Rectangle
    }
}
