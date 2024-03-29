﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace box.api.Request
{
    public class BodyStorageRequest
    {
        /// <summary>
        /// File
        /// </summary>
        [Required]
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public IFormFile File { get; set; }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

        /// <summary>
        /// Project
        /// </summary>
        [Required]
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public string Project { get; set; }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

        public class BodyStorageRequestValidator : AbstractValidator<BodyStorageRequest>
        {
            public BodyStorageRequestValidator()
            {
                RuleFor(x => x.File).NotNull();
                RuleFor(x => x.Project).NotEmpty();
            }
        }
    }
}