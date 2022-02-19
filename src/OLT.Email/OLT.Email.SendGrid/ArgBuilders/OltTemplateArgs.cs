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
        public T WithTemplate(IOltEmailTemplate template)
        {
            this.Template = template;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(Template?.TemplateName))
            {
                errors.Add("SendGrid Template ID Missing");
            }
            return errors;
        }
    }
}
