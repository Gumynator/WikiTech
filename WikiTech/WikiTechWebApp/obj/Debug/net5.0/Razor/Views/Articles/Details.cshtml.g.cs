#pragma checksum "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "677e089dc199257740f4269e3d575dee50a2a553"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Articles_Details), @"mvc.1.0.view", @"/Views/Articles/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\_ViewImports.cshtml"
using WikiTechWebApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\_ViewImports.cshtml"
using WikiTechWebApp.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
using WikiTechWebApp.Areas.Identity.Data;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"677e089dc199257740f4269e3d575dee50a2a553", @"/Views/Articles/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9d1102c9b6673538f36e809ead68f4d91b7c784d", @"/Views/_ViewImports.cshtml")]
    public class Views_Articles_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WikiTechAPI.Models.Article>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 15 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
   ViewData["Title"] = "Details"; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "677e089dc199257740f4269e3d575dee50a2a5533992", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 19 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
     if (Model.IsqualityArticle == true)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <p> C\'est un article de qualité, vous devez être abonné pour le voir. Cette fonctionnalité est en cours de développement !</p>\r\n");
#nullable restore
#line 22 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <!-- Page content-->\r\n        <div class=\"container\">\r\n            <div class=\"row\">\r\n                <!-- Post content-->\r\n                <div class=\"col-lg-8\">\r\n                    <!-- Title-->\r\n                    <h1 class=\"mt-4\">");
#nullable restore
#line 31 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                                Write(Html.DisplayFor(model => model.TitreArticle));

#line default
#line hidden
#nullable disable
                WriteLiteral("</h1>\r\n                    <!-- Author-->\r\n                    <p class=\"lead\">\r\n                        by\r\n                        <a href=\"#!\">");
#nullable restore
#line 35 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                                Write(Html.DisplayFor(model => model.IdNavigation.PrenomAspnetuser));

#line default
#line hidden
#nullable disable
                WriteLiteral("</a>\r\n                    </p>\r\n                    <hr />\r\n                    <!-- Date and time-->\r\n                    <p>Posted on ");
#nullable restore
#line 39 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                            Write(Html.DisplayFor(model => model.DatepublicationArticle));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</p>
                    <hr />
                    <!-- Preview image
                    <img class=""img-fluid rounded"" src=""https://via.placeholder.com/900x300"" alt=""..."" />
                    <hr />-->
                    <!-- Post content-->
                    <p class=""lead"">");
#nullable restore
#line 45 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                               Write(Html.DisplayFor(model => model.DescriptionArticle));

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\r\n                    <p>");
#nullable restore
#line 46 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                  Write(Html.Raw(Model.TextArticle));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</p>
                    <hr />

                </div>
                <!-- Sidebar widgets column-->
                <div class=""col-md-4"">
                    <!-- Categories widget-->
                    <div class=""card my-4"">
                        <h5 class=""card-header"">Categories</h5>
                        <div class=""card-body"">
                            <div class=""row"">
                                <div class=""col-lg-6"">
                                    <ul class=""list-unstyled mb-0"" id=""tags"">
");
#nullable restore
#line 59 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                                         foreach (var item in Model.Referencer)
                                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                                <script type=\"text/javascript\">\r\n\r\n                                            getTags(");
#nullable restore
#line 63 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                                               Write(Html.Raw(item.IdTag));

#line default
#line hidden
#nullable disable
                WriteLiteral(@");

                                            function getTags(id) {
                                                $.ajax(
                                                    {
                                                        url: ""http://localhost:59601/Api/Tags/"" + id,
                                                        type: ""GET"",
                                                        dataType: ""JSON"",
                                                        contentType: ""application/json"",
                                                        success: function (data) {
                                                            createTagSelect(data);
                                                        }
                                                    });
                                            }

                                                </script>
");
#nullable restore
#line 79 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"
                                            }

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    </ul>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 88 "C:\Users\M4800\Desktop\GITKRAKEN\clonedeloan\WikiTech-Proto\WikiTech\WikiTechWebApp\Views\Articles\Details.cshtml"

     }

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

<script>

    function createTagSelect(data) {
        var select = $(""#tags"");
            console.log(data.nomTag + data.idTag);
        var element = ""<li><a href=\""tags/"" + data.idTag + ""\"">"" + data.nomTag + ""</a></li>"";
            select.append(element);
    }



</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<ApplicationUser> UserManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<ApplicationUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WikiTechAPI.Models.Article> Html { get; private set; }
    }
}
#pragma warning restore 1591
