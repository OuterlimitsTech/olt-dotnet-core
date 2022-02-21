using System;
using OLT.Core;

namespace OLT.Email
{
    public interface IOltSmtpConfiguration : IOltEmailConfiguration
    {
        IOltSmtpServer Smtp { get; }
    }

    public interface IOltSmtpServer
    {
        string Server { get; }
        bool DisableSsl { get; }
        short? Port { get; }
    }

    public interface IOltSmtpServerCredentials
    {
        string Username { get; }
        string Password { get; }
    }
}