﻿using System;
using OLT.Core;

namespace OLT.Email
{
    public interface IOltEmailRequest 
    {
        Guid EmailUid { get; }
        OltEmailRecipients Recipients { get; }
    }
}