using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Core.Commands
{
    public abstract class CommandBase : ICommand
    {
        private readonly List<string> _errors;
        private bool _validationPerformed;
        public bool IsValid => _validationPerformed && !_errors.Any();

        public IEnumerable<string> ValidationErrorMessges => _errors;


        protected CommandBase()
        {
            _errors = new List<string>();
            _validationPerformed = false;
        }

        protected virtual IEnumerable<string> OnValidation()
        {
            return Enumerable.Empty<string>();
        }

        public void Validate()
        {
            _errors.AddRange(OnValidation());
            _validationPerformed = true;
        }
    }
}
