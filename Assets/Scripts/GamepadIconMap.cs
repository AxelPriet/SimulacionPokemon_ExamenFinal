using System.Collections.Generic;
using UnityEngine;

public enum GamepadType
{
    PC,
    Generic,
    Xbox,
    PlayStation,
    Nintendo
}

[CreateAssetMenu(fileName = "GamepadIconMap", menuName = "Input/Icon Map")]
public class GamepadIconMap : ScriptableObject
{
    public GamepadType gamepadType;

    [System.Serializable]
    public class IconEntry
    {
        public string displayName;
        public string controlPath;
        public Sprite sprite;
    }

    public List<IconEntry> entries = new();

    public Sprite GetIcon(string path)
    {
        IconEntry entry = entries.Find(e => e.controlPath.ToLower() == path.ToLower());
        return entry != null ? entry.sprite : null;
    }

    public string GetName(string path)
    {
        IconEntry entry = entries.Find(e => e.controlPath.ToLower() == path.ToLower());
        return entry != null ? entry.displayName : path;
    }
}
