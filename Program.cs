using System;
using System.Collections.Generic;
using System.IO;

namespace ChampionSelector
{    
    class Program
    {
        private const string RoleQuestion = "RoleQuestion";
        private const string LaneQuestion = "LaneQuestion";
        private const string DamageQuestion = "DamageQuestion";
        private const string NewnessQuestion = "NewnessQuestion";

        static void Main(string[] args)
        {
            //Construct API call and get champion list
            List<Champion> champData = APIMessage.MakeRequest();
            
            string questionDocument = File.ReadAllText(Directory.GetCurrentDirectory() + "/Data.yaml");

            //Construct question list
            Questions questionList = new Questions(questionDocument);

            //Ask questions
            foreach (QuestionObj q in questionList.questions)
            {
                Console.WriteLine(q.Question);
                foreach (AnswerObj answer in q.Answers)
                {
                    Console.WriteLine(answer.Text);
                }
                //Get Answer
                string input = (Console.ReadLine());
                string answerValue = q.Answers[Int32.Parse(input)].Value;
                //Filter list of available champions
                champData = ChampionUtil.FilterChampions(answerValue, q.Symbol, champData);
            }
            foreach (Champion c in champData)
            {
                Console.WriteLine(c.Name);
            }
        }
    }
}