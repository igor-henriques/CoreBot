namespace CoreBot.Utils;

public class TranslateToModel
{
    public static async ValueTask<IBaseLogModel> GeneratePickupItemLog(string log)
    {
        ////用户1090丢弃包裹1个60007

        var model = new ItemPickedup();

        model.Date = DateTime.Now;
        model.ItemId = 123;

        return await ValueTask.FromResult(model);
    }

    public static async ValueTask<IBaseLogModel> GenerateDropItemLog(string log)
    {
        ////用户1090丢弃包裹1个60007

        var model = new ItemDropped();

        model.Date = DateTime.Now;
        model.ItemId = 123;

        return await ValueTask.FromResult(model);
    }
}
