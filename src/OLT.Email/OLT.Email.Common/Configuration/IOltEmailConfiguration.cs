﻿using System.Collections.Generic;
using OLT.Core;

// ReSharper disable once CheckNamespace
namespace OLT.Email
{
    public interface IOltEmailConfiguration 
    {
        /// <summary>
        /// Default From Address if not specified in Sned
        /// </summary>
        OltEmailAddress From { get; }

        /// <summary>
        /// Test Environment whitelist 
        /// </summary>                
        OltEmailConfigurationWhitelist TestWhitelist { get; }

        /// <summary>
        /// Running in Production and send all emails if true
        /// </summary>
        bool Production { get; }


    }
}