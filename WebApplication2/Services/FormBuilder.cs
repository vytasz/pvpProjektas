using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class FormBuilder
    {
        public Form BuildForm(string inputText)
        {
            List<Question> questionList = new List<Question>();
            string text;
            int position, start = 0;
            string name = "default";
            bool doesTableNameExist = false;
            do
            {
                if (!doesTableNameExist)
                {
                    position = inputText.IndexOf('@', start);
                    text = inputText.Substring(start, position).Trim();
                    name = text;
                    doesTableNameExist = true;
                    start= position+2;
                }
                position = inputText.IndexOf('<', start);
                if (position >= 0)
                {
                    text = inputText.Substring(start - 1, position - start).Trim();
                    questionList.Add(FormatQuestion(text, questionList.Count()));
                    start = position + 1;
                }
            } while (position > 0);
            text = inputText.Substring(start - 1).Trim();
            questionList.Add(FormatQuestion(text, questionList.Count()));
            questionList.RemoveAt(0);
            Form result = new Form
            {
                tablename = name,
                questions = questionList
            };
            return result;
        }

        private Question FormatQuestion(string question, int Id)
        {
            Id--;
            StringReader strReader = new StringReader(question);
            Question output = new Question
            {
                qAnswer = "",
                id = Id
            };
            string line;
            while (true)
            {
                line = strReader.ReadLine();
                if (line != null)
                {
                    line = RemoveComments(line);
                    if (line.Contains("<"))
                    {
                        int start = 1;
                        int end = line.IndexOf(';', 0);
                        string check = line.Substring(start, end - start).ToLower();
                        switch (check)
                        {
                            case "text":
                                output.qType = Question.QuestionType.Text;
                                break;
                            case "shorttext":
                                output.qType = Question.QuestionType.ShortText;
                                break;
                        }

                        start = line.IndexOf(";", 0);
                        end = line.IndexOf('>', 0);
                        output.fieldname = line.Substring(start + 1, end - start - 1);
                        output.question = line.Substring(end + 1);
                    }
                    else
                    {
                        if (line != "") output.qAnswer += line;
                    }
                }
                else
                {
                    break;
                }
            }
            if (output.qType == Question.QuestionType.Multiple)
            {
                output.multanswers = new List<bool>();
                foreach (string answer in output.answers)
                {
                    output.multanswers.Add(false);
                }
            }
            return output;
        }

        private string RemoveComments(string line)
        {
            if (line.Contains("/*"))
            {
                int start = line.IndexOf("/*");
                int end = line.IndexOf("*/");
                line = line.Remove(start, end - start);
            }
            return line;
        }
    }
}