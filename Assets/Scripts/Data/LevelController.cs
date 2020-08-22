using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/**
* @author jentevandersanden
* This is a data class that will handle the data containers for 
* the characters that belong to each level, and the containers 
* that belong to each character's room.
*/
public class LevelController
{
    private const int MAX_CONTAINERS = 10;                          // Constant that represents the max. amount of containers in a room.

    // PUZZLES + CONTAINERS
    private GameObject window_puzzle_prefab;
    private GameObject ink_puzzle_prefab;

    private GameObject horse_puzzle_prefab;                         
    private GameObject rack_puzzle_prefab;                         
    private GameObject alchemy_puzzle_prefab;                      
    private GameObject wardrobe_puzzle_prefab;
    private GameObject knight_puzzle_prefab; 
    private GameObject toilet_container_prefab;   
    private GameObject cardinal_container_prefab;

    private GameObject locker_puzzle_prefab;                      
    private GameObject corpse_container_prefab;    
    private GameObject chest_puzzle_prefab;
    private GameObject dna_analyze_prefab;
    private GameObject blood_puzzle_prefab;

    private GameObject bed_container_prefab;        
    private GameObject sink_puzzle_prefab;
    private GameObject hack_puzzle_prefab;


    private string[][] characters_per_level;                        // A nested list which contains the characters that correspond to each level's index, the level_id represents the index
    private Dictionary<string,GameObject[]> container_dictionary;   // A dictionary which will hold all the containers needed to initialize a character's room (an array of GameObjects for every character)
    private Dictionary <string, string[]> message_dictionary;       // A dictionary which will hold all the messages for the containers per level.
    private byte[] max_players_per_level;                           // List that holds the amount of players per level index

    // CONSTRUCTOR
    // Note: For now, we just hardcoded and inserted the leveldata as strings (using enums), we realize this doesn't really support expendability and the code is not beautiful,
    // but if we'd support custom levels (the extra webplatform on top of our application), we're planning to store and get this data from a database and server. 
    // The constructor will then just initialize the data by querying the database and receiving unity packages from the server.
    public LevelController(){

      initializeCharacters();

      // Load puzzle prefabs for every level

      // TUTORIAL
      window_puzzle_prefab = Resources.Load("Prefabs/Puzzles/Tutorial/WindowPuzzleContainer") as GameObject;
      ink_puzzle_prefab = Resources.Load("Prefabs/Puzzles/Tutorial/InkPuzzleContainer") as GameObject;


      // MURDER SOLVE
      locker_puzzle_prefab = Resources.Load("Prefabs/Puzzles/MurderSolve/LockerPuzzleContainer") as GameObject;
      corpse_container_prefab = Resources.Load("Prefabs/Puzzles/MurderSolve/CorpseContainer") as GameObject;
      chest_puzzle_prefab = Resources.Load("Prefabs/Puzzles/MurderSolve/ChestPuzzleContainer") as GameObject;
      dna_analyze_prefab = Resources.Load("Prefabs/Puzzles/MurderSolve/dnaAnalyzePuzzle") as GameObject;
      blood_puzzle_prefab = Resources.Load("Prefabs/Puzzles/MurderSolve/BloodPuzzle") as GameObject;
      
      // PRISON ESCAPE
      bed_container_prefab = Resources.Load("Prefabs/Puzzles/PrisonEscape/BedContainer") as GameObject;
      sink_puzzle_prefab = Resources.Load("Prefabs/Puzzles/PrisonEscape/SinkPuzzleContainer") as GameObject;
      hack_puzzle_prefab = Resources.Load("Prefabs/Puzzles/PrisonEscape/HackPuzzleContainer") as GameObject;


      // 3 MUSQUETEERS
      horse_puzzle_prefab = Resources.Load("Prefabs/Puzzles/3Musqueteers/PuzzlePony") as GameObject;
      rack_puzzle_prefab = Resources.Load("Prefabs/Puzzles/3Musqueteers/RackPuzzlePivot") as GameObject;
      wardrobe_puzzle_prefab = Resources.Load("Prefabs/Puzzles/3Musqueteers/WardrobePuzzleContainer") as GameObject;
      alchemy_puzzle_prefab = Resources.Load("Prefabs/Puzzles/3Musqueteers/AlchemyLabContainer") as GameObject;
      knight_puzzle_prefab = Resources.Load("Prefabs/Puzzles/3Musqueteers/KnightPuzzleContainer") as GameObject;
      toilet_container_prefab = Resources.Load("Prefabs/RoomDecoration/ToiletContainer") as GameObject;
      cardinal_container_prefab = Resources.Load("Prefabs/Puzzles/3Musqueteers/CardinalDeskContainer") as GameObject;
      

      initializeContainers();
      initializeMessages();
      initializePlayers();
    }

    // Returns the corresponding characters that belong to a level.
    // @param level_id: The level of which you want the characters
    // @return List<Character> : the list of characters that belong to the given level
    public string[] getCharacters(int level_id){

        return characters_per_level[level_id - 1];
    }

    // Returns the corresponding containers that belong to a certain character's room.
    // @param character: The character of which you want the containers
    // @return GameObject[] : Returns an array of GameObjects that belong to this character's room
    public GameObject[] getContainers(string character){

        return container_dictionary[character];
    }

    public string[] getMessages(string character){
        return message_dictionary[character];
    }

    // Returns the corresponding max amount of players for a certain level.
    // @param level_id: The level of which you want to get the max amount of players.
    // @return int: Returns the max amount of players for this level.
    public byte getPlayersPerLevel(int level_id){

        return max_players_per_level.ElementAt(level_id - 1);
    }

    // Initializes the list of characters per level.
    private void initializeCharacters(){

        // Create a new empty nested list of characters
        characters_per_level = new string[5][];

        // We'll use this list to insert the data we need
        string[] insertlist = new string[1];
        string[] insertlist2 = new string[2];
        string[] insertlist3 = new string[2];
        string[] insertlist4 = new string[3];

        // Level 1
        insertlist[0] = Characters.TutorialPlayer.ToString();
        characters_per_level[0] = insertlist;


        //Level 2
        insertlist2[0] = Characters.LabScientist.ToString();
        insertlist2[1] = Characters.ForensicScientist.ToString();
        characters_per_level[1] = insertlist2;
  
        //Level 3
        insertlist3[0] = Characters.Prisoner.ToString();
        insertlist3[1] = Characters.Hacker.ToString();
        characters_per_level[2] = insertlist3;

        //Level 4
        insertlist4[0] = Characters.Porthos.ToString();
        insertlist4[1] = Characters.Athos.ToString();
        insertlist4[2] = Characters.Aramis.ToString();
        characters_per_level[3] = insertlist4;
    }

    // Initializes the list of containers per character's room.
    private void initializeContainers(){

        // Create a new dictionary 
        container_dictionary = new Dictionary<string, GameObject[]>();
        
        // We set the max of Containers per room to 10 (Again this is hardcoded in an ugly way right now, we could later set and store this on the server if we'd implement this,
        // so we can also verify the amount of containers per room every time someone uploads a new custom level.)
        GameObject[] containers_per_char_tutorial = new GameObject[MAX_CONTAINERS]; 

        GameObject[] containers_per_char_labscient = new GameObject[MAX_CONTAINERS];     
        GameObject[] containers_per_char_forscient = new GameObject[MAX_CONTAINERS]; 

        GameObject[] containers_per_char_prisoner = new GameObject[MAX_CONTAINERS];  
        GameObject[] containers_per_char_hacker = new GameObject[MAX_CONTAINERS];     

        GameObject[] containers_per_char_athos = new GameObject[MAX_CONTAINERS];   
        GameObject[] containers_per_char_aramis = new GameObject[MAX_CONTAINERS];  
        GameObject[] containers_per_char_porthos = new GameObject[MAX_CONTAINERS];  

        // Tutorial Player
        containers_per_char_tutorial[0] = window_puzzle_prefab;
        containers_per_char_tutorial[1] = ink_puzzle_prefab;
        container_dictionary.Add(Characters.TutorialPlayer.ToString(),containers_per_char_tutorial);

        // Lab Scientist
        // containers_per_char_labscient[0] = locker_puzzle_prefab;
        // containers_per_char_labscient[1] = dna_analyze_prefab;
        containers_per_char_labscient[0] = alchemy_puzzle_prefab;
        containers_per_char_labscient[1] = horse_puzzle_prefab;
        containers_per_char_labscient[2] = toilet_container_prefab;
        container_dictionary.Add(Characters.LabScientist.ToString(), containers_per_char_labscient);

        // Forensic Scientist
        // containers_per_char_forscient[0] = corpse_container_prefab;
        // containers_per_char_forscient[1] = chest_puzzle_prefab;
        // containers_per_char_forscient[2] = blood_puzzle_prefab;
        containers_per_char_forscient[0] = wardrobe_puzzle_prefab;
        containers_per_char_forscient[1] = knight_puzzle_prefab;
        containers_per_char_forscient[2] = cardinal_container_prefab;
        containers_per_char_forscient[3] = rack_puzzle_prefab;

        container_dictionary.Add(Characters.ForensicScientist.ToString(), containers_per_char_forscient);

        // Prisoner
        // containers_per_char_prisoner[0] = bed_container_prefab;
        // containers_per_char_prisoner[1] = sink_puzzle_prefab;
        container_dictionary.Add(Characters.Prisoner.ToString(), containers_per_char_prisoner);

        // Hacker
        containers_per_char_hacker[0] = hack_puzzle_prefab;
        container_dictionary.Add(Characters.Hacker.ToString(), containers_per_char_hacker);

        // Porthos
        containers_per_char_porthos[0] = rack_puzzle_prefab;
        container_dictionary.Add(Characters.Porthos.ToString(), containers_per_char_porthos);

        // Athos
        containers_per_char_athos[0] = wardrobe_puzzle_prefab;
        containers_per_char_athos[1] = knight_puzzle_prefab;
        containers_per_char_athos[2] = cardinal_container_prefab;
        container_dictionary.Add(Characters.Athos.ToString(), containers_per_char_athos);

        // Aramis
        containers_per_char_aramis[0] = alchemy_puzzle_prefab;
        containers_per_char_aramis[1] = horse_puzzle_prefab;
        containers_per_char_aramis[2] = toilet_container_prefab;
        container_dictionary.Add(Characters.Aramis.ToString(), containers_per_char_aramis);
    }

    private void initializeMessages(){
        
        message_dictionary = new Dictionary<string, string[]>();
        
        string[] messages_per_char_tutorial = new string[MAX_CONTAINERS];

        string[] messages_per_char_labscient = new string[MAX_CONTAINERS];
        string[] messages_per_char_forscient = new string[MAX_CONTAINERS];

        string[] messages_per_char_prisoner = new string[MAX_CONTAINERS];
        string[] messages_per_char_hacker = new string[MAX_CONTAINERS];

        string[] messages_per_char_porthos = new string[MAX_CONTAINERS];
        string[] messages_per_char_athos = new string[MAX_CONTAINERS];
        string[] messages_per_char_aramis = new string[MAX_CONTAINERS];

        // Tutorial player
        messages_per_char_tutorial[0] = "Place the indicator on a wall (at least 1m wide), with the arrow pointing towards the floor.";
        messages_per_char_tutorial[1] = "Place the indicator on the floor, with the arrow pointing away from you.";
        message_dictionary.Add(Characters.TutorialPlayer.ToString(), messages_per_char_tutorial);
        
        // Lab Scientist
        messages_per_char_labscient[0] = "Place the indicator on the floor, close to a wall, with the arrow pointing towards this wall.";
        messages_per_char_labscient[1] = "Place the indicator on the floor, close to a wall, with the arrow pointing towards this wall.";
        message_dictionary.Add(Characters.LabScientist.ToString(), messages_per_char_labscient);

        // Forensic Scientist
        messages_per_char_forscient[0] = "Place the indicator on the floor, with the arrow pointing away from you.";
        messages_per_char_forscient[1] = "Place the indicator on the floor, with the arrow pointing away from you.";
        messages_per_char_forscient[2] = "Place the indicator on a wall (at least 1m wide), with the arrow pointing towards the floor.";
        message_dictionary.Add(Characters.ForensicScientist.ToString(), messages_per_char_forscient);

        // Prisoner
        messages_per_char_prisoner[0] =  "Place the indicator on the floor, with the arrow pointing away from you.";
        messages_per_char_prisoner[1] =  "Place the indicator on a wall (at least 1m wide), with the arrow pointing towards the floor.";
        message_dictionary.Add(Characters.Prisoner.ToString(), messages_per_char_prisoner);

        // Hacker
        messages_per_char_hacker[0] =  "Place the indicator on the floor, with the arrow pointing away from you.";
        message_dictionary.Add(Characters.Hacker.ToString(), messages_per_char_hacker);

        // Porthos
        messages_per_char_porthos[0] = "Place the indicator on the floor, with the arrow pointing away from you.";
        message_dictionary.Add(Characters.Porthos.ToString(), messages_per_char_porthos);

        // Athos
        messages_per_char_athos[0] = "Place the indicator on the floor, close to a wall, with the arrow pointing towards this wall.";
        messages_per_char_athos[1] = "Place the indicator on the floor, close to a wall, with the arrow pointing towards this wall.";
        messages_per_char_athos[2] = "Place the indicator on the floor, with the arrow pointing away from you.";
        message_dictionary.Add(Characters.Athos.ToString(), messages_per_char_athos);

        // Aramis
        messages_per_char_aramis[0] = "Place the indicator on the floor, with the arrow pointing away from you.";
        messages_per_char_aramis[1] = "Place the indicator on the floor, with the arrow pointing away from you.";
        messages_per_char_aramis[2] = "Place the indicator on the floor, close to a wall, with the arrow pointing towards this wall.";
        message_dictionary.Add(Characters.Aramis.ToString(), messages_per_char_aramis);
    }

    // Initializes the list of max players per level.
    private void initializePlayers(){
        max_players_per_level = new byte[10];
        // Level 1
        max_players_per_level[0] = (byte)1;
        // Level 2
        max_players_per_level[1] = (byte)2;
        // Level 3
        max_players_per_level[2] = (byte)2;
        // Level 4
        max_players_per_level[3] = (byte)3;
        // Level 5
        max_players_per_level[4] = (byte)3;
    }
}
