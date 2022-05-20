namespace CoreBot.Domain.Interfaces;

public interface IBaseLogModel
{
    DateTime Date { get; set; }
    string ToString();
}
