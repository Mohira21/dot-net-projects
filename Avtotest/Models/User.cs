namespace Models;

public class User
{
    public long ChatId;
    public string Name;
    public int Step;

    public User(long chatId, string name)
    {
        ChatId = chatId;
        Name = name;
        Step = 0;
    }

    public User(long chatId, string name, int step)
    {
        ChatId = chatId;
        Name = name;
        Step = step;
    }

    public void SetStep(int step)
    {
        Step = step;
    }

    public string ToText()
    {
        return $"Id : {ChatId}, Ismi : {Name}, step : {Step}";
    }
}