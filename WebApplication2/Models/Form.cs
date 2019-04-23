using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class Form
    {
        public List<Question> questions = new List<Question>();
        public string tablename;
        public int userId;
        public int formId;
    }
}