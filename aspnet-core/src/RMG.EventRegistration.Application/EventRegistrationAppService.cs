using System;
using System.Collections.Generic;
using System.Text;
using RMG.EventRegistration.Localization;
using Volo.Abp.Application.Services;

namespace RMG.EventRegistration;

/* Inherit your application services from this class.
 */
public abstract class EventRegistrationAppService : ApplicationService
{
    protected EventRegistrationAppService()
    {
        LocalizationResource = typeof(EventRegistrationResource);
    }
}
