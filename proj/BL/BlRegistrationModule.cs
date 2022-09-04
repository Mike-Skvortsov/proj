﻿using Autofac;
using BL.Services;
using DataAccess;

namespace BL
{
    public class BlRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessRegistrationModule>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CardService>().As<ICardService>();
            builder.RegisterType<OperationService>().As<IOperationService>();
        }
    }
}
