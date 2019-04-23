using System.Collections.Generic;
using System.Data;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    internal class SQLCRUD : IDatabase
    {
        public bool Create(string table, List<SQLColumnQuery> answerList)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateTable(string table, List<string> columnNames)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(string table, List<CrudArgs> args)
        {
            throw new System.NotImplementedException();
        }

        public DataTable Read(string table, List<CrudArgs> args)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(string table, List<SQLColumnQuery> answerList, List<CrudArgs> args)
        {
            throw new System.NotImplementedException();
        }
    }
}