using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace box.api.Request
{
    public class BodyProjectRequest
    {
        /// <summary>
        /// File
        /// </summary>
        [Required]
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public string ProjectName { get; set; }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

        /// <summary>
        /// Project
        /// </summary>
        [Required]
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public string ProjectCode { get; set; }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

        public class BodyStorageRequestValidator : AbstractValidator<BodyProjectRequest>
        {
            public BodyStorageRequestValidator()
            {
                RuleFor(x => x.ProjectName).NotEmpty();
                RuleFor(x => x.ProjectCode).NotEmpty();
            }
        }
    }
}
