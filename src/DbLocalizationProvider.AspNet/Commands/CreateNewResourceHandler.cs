﻿// Copyright (c) Valdis Iljuconoks. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System;
using System.Linq;
using DbLocalizationProvider.Abstractions;
using DbLocalizationProvider.Commands;

namespace DbLocalizationProvider.AspNet.Commands
{
    public class CreateNewResourceHandler : ICommandHandler<CreateNewResource.Command>
    {
        public void Execute(CreateNewResource.Command command)
        {
            if(string.IsNullOrEmpty(command.Key)) throw new ArgumentNullException(nameof(command.Key));

            using(var db = new LanguageEntities())
            {
                var existingResource = db.LocalizationResources.FirstOrDefault(r => r.ResourceKey == command.Key);
                if(existingResource != null) throw new InvalidOperationException($"Resource with key `{command.Key}` already exists");

                db.LocalizationResources.Add(new LocalizationResource(command.Key)
                                             {
                                                 ModificationDate = DateTime.UtcNow,
                                                 FromCode = command.FromCode,
                                                 IsModified = false,
                                                 Author = command.UserName
                                             });

                db.SaveChanges();
            }
        }
    }
}
