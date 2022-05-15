using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : CombatActor
{
		/*public int DMG;
		public int HP;
		public int ARMOR;
		public int AP;*/
		public string characterName;
		public PartyMember() {
		//name = charName;
		stats[0] = 10;
		stats[1] = 150;
		stats[2] = 6;
		stats[3] = 5;
	}
	
	/*	public void attack() {
		GameObject attackTarget = base.target();
		
		if (attackTarget == null) {return;}
		
		Console.WriteLine("Isku tekee " + stats[0] + " vahinkoa kohteeseen "+attackTarget.name);
	}*/
	
	public int getARM() {
		return stats[2];
	}
	
	public void adjustARM(int adjust) {
		stats[2] += adjust;
	}
	

}
