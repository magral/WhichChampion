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
            foreach(QuestionObj q in questionList.questions)
            {
                Console.WriteLine(q.question);
                foreach (string answer in q.answers)
                {
                    Console.WriteLine(answer);
                }
                string input = (Console.ReadLine());
                LogQuestion(ans, q.symbol, input);
            }
            Console.WriteLine(ChampionUtil.FilterCrewByCriteria(ans, champData));
        }

        private static void LogQuestion(Answer ans, string questionSymbol, string answer)
        {
            if (questionSymbol == LaneQuestion)
            {
                Lane.TryParse(answer, out Lane lane);
                ans.lane = lane;
            }
            else if (questionSymbol == RoleQuestion)
            {
                Role.TryParse(answer, out Role role);
                ans.role = role;
            }
            else if (questionSymbol == DamageQuestion)
            {
                DamageType.TryParse(answer, out DamageType dmg);
                ans.dmgType = dmg;
            }
            else if (questionSymbol == NewnessQuestion)
            {
                IsNew.TryParse(answer, out IsNew n);
                ans.isNew = n;
            }
        }
        
        private const string Document = @"---
        questions:
            - symbol: LaneQuestion
              question: What lane do you feel like playing?
              answers:
                - Top
                - Jungle
                - Mid
                - Bottom
                - Support
            - symbol: RoleQuestion
              question: What style do you want to play?
              answers:
                - Ranged
                - Bruiser
                - Tank
            - symbol: DamageQuestion
              question: Preference on damage type?
              answers:
                - AP
                - AD
                - Hybrid
                - Don't Care
            - symbol: NewnessQuestion
              question: Do you want to try something new, or play something you know?
              answers:
                - Try something new!
                - Give me something familiar
                - I really don't care
...";
    }
}