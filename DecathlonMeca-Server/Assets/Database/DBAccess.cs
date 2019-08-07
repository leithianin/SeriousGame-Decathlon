﻿using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class DBAccess : MonoBehaviour
{
    public void Start()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/RankingDatabase";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand dbcmd;
        IDataReader reader;

        //Create Table
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS 'RankingTab'( " +
                            " 'rank' INTEGER NOT NULL UNIQUE, " +
                            " 'name' TEXT NOT NULL, " +
                            " 'score' INTEGER NOT NULL" +
                            ");";

        reader = dbcmd.ExecuteReader();

        //Insert dans la table
        /*IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (1, 'moi1', 50)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (2, 'moi2', 45)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (3, 'moi3', 40)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (4, 'moi4', 30)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (5, 'moi5', 25)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (6, 'moi6', 20)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (7, 'moi7', 15)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (8, 'moi8', 10)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (9, 'moi9', 5)";
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "INSERT INTO 'RankingTab' (rank, name, score) VALUES (10, 'moi10', 1)";
        cmnd.ExecuteNonQuery();*/

        //Delete All
        /*IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 1;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 2;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 3;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 4;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 5;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 6;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 7;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 8;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 9;
        cmnd.ExecuteNonQuery();
        cmnd.CommandText = "DELETE FROM 'RankingTab' where rank = " + 10;
        cmnd.ExecuteNonQuery();*/

        //Lire toute la table
        IDbCommand cmnd_read = dbcon.CreateCommand();

        cmnd_read.CommandText = "SELECT * FROM 'RankingTab' ";
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log(reader[0].ToString() + " / " + reader[1] + " / " + reader[2].ToString());
        }

        dbcon.Close();
    }

    #region GetHallOfFame
    /**************************************************
     *      Permet d'avoir le nom des joueurs         *
     *************************************************/
    public string getHallOfFameName(int rank)
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/RankingDatabase";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();

        cmnd_read.CommandText = "SELECT name FROM 'RankingTab' where rank =" + rank.ToString();
        IDataReader reader = cmnd_read.ExecuteReader();

        return reader[0].ToString();

    }

    /**************************************************
    *      Permet d'avoir le score des joueurs        *
    ***************************************************/
    public int getHallOfFameScore(int rank)
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/RankingDatabase";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();

        cmnd_read.CommandText = "SELECT score FROM 'RankingTab' where rank =" + rank.ToString();
        IDataReader reader = cmnd_read.ExecuteReader();

        return int.Parse(reader[0].ToString());
    }
    #endregion
}
