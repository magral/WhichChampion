using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace ChampionSelection
{
    public interface IQuestionDeserializer
    {
        QuestionList CreateQuestionList(string document);
    }

    //Serializes Data.yaml
    public class QuestionsDeserializer : IQuestionDeserializer
    {
        public QuestionList CreateQuestionList(string _document)
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
        public List<AnswerObj> answers { get; set; }  
    }

    public class AnswerObj
    {
        public AnswerValues answer { get; set; }
    }

    public class AnswerValues
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}