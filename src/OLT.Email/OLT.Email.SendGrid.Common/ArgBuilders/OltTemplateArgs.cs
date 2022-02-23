using System;
using System.Collections.Generic;

namespace OLT.Email.SendGrid
{

    public abstract class OltTemplateArgs<T> : OltUnsubscribeGroupArgs<T>
        where T : OltTemplateArgs<T>
    {
        protected internal IOltEmailTemplate Template { get; set; }
        
        protected OltTemplateArgs()
        {
        }

        /// <summary>
        /// Send Grid Template
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithTemplate(IOltEmailTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            this.Template = template;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(Template?.TemplateId))
            {
                errors.Add(OltArgErrorsSendGrid.TemplateId);
            }
            return errors;
        }
    }
}
