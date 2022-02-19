////using Microsoft.Extensions.DependencyInjection;
////using System.Collections.Generic;
////using System.Text;

////namespace OLT.Email.SendGrid
////{

////    public static class OltEmailSendGridExtensions
////    {
////        public static OltSendGridArgs BuildArgs(this IOltEmailConfigurationSendGrid configuration, bool disableClickTracking = false)
////        {
////            var args = new OltSendGridArgs()               
////                .WithFromEmail(configuration.From)
////                .WithWhitelist(configuration.TestWhitelist)
////                .WithApiKey(configuration.ApiKey)
////                .IsProduction(configuration.Production);

////            if (disableClickTracking)
////            {
////                args.WithoutClickTracking();
////            }

////            return args;
////        }


////        //public static IServiceCollection ConfigureEmailSendGridWithWhitelist<TConfig>(this IServiceCollection services, TConfig configuration) where TConfig : class, IOltEmailConfigurationSendGrid
////        //{
////        //    var args = new OltEmailArgs()
////        //        .WithFromEmail(configuration.From.Email, configuration.From.Name)
////        //        .WithWhitelist(configuration.TestWhitelist)
////        //        .IsProduction(configuration.Production);

////        //    services.AddSingleton(args);

////        //    return services;

////        //}

////        //public static IServiceCollection ConfigureEmailSendGridWithWhitelist(this IServiceCollection services, OltEmailArgs args)
////        //{
////        //    services.AddSingleton(args);
////        //    return services;
////        //}
////    }
////}
