namespace ClassLibrary1
{
    public class Command
    {
        public string Message { get; set; } = "Ok";
    }

    public class CommandHandler
    {
        public void Handle(Command command)
        {
            Console.WriteLine(command.Message); 
        }
    }   
}
