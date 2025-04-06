using Volo.Abp.Settings;

namespace RMG.EventRegistration.Settings;

public class EventRegistrationSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EventRegistrationSettings.MySetting1));
    }
}
