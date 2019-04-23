using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class Question
    {
        public enum QuestionType { Question, DropDown, YesNo, Text, ShortText, Multiple };
        public QuestionType qType;
        public string fieldname, question, qAnswer;

        public int id;
        public List<string> answers;
        public List<bool> multanswers;//reikalingas jei klausimas gali tureti keleta pasirinkimu
    }
}