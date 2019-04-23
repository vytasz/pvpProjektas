using System.Collections.Generic;
using System.Data;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    internal interface IDatabase
    {
        bool CreateTable(string table, List<string> columnNames);

        bool Create(string table, List<SQLColumnQuery> answerList);

        DataTable Read(string table, List<CrudArgs> args);

        bool Update(string table, List<SQLColumnQuery> answerList, List<CrudArgs> args);

        bool Delete(string table, List<CrudArgs> args);
    }
}