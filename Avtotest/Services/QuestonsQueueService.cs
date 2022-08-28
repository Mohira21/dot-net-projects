using Models;
using QuizApp.Bot.Console.Models;

namespace QuizApp.Bot.Console.Services;

public class QuestionsQueueService
{
    public Dictionary<long, Queue<QuestionEntity>> QuestionsQueue = new Dictionary<long, Queue<QuestionEntity>>();
    public QuestionsDataService QuestionsDataService;

    public QuestionsQueueService(QuestionsDataService questionsDataService)
    {
        QuestionsDataService = questionsDataService;
    }

    public void CreateTicket(long chatId)
    {

    }
}