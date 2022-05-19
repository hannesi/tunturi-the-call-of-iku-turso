using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Item
{
	private string name;
	private int statsAffected;
	private int[] statRefIDs; //References to attributes increased
	//private PropertyType[] statRefs;
	private int[] statBonuses;
	private bool isEquipped;
	
	public Item() {
		name = "default";
		statsAffected = 1;
		statRefIDs = new int[statsAffected];
		statBonuses = new int[statsAffected];
		isEquipped = false;
	}
	
	public Item(string itemname, int affected) {
		name = itemname;
		statsAffected = affected;
		statRefIDs = new int[statsAffected];
		statBonuses = new int[statsAffected];
		isEquipped = false;
	}
	
	public Item(string itemname, int affected, int[] readIDs, int[] readBonuses) {
		name = itemname;
		statsAffected = affected;
		statRefIDs = readIDs;
		statBonuses = readBonuses;
		isEquipped = false;
	}
	
	public string announce() {
		return "I am "+this.name+" and I have "+this.statRefIDs.Length+" refs and "+this.statBonuses.Length+" bonuses within";
	}
	
	// method for constructing items from XML data
	public static Item construct(int id) {
		//Create list for storing varying amounts of refs/bonuses
		//Convert to arrays after read, construct new item from data
		XmlReader itemReader = XmlReader.Create("datatest.xml");
		
		itemReader.MoveToContent();
		while(itemReader.Read()) {
			if (itemReader.NodeType == XmlNodeType.Text) {
				Debug.Log(itemReader.Value);
			} else {
				Debug.Log("closing reader");
				break;
			}
		}
		
		itemReader.Close();
		Item fetchedItem = new Item();
		return fetchedItem;
	}
	
	public static void readTest() {
		//Create lists for storing varying amounts of refs/bonuses
		//Convert to arrays after read, construct new item from data
		List<int> statrefs = new List<int>();
		List<int> bonuses = new List<int>();
		string readName = "";
		
		XmlReader itemReader = XmlReader.Create("datatest.xml");
		itemReader.MoveToContent();
		while(itemReader.Read()) {
			/*if (itemReader.NodeType == XmlNodeType.Text) {
				//Debug.Log(itemReader.Name);
				Debug.Log(itemReader.ReadString());
			}*/

			while (itemReader.MoveToNextAttribute()) {
				if (itemReader.ReadContentAsInt() == 2) {
					Debug.Log("found 2");
					while(itemReader.Read()) {
						switch(itemReader.Name) {
							case "name":
								readName = itemReader.ReadElementString();
								continue;
							case "statref":
								statrefs.Add(itemReader.ReadElementContentAsInt());
								continue;
							case "bonus":
								bonuses.Add(itemReader.ReadElementContentAsInt());
								continue;
							case "item":
								break;
						}
					}
				}
			}
				//Debug.Log("closing reader");
				//break;
			//}
		}
		
		itemReader.Close();
		Debug.Log(bonuses.Count + " in listed bonuses");
		Item newestItem = new Item(readName, 2, statrefs.ToArray(), bonuses.ToArray());
		Debug.Log(newestItem.announce());
		//Item fetchedItem = new Item();
		//return fetchedItem;
		return;
	}
	
	//PropertyType
}
