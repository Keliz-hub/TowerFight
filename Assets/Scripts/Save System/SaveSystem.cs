using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace TowerFight
{
    public class SaveSystem
    {
        public const string SaveName = "DataPlayer.bin";
        public const string PlayerSquad = "Player_Squad";
        public const string PlayerUnits = "Player_Units";
        public const string PlayerTowers = "Player_Towers";
        public const string PlayerActiveTower = "Player_Active_Tower";
        public const string Gold = "Gold";

        private static Dictionary<string, object> saveFile;

        public static Dictionary<string, object> Convert(DataPlayer dataPlayer)
        {
            var data = new Dictionary<string, object>();

            var squad = new List<int>();

            foreach (var item in dataPlayer.squad)
            {
                squad.Add(dataPlayer.playerUnits.IndexOf(item));
            }

            data.Add(PlayerSquad, squad.ToArray());

            var playerUnits = new List<Stats>();

            foreach (var item in dataPlayer.playerUnits)
            {
                playerUnits.Add(item.stats);
            }

            data.Add(PlayerUnits, playerUnits.ToArray());


            var playerTowers = new List<Stats>();

            foreach (var item in dataPlayer.playerTowers)
            {
                playerTowers.Add(item.stats);
            }

            data.Add(PlayerTowers, playerTowers.ToArray());


            data.Add(PlayerActiveTower, dataPlayer.playerTowers.IndexOf(dataPlayer.dataTower));

            data.Add(Gold, dataPlayer.Gold.Value);

            return data;
        }

        public static DataPlayer Convert(Dictionary<string, object> data, DataUnits dataUnits , DataPlayer reference)
        {
            DataPlayer dataPlayer = DataPlayer.Copy(reference);

            dataPlayer.Gold.SetValueAndForceNotify((int)data[Gold]);


            var unitStats = data[PlayerUnits] as Stats[];
            
            var playerUnits = new List<DataUnit>();
            int i = 0;

            foreach (var item in dataUnits.units)
            {
                    
                var unit = DataUnit.Copy(item);
                if (i < unitStats.Length)
                    unit.stats = unitStats[i];
                playerUnits.Add(unit);
                i++;
            }
            dataPlayer.playerUnits = playerUnits;



            var playerSquadNumbers = data[PlayerSquad] as int[];
            var playerSquad = new List<DataUnit>();
            foreach (var item in playerSquadNumbers)
            {
                playerSquad.Add(playerUnits[item]);
            }
            dataPlayer.squad = playerSquad;


            var towerStats = data[PlayerTowers] as Stats[];
            var playerTowers = new List<DataUnit>();
            i = 0;
            foreach (var item in dataUnits.towers)
            {
              
                var tower = DataUnit.Copy(item);

                if (i < towerStats.Length)                   
                    tower.stats = towerStats[i];
                playerTowers.Add(tower);
                i++;
            }
            dataPlayer.playerTowers = playerTowers;

            var activeTowerNumber = (int)data[PlayerActiveTower];
            if (activeTowerNumber >= 0 && playerTowers.Count > activeTowerNumber)
                dataPlayer.dataTower = playerTowers[activeTowerNumber];
            else
                dataPlayer.dataTower = playerTowers[0];

            return dataPlayer;

        }


        public static void Save(DataPlayer dataPlayer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = $"{Application.persistentDataPath}/{SaveName}";
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, Convert(dataPlayer));
            stream.Close();
        }
        public static DataPlayer Load(DataUnits dataUnits,DataPlayer reference)
        {
            string path = $"{Application.persistentDataPath}/{SaveName}";
            if (File.Exists(path))
            {
                Debug.Log($"Load successfully {path}");
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                saveFile = formatter.Deserialize(stream) as Dictionary<string, object>;
                DataPlayer data = Convert(saveFile, dataUnits, reference);

                return data;
            }
            Debug.Log($"Save file not found in {path}");
            return null;

        }

    }
}