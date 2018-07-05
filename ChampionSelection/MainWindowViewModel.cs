using System;
using ReactiveUI;
using Avalonia.Controls;
using System.Collections.Generic;
using Autofac;
using System.IO;
using Avalonia.Markup.Xaml.Data;
using System.Threading.Tasks;
using Avalonia;
using System.Runtime.InteropServices;

namespace ChampionSelection
{
    class MainWindowViewModel : ReactiveObject
    {
        StackPanel _panel;
        List<IControl> _radioButtons;
        TextBlock answerBox;
        TextBox summonerNameInput;
        QuestionList questionList;

        //Submit command accessor
        public ReactiveCommand SubmitAnswers { get; }
        public NotifyTaskCompletion<string> ChampionName { get; private set; }

        public MainWindowViewModel(StackPanel panel)
        {
            _panel = panel;
            _radioButtons = new List<IControl>();
            GetQuestions();

            //Command to submit answers
            SubmitAnswers = ReactiveCommand.Create(() =>
            {
                //In an ideal world, we'd have a separate page to input the user summoner name, and pre-load all the champions there
                //For now though, we are just going to load the champions upon submitting everything.
                var championGetter = Program.Container.Resolve<IAPIMessages>();
                string summonerName = summonerNameInput.Text.ToLower().Replace(" ", string.Empty);
                ChampionName = new NotifyTaskCompletion<string>(championGetter.GetChampionResult(summonerName, _radioButtons));
                Binding Champions = new Binding("ChampionName.Result", Avalonia.Data.BindingMode.Default);
                answerBox.Bind(TextBox.TextProperty, Champions);
            });
            //Create UI For questions and add to window
            foreach (QuestionObj q in questionList.questions)
            {
                LoadUIForQuestion(q);
            }

            //Create submit button
            Button submitButton = new Button
            {
                Content = "Submit",
                Command = SubmitAnswers,
            };

            //Display box for answer
            answerBox = new TextBlock
            {
                Name = "Answer",
                Text = "",
            };

            //Input for summoner name
            summonerNameInput = new TextBox
            {
                Name = "Summoner",
                Text = "Enter Summoner Name Here",
            };

            //Add submit button and answer display to main panel
            _panel.Children.Add(summonerNameInput);
            _panel.Children.Add(submitButton);
            _panel.Children.Add(answerBox);
        }

        private void GetQuestions()
        {
            var questionGetter = Program.Container.Resolve<IQuestionDeserializer>();
            //TODO: Add Mac file support here
            string questionDocument;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                questionDocument = File.ReadAllText(Directory.GetCurrentDirectory() + "/Data.yaml");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                questionDocument = File.ReadAllText(Directory.GetCurrentDirectory() + "..\\..\\..\\..\\Data.yaml");
            }
            else
            {
                questionDocument = File.ReadAllText(Directory.GetCurrentDirectory() + "/Data.yaml");
            }
            questionList = questionGetter.CreateQuestionList(questionDocument);
        }
        //Helper methods to build UI 
        //---------------------
        public void LoadUIForQuestion(QuestionObj question)
        {
            StackPanel p = new StackPanel();
            CreateTextBlock(p, question.question);
            foreach(AnswerObj ans in question.answers)
            {
                CreateRadioButton(p, ans.answer.value + " " + question.symbol, ans.answer.text);
            }
            _panel.Children.Add(p);
        }

        private void CreateTextBlock(StackPanel p, string text)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
            };
            p.Children.Add(textBlock);
        }

        private void CreateRadioButton(StackPanel p, string value, string displayText)
        {
            RadioButton btn = new RadioButton {
                Name = value,
                Content = displayText,
            };
            _radioButtons.Add(btn);
            p.Children.Add(btn);
        }
        //----------------------
    }
}
