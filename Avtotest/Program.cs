using User = Models.User;
using QuizApp.Bot.Console.Services;
using Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var db = new QuestionsDataService();
var bot = new TelegramBotService();
var userDb = new UsersDatabase();
var questionsQueue = new QuestionsQueueService(db);

bot.GetUpdate((_, update, _) => Task.Run(() => GetUpdate(update)));

Console.ReadKey();

void GetUpdate(Update update)
{
    if (update.Type == UpdateType.CallbackQuery) HandleCallback(update);

    if (update.Type != UpdateType.Message) return;

    var user = GetUser(update);
    var message = update.Message!.Text;

    switch (user.Step)
    {
        case 0: ShowMenu(user); break;
        case 1: SwitchMenu(user, message!); break;
        default: ShowMenu(user); break;
    }
}

User GetUser(Update update)
{
    var chatId = update.Message!.Chat.Id;

    var name = string.IsNullOrEmpty(update.Message!.From!.Username)
        ? update.Message.From.FirstName
        : "@" + update.Message.From.Username;

    var user = userDb.AddUser(chatId, name);
    return user;
}
void HandleCallback(Update update)
{
    var data = update.CallbackQuery!.Data!.Split(',').Select(int.Parse).ToArray();
    var answer = db.CheckAnswer(data[0], data[1]);

    var msgId = update.CallbackQuery.Message!.MessageId;
    var chatID = update.CallbackQuery.Message.Chat.Id;
    var reply = update.CallbackQuery.Message.ReplyMarkup;

    bot.EditMessageReplyMarkup(chatID, msgId, SetResultToButton(reply!, data[1], answer));
    SendQuestion(chatID, data[0] + 1);
}

void SwitchMenu(User user, string message)
{
    switch (message)
    {
        case "Tickets": bot.SendMessage(user.ChatId, "Coming soon..."); break;
        case "Examination":; break;
    }

}

void SendQuestion(long chatId, int questionIndex)
{
    var (message, buttons) = GetQuestionMessage(questionIndex);
    bot.SendPhoto(chatId, db.GetQuestionImageStream(questionIndex), message, buttons);
}

Tuple<string, InlineKeyboardMarkup> GetQuestionMessage(int index)
{
    var question = db.Questions![index];
    var choicesText = question.Choices!.Select(c => c.Text).ToList();
    return new(question.Question!, GetInlineButtons(choicesText!, index));
}

InlineKeyboardMarkup GetInlineButtons(List<string> buttonsText, int? questionIndex = null)
{
    var buttons = new InlineKeyboardButton[buttonsText.Count][];

    for (int i = 0; i < buttons.Length; i++)
    {
        var callBackData = questionIndex == null ? null : questionIndex + ",";
        buttons[i] = new[] { InlineKeyboardButton.WithCallbackData(text: buttonsText[i], callbackData: $"{callBackData}{i}") };
    }

    return new InlineKeyboardMarkup(buttons);
}

InlineKeyboardMarkup SetResultToButton(InlineKeyboardMarkup buttons, int choiceIndex, bool answer)
{
    var buttonsList = buttons.InlineKeyboard.ToList();
    buttonsList[choiceIndex].ToList()[0].Text += answer ? " ✅" : " ❌";

    return buttons;
}

void ShowMenu(User user)
{
    var message = "Tanlang";
    var buttons = new KeyboardButton[2][];
    buttons[0] = new[] { new KeyboardButton("Tickets") };
    buttons[1] = new[] { new KeyboardButton("Examination") };
    user.SetStep(1);
    bot.SendMessage(user.ChatId, message, new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true });
}