using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace ChampionSelector
{
    //Serializes Data.yaml
    public class QuestionsDeserializer
    {
        private readonly string _document;
        
        public QuestionsDeserializer(string document)
        {
            _document = document;
        }

        public QuestionList ParseDocument()
        {
            var input = new StringReader(_document);
            var deserializerBuilder = new DeserializerBuilder();
            var deserializer = deserializerBuilder.Build();
            return deserializer.Deserialize<QuestionList>(input);;
        }
    }

    public class QuestionList
    {
        public List<QuestionObj> questions { get; set; }
    }
        
    public class QuestionObj
    {
        public string symbol { get; set; }
        public string question { get; set; }
        public List<AnswerList> answers { get; set; }  
    }

    public class AnswerList
    {
        public AnswerObj answer { get; set; }
    }

    public class AnswerObj
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}