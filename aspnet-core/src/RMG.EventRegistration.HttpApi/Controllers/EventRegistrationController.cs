using RMG.EventRegistration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace RMG.EventRegistration.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EventRegistrationController : AbpControllerBase
{
    protected EventRegistrationController()
    {
        LocalizationResource = typeof(EventRegistrationResource);
    }
}
