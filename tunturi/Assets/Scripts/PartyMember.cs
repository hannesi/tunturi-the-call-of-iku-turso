using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PartyMember : CombatActor
{
		public int DMG;
		public int HP;
		public int ARMOR;
		public int AP;
		public PartyMember() {
		//name = charName;
		stats[0] = 10;
		stats[1] = 150;
		stats[2] = 13;
		stats[3] = 5;
	}
	
	/*	public void attack() {
		GameObject attackTarget = base.target();
		
		if (attackTarget == null) {return;}
		
		Console.WriteLine("Isku tekee " + stats[0] + " vahinkoa kohteeseen "+attackTarget.name);
	}*/
	
	

}
