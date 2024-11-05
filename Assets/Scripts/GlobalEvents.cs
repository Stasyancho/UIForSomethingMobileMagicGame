using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{ 
    static UnityEvent<Vector2> MovedJoystickMove = new UnityEvent<Vector2>();
    static UnityEvent<Vector2> CastedJoystickMove = new UnityEvent<Vector2>();
    static UnityEvent<int> PickSpell = new UnityEvent<int>();
    static UnityEvent<int, Vector2> CastSpell = new UnityEvent<int, Vector2>();

    #region Invokes
    public static void MovedJoystickMoveInvoke(Vector2 vector2Value)
    {
        MovedJoystickMove.Invoke(vector2Value);
    }
    public static void CastedJoystickMoveInvoke(Vector2 vector2Value)
    {
        CastedJoystickMove.Invoke(vector2Value);
    }
    public static void PickSpellInvoke(int intValue)
    {
        PickSpell.Invoke(intValue);
    }
    public static void CastSpellInvoke(int intValue, Vector2 vector2Value)
    {
        CastSpell.Invoke(intValue, vector2Value);
    }
    #endregion

    #region Additions
    public static void MovedJoystickMoveAdd(UnityAction<Vector2> metod)
    {
        MovedJoystickMove.AddListener(metod);
    }
    public static void CastedJoystickMoveAdd(UnityAction<Vector2> metod)
    {
        CastedJoystickMove.AddListener(metod);
    }
    public static void PickSpellAdd(UnityAction<int> metod)
    {
        PickSpell.AddListener(metod);
    }
    public static void CastSpellAdd(UnityAction<int, Vector2> metod)
    {
        CastSpell.AddListener(metod);
    }
    #endregion
}
