﻿using Microsoft.Extensions.Localization;
using RMG.EventRegistration.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace RMG.EventRegistration;

[Dependency(ReplaceServices = true)]
public class EventRegistrationBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<EventRegistrationResource> _localizer;

    public EventRegistrationBrandingProvider(IStringLocalizer<EventRegistrationResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
