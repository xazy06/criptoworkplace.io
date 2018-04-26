using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace CWPIO.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;environment&gt; elements that conditionally renders
    /// content based on the current value of <see cref="IHostingEnvironment.EnvironmentName"/>.
    /// If the environment is not listed in the specified <see cref="Names"/> or <see cref="Include"/>, 
    /// or if it is in <see cref="Exclude"/>, the content will not be rendered.
    /// </summary>
    public class LocaleTagHelper : TagHelper
    {
        private static readonly char[] NameSeparator = new[] { ',' };
        private readonly IHttpContextAccessor _contextAccessor;
        public LocaleTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        
        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// A comma separated list of environment names in which the content should be rendered.
        /// If the current environment is also in the <see cref="Exclude"/> list, the content will not be rendered.
        /// </summary>
        /// <remarks>
        /// The specified environment names are compared case insensitively to the current value of locale
        /// </remarks>
        public string Include { get; set; }

        /// <summary>
        /// A comma separated list of environment names in which the content will not be rendered.
        /// </summary>
        /// <remarks>
        /// The specified locale names are compared case insensitively to the current value of locale
        /// </remarks>
        public string Exclude { get; set; }
        
        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            // Always strip the outer tag name as we never want <environment> to render
            output.TagName = null;

            if (string.IsNullOrWhiteSpace(Include) && string.IsNullOrWhiteSpace(Exclude))
            {
                // No names specified, do nothing
                return;
            }

            var httpContext = _contextAccessor.HttpContext;
            var currentLocale = httpContext.GetCurrentCulture();
            if (string.IsNullOrEmpty(currentLocale))
            {
                // No current environment name, do nothing
                return;
            }

            if (Exclude != null)
            {
                var tokenizer = new StringTokenizer(Exclude, NameSeparator);
                foreach (var item in tokenizer)
                {
                    var locale = item.Trim();
                    if (locale.HasValue && locale.Length > 0)
                    {
                        if (locale.Equals(currentLocale, StringComparison.OrdinalIgnoreCase))
                        {
                            // Matching environment name found, suppress output
                            output.SuppressOutput();
                            return;
                        }
                    }
                }
            }

            var hasEnvironments = false;

            if (Include != null)
            {
                var tokenizer = new StringTokenizer(Include, NameSeparator);
                foreach (var item in tokenizer)
                {
                    var locale = item.Trim();
                    if (locale.HasValue && locale.Length > 0)
                    {
                        hasEnvironments = true;
                        if (locale.Equals(currentLocale, StringComparison.OrdinalIgnoreCase))
                        {
                            // Matching environment name found, do nothing
                            return;
                        }
                    }
                }
            }

            if (hasEnvironments)
            {
                // This instance had at least one non-empty environment (names or include) specified but none of these
                // environments matched the current environment. Suppress the output in this case.
                output.SuppressOutput();
            }
        }
    }
}
