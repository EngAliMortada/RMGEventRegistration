using RMG.EventRegistration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace RMG.EventRegistration.Permissions;

public class EventRegistrationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EventRegistrationPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EventRegistrationPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EventRegistrationResource>(name);
    }
}
