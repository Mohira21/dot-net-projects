using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace QuizApp.Bot.Console.Services
{
    public class TelegramBotService
    {
        private static string Token = "5542868289:AAGIH1AAFKYH71fd87uaObPAY1n0WHzMTcU";
        private TelegramBotClient Bot;

        public TelegramBotService()
        {
            Bot = new TelegramBotClient(Token);
        }

        public void GetUpdate(Func<ITelegramBotClient,Update,CancellationToken,Task> update)
        {
            Bot.StartReceiving(
                updateHandler: update,
                errorHandler : (_,ex,_) =>
                {
                    System.Console.WriteLine(ex.Message);
                    return Task.CompletedTask;
                });
        }

        public void SendMessage(long chatId, string messageText, IReplyMarkup? reply = null)
        {
            Bot.SendTextMessageAsync(chatId,messageText, replyMarkup: reply);
        }

        public void SendPhoto(long chatId, Stream? stream)
        {
            if(stream == null) return;
            Bot.SendPhotoAsync(chatId, new InputOnlineFile(stream));
        }
        public void SendPhoto(long chatId, Stream? stream, string messageText, IReplyMarkup? reply = null)
        {
            if(stream != null)
                Bot.SendPhotoAsync(chatId, new InputOnlineFile(stream), messageText, replyMarkup: reply);
        }

        public void EditMessageReplyMarkup(long chatId, int messageId, InlineKeyboardMarkup? reply)
        {
            Bot.EditMessageReplyMarkupAsync(chatId, messageId, replyMarkup: reply);
        }
    }
    
}