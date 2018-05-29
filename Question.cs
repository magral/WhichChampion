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
        public string symbol { get; set; }
        public string question { get; set; }
        public List<string> answers { get; set; }
         
    }
   
}