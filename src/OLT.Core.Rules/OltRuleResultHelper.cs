﻿namespace OLT.Core
{
    public static class OltRuleResultHelper
    {
        public static IOltResult Success => new OltResultSuccess();
        public static IOltResultValidation Valid => new OltResultValid();
    }
}