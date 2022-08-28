using QuizApp.Bot.Console.Models;
using Newtonsoft.Json;

namespace QuizApp.Bot.Console.Services
{
    public class QuestionsDataService
    {
        private string JsonFilePath = "JsonData/uzlotin.json";
        private string ImagesPath = "Images";
        public List <QuestionEntity>? Questions {get; set;}

        public QuestionsDataService()
        {
            var jsonText = File.ReadAllText(JsonFilePath);
            Questions = JsonConvert.DeserializeObject<List<QuestionEntity>>(jsonText);
        }

        public Stream ? GetQuestionImageStream(int index)
        {
            if (!Questions![index].Media!.Exist) return null;

            var image = Questions[index].Media!.Name;
            var bytes = File.ReadAllBytes($"{ImagesPath} / {image}.png");
            return new MemoryStream(bytes);
        }

        public bool CheckAnswer(int questionIndex, int choiceIndex)
        {
            var question = Questions![questionIndex];
            var choice = question.Choices![choiceIndex];
            return choice.Answer;
        }
    }
}