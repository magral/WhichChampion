using System;
using System.Collections.Generic;
using System.IO;
using GLib;
using YamlDotNet.Serialization;

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

            //Construct question list
            Questions questionList = new Questions(Document);

            //Holds answers to questions
            Answer ans = new Answer();

            //Ask questions
            foreach (QuestionObj q in questionList.questions)
            {
                Console.WriteLine(q.Question);
                foreach (AnswerObj answer in q.Answers)
                {
                    Console.WriteLine(answer.Text);
                }
                string input = (Console.ReadLine());
                champData = ChampionUtil.FilterChampions(input, q.Symbol, champData);
            }
            foreach (Champion c in champData)
            {
                Console.WriteLine(c.Name);
            }
        }

        private const string Document = @"---
        questions:
            - symbol: LaneQuestion
              question: What lane do you feel like playing?
              answers:
                - answer:
                    value: top
                    text: Top Lane
                - answer:
                    value: jungle
                    text: Jungle
                - answer:
                    value: mid
                    text: Mid Lane
                - answer:
                    value: bottom
                    text: Bottom
                - answer:
                    value: support
                    text: Support
            - symbol: RoleQuestion
              question: What style do you want to play?
              answers:
                - answer:
                    value: ranged
                    text: Ranged
                - answer:
                    value: bruiser
                    text: Bruiser
                - answer:
                    value: tank
                    text: Tank
            - symbol: DamageQuestion
              question: Preference on damage type?
              answers:
                - answer:
                    value: AP
                    text: AP
                - answer:
                    value: AD
                    text: AD
                - answer:
                    value: hybrid
                    text: Hybrid
                - answer:
                    value: NoPref
                    text: Don't Care
            - symbol: NewnessQuestion
              question: Do you want to try something new, or play something you know?
              answers:
                - answer:
                    value: Yes
                    text: Try something new!
                - answer:
                    value: No
                    text: Give me something familiar
                - answer:
                    value: NoPref
                    text: I really don't care
...";
    }
}