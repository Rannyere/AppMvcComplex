using System;
using FluentValidation;
using FluentValidation.Results;
using IO.Business.Models;

namespace IO.Business.Services
{
    public abstract class BaseService
    {
        //protected BaseService(INotificador notificador)
        //{
        //    _notificador = notificador;
        //}

        protected void Notificacion(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificacion(error.ErrorMessage);
            }
        }

        protected void Notificacion(string mensagem)
        {
            //_notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notificacion(validator);

            return false;
        }
    }
}
