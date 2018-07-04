using System;
using ReactiveUI;
using Avalonia;
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
            SubmitAnswers = ReactiveCommand.Create(() =>
            {
                foreach(RadioButton rbtn in _radioButtons)
                {
                    if ( (bool) rbtn.IsChecked) {
                        Program.championList = ChampionUtil.FilterChampions(rbtn.Name.Split(' ')[0], rbtn.Name.Split(' ')[1], Program.championList);
                    }
                }
                foreach (Champion c in Program.championList)
                {
                    answerBox.Text += c.Name;
                }
            });
            foreach (QuestionObj q in Program.questionList.questions)
            {
                LoadUIForQuestion(q);
            }
            Button submitButton = new Button
            {
                Content = "Submit",
                Command = SubmitAnswers,
            };

            answerBox = new TextBlock
            {
                Name = "Answer",
            };
            _panel.Children.Add(submitButton);
            _panel.Children.Add(answerBox);
        }
        public ReactiveCommand SubmitAnswers { get; }

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
    }
}
