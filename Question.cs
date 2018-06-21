using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace ChampionSelector
{
    public class Questions
    {
        public List<QuestionObj> questions { get; }

        public Questions(string document)
        {
            var input = new StringReader(document);
            var deserializerBuilder = new DeserializerBuilder();
            var deserializer = deserializerBuilder.Build();
            deserializer.Deserialize<Questions>(input);
        }
    }
        
    public class QuestionObj
    {
        public string Symbol { get; set; }
        public string Question { get; set; }
        public List<AnswerObj> Answers { get; set; }  
    }

    public class AnswerObj
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}