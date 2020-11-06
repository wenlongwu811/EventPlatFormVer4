using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace EventPlatFormVer4.Service
{
    public class ParticipantService
    {
        //报名，将PartiState改成0
        public static void Apply()
        {
            MySqlConnection connection = new MySqlConnection();
            connection.Open();
            string applySql = "SELECT * FROM events WHERE State=1";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(applySql,connection);
            DataSet eventList = new DataSet();
            dataAdapter.Fill(eventList);
        }
    }
}
