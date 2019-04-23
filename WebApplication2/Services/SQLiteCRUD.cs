using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class SQLiteCRUD : IDatabase
    {
        private readonly string _conUrl = Settings.sqliteConnectionString;

        public SQLiteConnection GetConnection()
        {
            try
            {
                var cnn = new SQLiteConnection(_conUrl);
                return cnn;
                //cnn.Open();
            }
            catch (SQLiteException ex) { Console.WriteLine(ex.ToString()); }

            return null;
        }

        public bool CreateTable(string table, List<string> columnNames)
        {
            var conn = GetConnection();
            conn.Open();
            string sqlQuery = "CREATE TABLE " + table + "(";
            foreach (string obj in columnNames)
            {
                sqlQuery += columnNames + " STRING, ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2);
            sqlQuery += ")";
            SQLiteCommand insertSQL = new SQLiteCommand(sqlQuery, conn);
            try
            {
                insertSQL.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                conn.Close();
                return false;
            }
        }

        public bool Create(string table, List<SQLColumnQuery> answerList)
        {
            var conn = GetConnection();
            conn.Open();
            string sqlQuery = "INSERT INTO " + table + " (";
            foreach (SQLColumnQuery obj in answerList)
            {
                sqlQuery += obj.columnName + ", ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2);
            sqlQuery += ") VALUES (";
            foreach (SQLColumnQuery obj in answerList)
            {
                sqlQuery += "?, ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2);
            sqlQuery += ")";
            SQLiteCommand insertSQL = new SQLiteCommand(sqlQuery, conn);
            foreach (SQLColumnQuery obj in answerList)
            {
                insertSQL.Parameters.Add(new SQLiteParameter(obj.columnName) { Value = obj.answer });
            }
            try
            {
                insertSQL.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                conn.Close();
                throw e;
                //return false;
            }
        }

        public DataTable Read(string table, List<CrudArgs> args)
        {
            var conn = GetConnection();
            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn)
            {
                CommandText = "SELECT * FROM " + table + WhereStringBuilder(args) + ";"
            };
            SQLiteDataReader reader = cmd.ExecuteReader();
            DataTable tableSchema = reader.GetSchemaTable();
            DataTable dt = new DataTable();
            List<DataColumn> columnList = new List<DataColumn>();

            foreach (DataRow row in tableSchema.Rows)
            {
                string columnName = Convert.ToString(row["ColumnName"]);
                DataColumn column = new DataColumn(columnName, (Type)row["DataType"])
                {
                    Unique = (bool)row["IsUnique"],
                    AllowDBNull = (bool)row["AllowDBNull"],
                    AutoIncrement = (bool)row["IsAutoIncrement"]
                };
                columnList.Add(column);
                dt.Columns.Add(column);
            }
            while (reader.Read())
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < columnList.Count; i++)
                {
                    row[columnList[i]] = reader[i];
                }
                dt.Rows.Add(row);
            }
            reader.Close();
            conn.Close();
            return dt;
        }

        private string WhereStringBuilder(List<CrudArgs> args)
        {
            string whereString = " Where 1=1";
            if (args.Count > 0 || args != null)
            {
                for (int i = 0; i < args.Count; i++)
                {
                    whereString += " AND " + args[i].column + " " + args[i].argument + " " + "'" + args[i].value + "'";
                }
            }

            return whereString;
        }

        public bool Update(string table, List<SQLColumnQuery> answerList, List<CrudArgs> args)
        {
            var conn = GetConnection();
            conn.Open();
            string sqlQuery = $"UPDATE {table} SET ";
            foreach (SQLColumnQuery obj in answerList)
            {
                sqlQuery += $"{obj.columnName} = @{obj.columnName}, ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2);
            sqlQuery += WhereStringBuilder(args);
            SQLiteCommand insertSQL = new SQLiteCommand(sqlQuery, conn);
            foreach (SQLColumnQuery obj in answerList)
            {
                insertSQL.Parameters.AddWithValue($"@{obj.columnName}", obj.answer);
            }

            try
            {
                insertSQL.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                conn.Close();
                return false;
            }
        }

        public bool Delete(string table, List<CrudArgs> args)
        {
            var conn = GetConnection();
            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn)
            {
                CommandText = "DELETE FROM " + table + WhereStringBuilder(args) + ";"
            };
            cmd.ExecuteNonQuery();
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                conn.Close();
                return false;
            }
        }
    }
}