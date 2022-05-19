namespace CoreBot.Utils;

public class TranslateToModel
{
    public static async ValueTask<IBaseLogModel> GeneratePickupItemLog(string log)
    {
        var model = new PickupItemFromWorldModel();

        model.Date = DateTime.Now;
        model.ItemId = 123;

        return await ValueTask.FromResult(model);
    }
}
