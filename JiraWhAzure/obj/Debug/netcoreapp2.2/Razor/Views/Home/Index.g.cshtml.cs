#pragma checksum "C:\Users\User\source\repos\JiraWhAzure\JiraWhAzure\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "714fd3a3f756c1c8e56037545a72277734458781"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 1 "C:\Users\User\source\repos\JiraWhAzure\JiraWhAzure\Views\_ViewImports.cshtml"
using JiraWhAzure;

#line default
#line hidden
#line 2 "C:\Users\User\source\repos\JiraWhAzure\JiraWhAzure\Views\_ViewImports.cshtml"
using JiraWhAzure.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"714fd3a3f756c1c8e56037545a72277734458781", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f098cf00c9e446a4b37fff04da40b512d7113930", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\User\source\repos\JiraWhAzure\JiraWhAzure\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(45, 103, true);
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <ul class=\"list-group\" id=\"messagesList\">\r\n    </ul>\r\n</div>\r\n\r\n\r\n\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(166, 816, true);
                WriteLiteral(@"
    <script src=""lib/dist/browser/signalr.min.js""></script>
    <script>
        var connection = new signalR.HubConnectionBuilder()
            .withUrl(""/jirahook"")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        console.log(connection);

        connection.on(""BroadcastMessage"", (url) => {
            console.log(url);
            var msg = ""New Issue at Github!!! Issue Title : "" + url;
            var li = document.createElement(""li"");
            li.classList.add(""list-group-item"");
            li.textContent = msg;
            document.getElementById(""messagesList"").appendChild(li);
        });

        connection.start().catch(function (err) {
            return console.error(err.toString());
        });


        
    </script>
");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
