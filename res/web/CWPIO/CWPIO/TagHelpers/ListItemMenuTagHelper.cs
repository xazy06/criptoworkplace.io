using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWPIO.TagHelpers
{
    [HtmlTargetElement("li", Attributes = ActionAttributeName)]
    [HtmlTargetElement("li", Attributes = ControllerAttributeName)]
    [HtmlTargetElement("li", Attributes = AreaAttributeName)]
    [HtmlTargetElement("li", Attributes = PageAttributeName)]
    [HtmlTargetElement("li", Attributes = PageHandlerAttributeName)]
    [HtmlTargetElement("li", Attributes = FragmentAttributeName)]
    [HtmlTargetElement("li", Attributes = HostAttributeName)]
    [HtmlTargetElement("li", Attributes = ProtocolAttributeName)]
    [HtmlTargetElement("li", Attributes = RouteAttributeName)]
    [HtmlTargetElement("li", Attributes = ActiveClassName)]
    public class ListItemMenuTagHelper : TagHelper
    {
        private const string ActionAttributeName = "active-action";
        private const string ControllerAttributeName = "active-controller";
        private const string AreaAttributeName = "active-area";
        private const string PageAttributeName = "active-page";
        private const string PageHandlerAttributeName = "active-page-handler";
        private const string FragmentAttributeName = "active-fragment";
        private const string HostAttributeName = "active-host";
        private const string ProtocolAttributeName = "active-protocol";
        private const string RouteAttributeName = "active-route";
        private const string ActiveClassName = "active-class";
        private const string Class = "class";


        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        /// <summary>
        /// Creates a new <see cref="AnchorTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public ListItemMenuTagHelper(IUrlHelperFactory urlHelperFactory, IHttpContextAccessor contextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// The name of the action method.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// The name of the area.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        /// <summary>
        /// The name of the page.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Action"/>, <see cref="Controller"/>
        /// or <see cref="Area"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(PageAttributeName)]
        public string Page { get; set; }

        /// <summary>
        /// The name of the page handler.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Action"/>, or <see cref="Controller"/>
        /// is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(PageHandlerAttributeName)]
        public string PageHandler { get; set; }

        /// <summary>
        /// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
        /// </summary>
        [HtmlAttributeName(ProtocolAttributeName)]
        public string Protocol { get; set; }

        /// <summary>
        /// The host name.
        /// </summary>
        [HtmlAttributeName(HostAttributeName)]
        public string Host { get; set; }

        /// <summary>
        /// The URL fragment name.
        /// </summary>
        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        /// <summary>
        /// Name of the route.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if one of <see cref="Action"/>, <see cref="Controller"/>, <see cref="Area"/> 
        /// or <see cref="Page"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        [HtmlAttributeName(ActiveClassName)]
        public string ActiveClass { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <inheritdoc />
        /// <remarks>Does nothing if user provides an <c>href</c> attribute.</remarks>
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

            // If "class" is already set, add active to this.
            if (output.Attributes.ContainsName(Class))
            {
                if (Action != null ||
                    Controller != null ||
                    Area != null ||
                    Page != null ||
                    PageHandler != null ||
                    Route != null ||
                    Protocol != null ||
                    Host != null ||
                    Fragment != null ||
                    ActiveClass != null)
                {
                    var current = output.Attributes[Class];
                }
            }

            var routeLink = Route != null;
            var actionLink = Controller != null || Action != null;
            var pageLink = Page != null || PageHandler != null;

            if ((routeLink && actionLink) || (routeLink && pageLink) || (actionLink && pageLink))
            {
                var message = string.Join(
                    Environment.NewLine,
                    "The following attributes are mutually exclusive:",
                    RouteAttributeName,
                    ControllerAttributeName + ", " + ActionAttributeName,
                    PageAttributeName + ", " + PageHandlerAttributeName);

                throw new InvalidOperationException(message);
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            string url;
            if (pageLink)
                url = urlHelper.Page(Page, PageHandler, null, Protocol, Host, Fragment);
            else if (routeLink)
                url = urlHelper.RouteUrl(Route, null, Protocol, Host, Fragment);
            else
                url = urlHelper.Action(Action, Controller, null, Protocol, Host, Fragment);

            var request = _contextAccessor.HttpContext.Request;
            if (request.Path == url)
            {
                TagBuilder tagBuilder = new TagBuilder("li");
                tagBuilder.MergeAttribute(Class, ActiveClass);
                output.MergeAttributes(tagBuilder);
            }
        }
    }
}
