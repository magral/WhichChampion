using System;
using ReactiveUI;
using Avalonia.Controls;
using System.Collections.Generic;

namespace ChampionSelection
{
    class MainWindowViewModel : ReactiveObject
    {
        StackPanel _panel;
        List<IControl> _radioButtons;
        TextBlock answerBox;

        public MainWindowViewModel(StackPanel panel)
        {
            _panel = panel;
            _radioButtons = new List<IControl>();

            //Command to submit answers
            SubmitAnswers = ReactiveCommand.Create(() =>
            {
                foreach(RadioButton rbtn in _radioButtons)
                {
                    if ( (bool) rbtn.IsChecked) {
                        Program.championList = ChampionUtil.FilterChampions(rbtn.Name.Split(' ')[0], rbtn.Name.Split(' ')[1], Program.championList);
                    }
                }
                Random rand = new Random();
                answerBox.Text += Program.championList[rand.Next(Program.championList.Count)].Name;
            });
            //Create UI For questions and add to window
            foreach (QuestionObj q in Program.questionList.questions)
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
            };

            //Add submit button and answer display to main panel
            _panel.Children.Add(submitButton);
            _panel.Children.Add(answerBox);
        }

        //Submit command accessor
        public ReactiveCommand SubmitAnswers { get; }


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
