using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class Higher
    {
        public bool WriteForm(int userId, Form form)
        {
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }
            form.userId = userId;

            List<SQLColumnQuery> answerList = new List<SQLColumnQuery>
            {
                new SQLColumnQuery
                {
                    columnName = "FormName",
                    answer = form.tablename
                },
                new SQLColumnQuery
                {
                    columnName = "FormJSON",
                    answer = JsonConvert.SerializeObject(form)
                },
                new SQLColumnQuery
                {
                    columnName = "UserId",
                    answer = userId.ToString()
                },
            };
            return _crud.Create("Forms", answerList);
        }

        public bool UpdateForm(int formId, Form form)
        {
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }

            List<SQLColumnQuery> answerList = new List<SQLColumnQuery>
            {
                new SQLColumnQuery
                {
                    columnName = "FormName",
                    answer = form.tablename
                },
                new SQLColumnQuery
                {
                    columnName = "FormJSON",
                    answer = JsonConvert.SerializeObject(form)
                }
            };
            return _crud.Update("Forms", answerList, new List<CrudArgs> { new CrudArgs { column = "FormId", argument = "=", value = formId.ToString() } });
        }

        internal User ReadUser(string username)
        {
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }
            List<CrudArgs> args = new List<CrudArgs>
            {
                new CrudArgs()
                {
                    column ="username",
                    argument = "=",
                    value = username
                }
            };
            DataRow[] userRow = _crud.Read("Users", args).Select();
            if (userRow.Length == 0) return null;
            User user = new User
            {
                username = userRow[0]["UserName"].ToString(),
                password = userRow[0]["UserPassword"].ToString(),
                id = int.Parse(userRow[0]["UserId"].ToString())
            };
            return user;
        }

        public Form ReadForm(int formId)
        {
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }
            List<CrudArgs> args = new List<CrudArgs>
            {
                new CrudArgs()
                {
                    column = "FormId",
                    argument = "=",
                    value = formId.ToString()
                }
            };
            DataRow[] formRows = _crud.Read("Forms", args).Select();
            Form form = JsonConvert.DeserializeObject<Form>(formRows[0]["FormJSON"].ToString());
            form.formId = int.Parse(formRows[0]["FormId"].ToString());
            return form;
        }

        internal void DeleteFormFormatted(Form form)
        {
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }
            var args = new List<CrudArgs>
                {
                    new CrudArgs
                    {
                        column = "formId",
                        argument = "=",
                        value = form.formId.ToString()
                    }
                };
            _crud.Delete("FormsFormatted", args);
        }

        public Dictionary<string, int> ReadUserFormList(int userId)
        {
            var result = new Dictionary<string, int>();
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }
            List<CrudArgs> args = new List<CrudArgs>
            {
                new CrudArgs()
                {
                    column = "UserId",
                    argument = "=",
                    value = userId.ToString()
                }
            };
            DataRow[] formRows = _crud.Read("Forms", args).Select();
            foreach(DataRow row in formRows)
            {
                result.Add(row["FormName"].ToString(), int.Parse(row["FormId"].ToString()));
            }
            return result;
        }

        public void WriteFormFormatted(Form form)
        {
            IDatabase _crud;
            if (Settings.testing)
            {
                _crud = new SQLiteCRUD();
            }
            else
            {
                _crud = new SQLCRUD();
            }
            foreach (Question q in form.questions)
            {
                var answerList = new List<SQLColumnQuery>
                {
                    new SQLColumnQuery()
                    {
                        columnName = "formId",
                        answer = form.formId.ToString()
                    },
                    new SQLColumnQuery()
                    {
                        columnName = "questionName",
                        answer = q.question
                    },
                    new SQLColumnQuery()
                    {
                        columnName = "value",
                        answer = q.qAnswer
                    }
                };
                _crud.Create("FormsFormatted", answerList);
            }
        }
    }
}