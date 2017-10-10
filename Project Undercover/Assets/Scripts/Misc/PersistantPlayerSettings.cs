using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantPlayerSettings {

	public static string testString = "";
    public enum Character {
        Guard, Spy
    }
    public static Character character = PersistantPlayerSettings.Character.Spy;
	
}
