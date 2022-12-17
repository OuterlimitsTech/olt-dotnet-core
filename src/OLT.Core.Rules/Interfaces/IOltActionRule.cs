﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.Core
{
    [Obsolete("Move to OltCommandBus")]
    public interface IOltActionRule : IOltRule
    {
        Task<List<OltRuleCanRunException>> CanExecuteAsync();
        Task ExecuteAsync<TContext>(TContext context) where TContext : DbContext, IOltDbContext;
        Task ExecuteAsync(IDbContextTransaction dbTransaction);
    }
}